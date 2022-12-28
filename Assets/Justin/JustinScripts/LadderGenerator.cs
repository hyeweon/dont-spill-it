using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LadderGenerator : MonoBehaviour
{
    [System.Serializable]
    private struct positionOrderData
    {
        public int playerCount;

        public float xDis;
        public float maxHorizontal;

        public float zDis;
        public float maxVertical;
    }

    [SerializeField] private positionOrderData posData = new positionOrderData();
    [SerializeField] private GameObject brickOBJ;
    [SerializeField] private Material[] mats;

    [SerializeField] private int[] curColorsCount;
    [SerializeField] private int maxColorsCount;
    [SerializeField] private int[] colorsLayer;

    [SerializeField] private int totalGenerateCount;
    [SerializeField] private int curGenerateCount;

    private void Reset()
    {
        posData.playerCount = 4;

        posData.xDis = 2;
        posData.maxHorizontal = 20;

        posData.zDis = -2;
        posData.maxVertical = 20;

        curColorsCount = new int[posData.playerCount];
        colorsLayer = new int[posData.playerCount];


        // 14번 부터 17까지가 색상별 NPC 레이어
        int layer = 14;
        for (int i = 0; i < 4; i++)
        {
            colorsLayer[i] = layer;
            layer++;
        }
    }

    private void Start()
    {
        curColorsCount = new int[posData.playerCount];

        var maxGenerate = (Mathf.CeilToInt(posData.maxHorizontal * posData.maxVertical));

        maxColorsCount = maxGenerate / posData.playerCount;
        totalGenerateCount = (maxGenerate % 2) >= 1 ? maxGenerate - 1 : maxGenerate;

        GenerateLadder();
    }

    private void GenerateLadder()
    {
        float increasX = 0;
        float increasZ = 0;

        for (int i = 0; i < posData.maxVertical; i++)
        {
            for (int j = 0; j < posData.maxHorizontal; j++)
            {
                var instancePos = new Vector3((transform.position.x + increasX),
                    transform.position.y, transform.position.z + increasZ);

                GameObject go = Instantiate(brickOBJ, instancePos, brickOBJ.transform.rotation, transform);
                ChangeColor(go);

                increasX += posData.xDis;
            }
            increasZ += posData.zDis;
            increasX = 0;
        }
    }

    private void ChangeColor(GameObject go)
    {
        int ran = Random.Range(0, mats.Length);

        // 제작끝
        if (curGenerateCount >= totalGenerateCount)
            return;

        // 현재 색상은 원하는 만큼 모두 생성되었으니 다시 돌면서 다른 컬러를 찾아라
        if (curColorsCount[ran] == maxColorsCount)
        {
            ChangeColor(go);
            return;
        }

        go.GetComponent<LadderBack>().colorChangeAction?.Invoke(mats[ran]);
        go.layer = colorsLayer[ran];

        curGenerateCount++;
        curColorsCount[ran]++;
    }
}
