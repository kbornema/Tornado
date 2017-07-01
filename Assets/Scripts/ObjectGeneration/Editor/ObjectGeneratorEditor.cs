using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (ObjectGenerator))]
public class ObjectGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        ObjectGenerator objEditor = (ObjectGenerator)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            objEditor.GenerateObjects();
        }
    }

}
