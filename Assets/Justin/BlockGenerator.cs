using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private  GameObject blockOBJ;

    private float yDis = 0;
    [SerializeField] private int count = 0;
    [SerializeField] private int increasNum = 0;


    private void Reset()
    {
        count = 40;
        blockOBJ = Resources.Load<GameObject>("Block");
    }

    public void DestoryAll()
    {
        if(transform.childCount <= 0)
        {
            print("삭제할게 없습니다 ㅠ.ㅠ");
            return;
        }

        Transform[] trs = transform?.GetComponentsInChildren<Transform>();
        
        GameObject[] gos = new GameObject[trs.Length];
        int count = 0;
        for (int i = 1; i < trs.Length; i++)
        {
            if (trs[i].TryGetComponent<BoxCollider>(out var box))
                gos[++count] = trs[i].gameObject;
        }

        foreach(var item in gos)
        {
            DestroyImmediate(item);
        }

    }

    public void GenerateBrick()
    {
        if (transform.childCount > 0)
            DestoryAll();

        yDis = blockOBJ.GetComponent<Renderer>().bounds.size.y;

        float upYPos = 0;
        float num = 0;

        for (int i = 0; i < count; i++)
        {
            var pos = new Vector3(transform.position.x, transform.position.y + upYPos, transform.position.z);
            GameObject go =  Instantiate(blockOBJ, pos, Quaternion.identity, transform);
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{num}";
            upYPos = upYPos + yDis;
            num = num + increasNum;
        }
    }
}
