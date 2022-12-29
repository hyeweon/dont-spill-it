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

    [SerializeField] Blocks blocks;


    private void Reset()
    {
        count = 40;
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

        for (int i = 0; i < count; i++)
        {
            var pos = new Vector3(blockOBJ.transform.position.x, blockOBJ.transform.position.y + upYPos, blockOBJ.transform.position.z);
            GameObject go =  Instantiate(blockOBJ, pos, Quaternion.identity, transform);
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText($"{(i + 1) * 5}");

            blocks.blockRenderers[i] = go.GetComponent<Renderer>();

            upYPos = upYPos + yDis;
        }
    }
}
