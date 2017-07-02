using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement {

    public Vector2 Center, Size;

    AreaSetting setting;
    GameObject Root;

    Vector3 scale;
    float rotation;

    public List<Parcel> parcels;
    public List<GameObject> Buildings;
    
    public Settlement(Vector2 center, AreaSetting setting, GameObject root, Vector3 _scale)
    {
        Center = center;
        Size = setting.Size;
        scale = _scale;

        this.setting = setting;
        Root = root;

        Parcel parcel = new Parcel(Center, Size);

        parcels = new List<Parcel>() { parcel };

        rotation = Random.Range(0f, 90f);

        GenerateParcels(Random.Range((int)(setting.targetNumber * 0.7f), setting.targetNumber));
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

/*    void GenerateBuildings()
    {
        foreach(Parcel p in parcels)
        {
            GameObject obj = GameObject.Instantiate(BuildingBP);

            Vector3 startPoint = new Vector3(p.Center.x, 200, p.Center.y);
            Vector3 settlementCenter = new Vector3(Center.x, 200, Center.y);
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * p.Size.x * scale.x, obj.transform.localScale.y * scale.y, obj.transform.localScale.z * p.Size.y * scale.z);

            Vector3 finalScale = new Vector3(obj.transform.localScale.x * p.Size.x * scale.x, obj.transform.localScale.y * scale.y, obj.transform.localScale.z * p.Size.y * scale.z);

            Vector3 topLeft = new Vector3(p.TopLeft.x, 200, p.TopLeft.y);
            topLeft = new Vector3(topLeft.x * finalScale.x, topLeft.y * finalScale.y, finalScale.z * finalScale.z);
            Vector3 Size = new Vector3(p.Size.x* finalScale.x, 0, p.Size.y * finalScale.z);


            obj.transform.SetParent(Root.transform, true);
            RaycastHit hit;
            int layerMask = 1<<LayerMask.NameToLayer("Ground");
            //Constants.Instance.MeshObject.GetComponent<MeshCollider>().Raycast(ray, out hit, 1000);

            List<Vector3> corners = new List<Vector3>() {
                new Vector3(topLeft.x, 200, topLeft.y),
                new Vector3(topLeft.x + Size.x, 200, topLeft.y),
                new Vector3(topLeft.x, 200, topLeft.y + Size.z),
                new Vector3(topLeft.x + Size.x, 200, topLeft.y+ Size.z)
            };

            for(int i = 0; i < 4; i++)
            {
                Debug.DrawRay(corners[i], -Vector3.up * 1000, Color.red, 10f);
                corners[i] = RotateInOrigin(corners[i], settlementCenter, rotation);
            }
            Vector3 hitPoint = new Vector3(0, 205, 0);
            for(int i = 0; i < 4; i++)
            {
                Debug.DrawRay(corners[i], -Vector3.up*1000, Color.black, 10f);
                if(Physics.Raycast(corners[i], -Vector3.up, out hit, 1000f, layerMask))
                {
                    if (hit.point.y < hitPoint.y)
                        hitPoint = hit.point;
                }
                else
                {
                    Debug.Log("No Intersection with Mesh");
                }
            }

            startPoint = RotateInOrigin(startPoint, settlementCenter, rotation);

            if (Physics.Raycast(startPoint, -Vector3.up, out hit, 1000.0f, layerMask))
            {
                obj.transform.position = hit.point;
            }
            obj.transform.position = new Vector3(startPoint.x, hitPoint.y, startPoint.z);
            obj.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }*/

    void GenerateBuildings()
    {
        Vector3 SettlementCenter = new Vector3(Center.x, 100f, Center.y);
        //Debug.DrawRay(SettlementCenter, -Vector3.up * 100f, Color.green, 10f);
        foreach(Parcel p in parcels)
        {
            Vector3 startPoint = new Vector3(p.Center.x, 100, p.Center.y);

            //Debug.DrawRay(startPoint, -Vector3.up * 100, Color.red, 10f);

            List<Vector3> corners = new List<Vector3>() {
                new Vector3(p.TopLeft.x, 100, p.TopLeft.y),
                new Vector3(p.TopLeft.x, 100, p.TopLeft.y + p.Size.y),
                new Vector3(p.TopLeft.x + p.Size.x, 100, p.TopLeft.y + p.Size.y),
                new Vector3(p.TopLeft.x + p.Size.x, 100, p.TopLeft.y)
            };
            //for (int i = 0; i < 4; i++)
            //    Debug.DrawLine(corners[i], corners[(i + 1) % 4]);

            for(int i = 0; i < corners.Count; i++)
            {
                corners[i] = RotateInOrigin(corners[i], SettlementCenter, rotation);
            }
            startPoint = RotateInOrigin(startPoint, SettlementCenter, rotation);
            //Debug.DrawRay(startPoint, -Vector3.up * 100f, Color.green, rotation);

            //for (int i = 0; i < 4; i++)
            //    Debug.DrawLine(corners[i], corners[(i + 1) % 4], Color.yellow);

            Vector3 lowestPoint = new Vector3(0, 200, 0);
            RaycastHit hit;

            for(int i = 0; i < 4; i++)
            {
                if(Physics.Raycast(corners[i], -Vector3.up, out hit, 200f))
                {
                    if (hit.point.y < lowestPoint.y)
                        lowestPoint = hit.point; 
                }
            }

            GameObject obj = GameObject.Instantiate(setting.GetRandomObject());

            obj.transform.position = new Vector3(startPoint.x, lowestPoint.y, startPoint.z);

            obj.transform.SetParent(Root.transform, true);
            obj.transform.Rotate(0, rotation, 0);

            Vector3 scale = obj.transform.localScale;
            scale.Scale(new Vector3(0.3f + p.Size.x / Size.x, 1f, 0.3f + p.Size.y / Size.y));

            obj.transform.localScale = scale;

        }
    }

    Vector3 RotateInOrigin(Vector3 vec, Vector3 pointOrigin, float angle)
    {
        Vector4 vec4 = pointOrigin;
        vec4.w = 1;
        Vector4 result = vec;
        result.w = 1;
        Matrix4x4 matT1 = Matrix4x4.Translate(vec4);
        Matrix4x4 matT2 = Matrix4x4.Translate(-vec4);
        Quaternion quat = Quaternion.Euler(0, rotation, 0);
        result = matT2* result;
        result = quat * result;
        result.w = 1f;
        result = matT1 * result;
        return result;
    }
}
