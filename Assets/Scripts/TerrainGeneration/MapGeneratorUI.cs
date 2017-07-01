using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorUI : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            mapGen.GenerateMap();
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }

    }
}
