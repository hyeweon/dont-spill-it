using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlingMakeLadder : MonoBehaviour
{
    [SerializeField] GameObject ladderObj;
    [SerializeField] GameObject lastField;
    [SerializeField] List<GameObject> objList;
    [SerializeField] int MakeCount;
    [SerializeField] Material[] color;

    Vector3 NextPos;
    GameObject obj;
    Renderer ren;
    // Start is called before the first frame update
    void Start()
    {
        NextPos = new Vector3(1,-1,0);
        StartCoroutine(makeSlider());
    }

    IEnumerator makeSlider()
    {
        var delay = new WaitForSeconds(0.1f);
        while(MakeCount >=0)
        {
            obj = Instantiate(ladderObj, transform.position + NextPos, Quaternion.Euler(120,90,0));
            objList.Add(obj);
            NextPos += new Vector3(1, -1, 0);
            MakeCount--;
            yield return delay;
        }
        //Instantiate(lastField, transform.position + NextPos, Quaternion.identity);
    }
}
