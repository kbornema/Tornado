using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Areas
{
    Town,
    Village,
    Woods,
}
[System.Serializable]
public class ObjectWithPercentage
{
    public GameObject BP;
    [Range(0f, 1f)]
    public float Percentage;
}

[System.Serializable]
public class AreaSetting
{
    public Areas area;
    [Range(0, 100)]
    public int targetNumber;
    public ObjectWithPercentage[] objects;
    public Vector2 Size;

    public GameObject GetRandomObject()
    {
        float randomValue = Random.Range(0f, 1f);
        for (int i = 0; i < objects.Length - 1; i++)
        {
            if (randomValue < objects[i].Percentage)
                return objects[i].BP;
        }
        return objects[objects.Length - 1].BP;
    }
}

public class ObjectGenerator : MonoBehaviour {

    public Vector3 Scale;

    public GameObject ObjectRoot;
    public GameObject Mesh;
    public GameObject MapGenerator;

    public float maxHeight;
    public float minHeight;

    [Range(0, 100)]
    public int SettlementNumeber;
    public AreaSetting[] settings;

    Mesh mesh;
    MapGenerator mapGen;

	// Use this for initialization
	void Start () {



       // GenerateObjects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateObjects()
    {
        if (mesh == null)
            mesh = Mesh.GetComponent<MeshFilter>().sharedMesh;
        if (mapGen == null)
            mapGen = MapGenerator.GetComponent<MapGenerator>();

        PlaceRandomSpheres();

        
    }

    public void PlaceRandomSpheres()
    {
        if (Application.isPlaying)
        {
            while(ObjectRoot.transform.childCount> 0)
                Destroy(ObjectRoot.transform.GetChild(0).gameObject);
        }
        else
        {
            while(ObjectRoot.transform.childCount > 0)
            {
                DestroyImmediate(ObjectRoot.transform.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < SettlementNumeber; i++)
        {
            Vector2 vertexPos2D;
            Vector3 vertexPos;
            do
            {
                int randomVertex = Random.Range(0, mesh.vertexCount - 1);

                vertexPos = mesh.vertices[randomVertex];

                vertexPos2D = new Vector2(vertexPos.x, vertexPos.z);
            }
            while (vertexPos2D.magnitude > mapGen.SaveZoneRadius || vertexPos.y * -Mesh.transform.localScale.y > maxHeight || vertexPos.y * -Mesh.transform.localScale.y < minHeight);

            vertexPos = Mesh.transform.TransformPoint(vertexPos);

            Settlement settlement = new Settlement(new Vector2(vertexPos.x, vertexPos.z), settings[Random.Range(0, settings.Length)], ObjectRoot, Scale);
        }
    }
}
