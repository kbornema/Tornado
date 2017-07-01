using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    public GameObject cube;
    public GameObject sphere;
//public GameObject camera;

    SphereCollider coll;
    float radius;

    SphereCoord cubePos;

	// Use this for initialization
	void Start () {
        coll = sphere.GetComponent<SphereCollider>();

        radius = coll.radius * sphere.transform.localScale.x;

        cubePos = new SphereCoord(radius, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 vec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        vec.Normalize();

        vec  = Vector3.RotateTowards(vec, vec, 10, 10);
       

        Vector2 rotation = new Vector2(vec.y, vec.x);

        rotation *= rotation.magnitude;

        rotation *= 0.1f;



        cubePos = cubePos + rotation;

        cube.transform.position = cubePos.Vector3;
        cube.transform.up = Vector3.zero + cubePos.Vector3;

        //print("vec: " + rotation);
        //print("spherecoord" + cubePos.zenith + ", " + cubePos.azimuth);
	}
}
