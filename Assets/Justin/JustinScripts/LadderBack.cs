using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LadderBack : MonoBehaviour
{
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Transform originParent;
    [SerializeField] private Collider myCol;

    [SerializeField] private Renderer renderColor;
    [SerializeField] private int originLayer;

    public Action<Material> colorChangeAction { get; private set; }
    public Action backToOriginAction { get; private set; }

    private void Awake()
    {
        originPos = transform.position;
        originParent = transform.parent;
        colorChangeAction = ChangeColor;
    }

    WaitForSeconds wfs05 = new WaitForSeconds(0.5f);
    private IEnumerator Start()
    {
        yield return wfs05;
        originLayer = this.gameObject.layer;
        backToOriginAction = BackToOrigin;

    }

    private void BackToOrigin()
    {
        myCol.enabled = true;
        transform.position = originPos;
        transform.SetParent(originParent);
        transform.localRotation = Quaternion.identity;
        gameObject.layer = originLayer;
    }

    private void ChangeColor(Material mat)
    {
        renderColor.material = mat;
    }
}
