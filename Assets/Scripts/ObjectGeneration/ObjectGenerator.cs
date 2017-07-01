using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour {

    public GameObject SphereBP;
    public GameObject Mesh;
    public GameObject MapGenerator;

    Mesh mesh;
    MapGenerator mapGen;


    List<GameObject> objects;

	// Use this for initialization
	void Start () {

        objects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateObjects()
    {
        if( objects == null)
            objects = new List<GameObject>();
        if (mesh == null)
            mesh = Mesh.GetComponent<MeshFilter>().sharedMesh;
        if (mapGen == null)
            mapGen = MapGenerator.GetComponent<MapGenerator>();

        PlaceRandomSpheres();
    }

    public void PlaceRandomSpheres()
    {
        if (objects != null && objects.Count > 0)
        {
            for (int i = objects.Count-1; i >= 0; i--)
            {
                DestroyImmediate(objects[i]);
            }
        }

        objects.Clear();

        for(int i = 0; i < 10; i++)
        {
            Vector2 vertexPos2D;
            Vector3 vertexPos;
            do
            {
                int randomVertex = Random.Range(0, mesh.vertexCount - 1);

                vertexPos = mesh.vertices[randomVertex];


                vertexPos2D = new Vector2(vertexPos.x, vertexPos.z);
            }
            while (vertexPos2D.magnitude > mapGen.SaveZoneRadius);

            vertexPos = new Vector3(vertexPos.x * Mesh.transform.localScale.x, vertexPos.y * Mesh.transform.localScale.y + 100, vertexPos.z * Mesh.transform.localScale.z);

            GameObject obj = Instantiate(SphereBP);
            obj.transform.position = vertexPos;

            objects.Add(obj);
        }
    }
}
