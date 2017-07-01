using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement {

    public Vector2 Center, Size;

    GameObject BuildingBP;
    GameObject Root;

    public List<Parcel> parcels;
    public List<GameObject> Buildings;

    public Settlement(Vector2 center, Vector2 size, GameObject buildingBP, GameObject root)
    {
        Center = center;
        Size = size;

        BuildingBP = buildingBP;
        Root = root;

        Parcel parcel = new Parcel(Center, Size);

        parcels = new List<Parcel>() { parcel };

        GenerateParcels(Random.Range(4, 10));
        GenerateBuildings();
    }

    void GenerateParcels(int targetParcels)
    {
        while(parcels.Count < targetParcels)
        {
            int index = Random.Range(0, parcels.Count);
            Parcel p = parcels[index].Split(Random.Range(0, 2), Random.Range(0.25f, 0.75f));
            parcels.Add(p);
        }
    }

    void GenerateBuildings()
    {
        foreach(Parcel p in parcels)
        {
            GameObject obj = GameObject.Instantiate(BuildingBP);

            Vector3 startPoint= new Vector3(p.Center.x, 1000, p.Center.y);
            Vector3 Scale = Constants.Instance.MeshObject.transform.localScale;
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * p.Size.x * 0.5f, obj.transform.localScale.y, obj.transform.localScale.z * p.Size.y * 0.5f);

            obj.transform.SetParent(Root.transform, true);
            RaycastHit hit;
            //Constants.Instance.MeshObject.GetComponent<MeshCollider>().Raycast(ray, out hit, 1000);
            if (Physics.Raycast(startPoint, -Vector3.up, out hit, 1000.0f))
            {

                obj.transform.position = hit.point;
                Debug.Log(hit.transform.name);
            }
        }
    }
}
