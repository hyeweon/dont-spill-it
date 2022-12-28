using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockGenerator))]
public class ButtonBlockGenerate : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BlockGenerator generator = (BlockGenerator)target;

        if (GUILayout.Button("Delete All Block"))
        {
            generator.DestoryAll();
        }

        if (GUILayout.Button("Generate Block"))
        {
            generator.GenerateBrick();
        }
    }
}
