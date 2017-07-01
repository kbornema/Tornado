using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour {

    public GameObject SphereBP;
    public GameObject Mesh;
    Mesh mesh;



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
        if (mesh == null)
        {
            mesh = Mesh.GetComponent<MeshFilter>().mesh;
        }

        PlaceRandomSpheres();
    }

    public void PlaceRandomSpheres()
    {
        if (objects != null && objects.Count > 0)
        {
            for (int i = objects.Count-1; i >= 0; i--)
            {
                DestroyImmediate(objects[i]);
                Debug.Log("DESTROYED");
            }
        }

        objects.Clear();

        for(int i = 0; i < 10; i++)
        {
            int randomVertex = Random.Range(0, mesh.vertexCount-1);

            Vector3 vertexPos = mesh.vertices[randomVertex];

            vertexPos = new Vector3(vertexPos.x * Mesh.transform.localScale.x, vertexPos.y * Mesh.transform.localScale.y + 100, vertexPos.z * Mesh.transform.localScale.z);

            GameObject obj = Instantiate(SphereBP);
            obj.transform.position = vertexPos;

            objects.Add(obj);

            Debug.Log("Count: " + objects.Count);
        }
    }
}
