using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpawner : MonoBehaviour {

    public GameObject Mesh;
    public GameObject Tornado;

    Mesh mesh;

	// Use this for initialization
	void Start () {
        mesh = Mesh.GetComponent<MeshCollider>().sharedMesh;
        

       // int layerMask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(0, 100, 0), -Vector3.up, 10000f);
        foreach(RaycastHit r in hits)
        {
            if (r.collider.gameObject.tag == "Mesh")
                Tornado.transform.position = r.point + Vector3.up * 3f;
        }
	}
}
