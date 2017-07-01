using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour {

    public Vector3 Scale;

    public GameObject HouseBP;
    public GameObject ObjectRoot;
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
        for (int j = ObjectRoot.transform.childCount - 1; j >= 0; j--)
        {
            DestroyImmediate(ObjectRoot.transform.GetChild(j).gameObject);
        }

        for (int i = 0; i < 10; i++)
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

            vertexPos = Mesh.transform.TransformPoint(vertexPos);
            /*
                        GameObject obj = Instantiate(HouseBP);

                        obj.transform.localScale = new Vector3(obj.transform.localScale.x * Scale.x, obj.transform.localScale.y * Scale.y, obj.transform.localScale.z * Scale.z);
                        obj.transform.position = vertexPos;

                        obj.transform.parent = ObjectRoot.transform;

                        objects.Add(obj);
            */

            Settlement settlement = new Settlement(new Vector2(vertexPos.x, vertexPos.z), new Vector2(10, 8), HouseBP, ObjectRoot);
        }
    }
}
