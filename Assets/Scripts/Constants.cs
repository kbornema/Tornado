using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    private static Constants instance;

    public static Constants Instance
    {
        get { if (instance == null) instance = new Constants();
            return instance;
        }
    }

    public GameObject MeshObject;
    public Mesh Mesh;

    public Constants()
    {
        UpdateMesh(GameObject.Find("Mesh"));
    }

    public void UpdateMesh(GameObject meshObject)
    {
        MeshObject = meshObject;
        Mesh = MeshObject.GetComponent<MeshFilter>().sharedMesh;
    }

}
