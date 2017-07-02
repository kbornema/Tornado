using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AreaSetting
{
    public AreaSetting(int targetNumber, GameObject[] _objects, float[] _percentages, Vector2 size)
    {
        this.targetNumber = targetNumber;
        arr = new GameObject[_objects.Length];
        percentages = new float[_percentages.Length];
        for (int i = 0; i < _objects.Length; i++)
        {
            arr[i] = _objects[i];
            percentages[i] = _percentages[i];
        }
        Size = size;
    }

    public int targetNumber;
    GameObject[] arr;
    float[] percentages;
    public Vector2 Size;

    public GameObject GetRandomObject()
    {
        float randomValue = Random.Range(0f, 1f);
        for (int i = 0; i < percentages.Length - 1; i++)
        {
            if (randomValue < percentages[i])
                return arr[i];
        }
        return arr[arr.Length - 1];
    }
}

public class ObjectGenerator : MonoBehaviour {

    enum Areas
    {
        Town,
        Village,
        Woods,
    }

    Dictionary<Areas, AreaSetting> dict;

    public Vector3 Scale;

    public GameObject HouseBP;
    public GameObject TreeBP;
    public GameObject ObjectRoot;
    public GameObject Mesh;
    public GameObject MapGenerator;

    Mesh mesh;
    MapGenerator mapGen;

    List<GameObject> objects;

	// Use this for initialization
	void Start () {

        objects = new List<GameObject>();

       // GenerateObjects();
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

        if(dict == null)
        {
            dict = new Dictionary<Areas, AreaSetting>();
            dict.Add(Areas.Village, new AreaSetting(9, new GameObject[] { HouseBP, TreeBP }, new float[] { 0.7f, 0.3f }, new Vector2(12, 8)));
            dict.Add(Areas.Woods, new AreaSetting(25, new GameObject[] { TreeBP }, new float[] { 1f }, new Vector2(30, 40)));
            dict.Add(Areas.Town, new AreaSetting(15, new GameObject[] { HouseBP, TreeBP }, new float[] { 0.85f, 0.15f }, new Vector2(16, 14)));
        }

        PlaceRandomSpheres();

        
    }

    public void PlaceRandomSpheres()
    {
        for (int j = ObjectRoot.transform.childCount - 1; j >= 0; j--)
        {
            for (int i = 0; i < ObjectRoot.transform.GetChild(j).childCount; i++)
                DestroyImmediate(ObjectRoot.transform.GetChild(j).GetChild(i).gameObject);
            DestroyImmediate(ObjectRoot.transform.GetChild(j).gameObject);
        }

        for (int i = 0; i < 20; i++)
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

            Settlement settlement = new Settlement(new Vector2(vertexPos.x, vertexPos.z), dict[(Areas)Random.Range(0, 3)], ObjectRoot, Scale);
        }
    }
}
