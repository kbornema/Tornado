using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 orientation;

    float offset;

	// Use this for initialization
	void Start () {

        offset = -5f;
	}
	
	// Update is called once per frame
	void Update () {
		orientation = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 vec = new Vector3(orientation.x, 0, orientation.y);


        Vector3 move = vec.z * transform.up + vec.x * transform.right;
        this.gameObject.transform.position += move;

        Debug.Log("move: " + move);



        Vector3 target = -transform.position;
        target.Normalize();


        //gameObject.transform.position = target * offset;

        this.gameObject.transform.forward = target;

        transform.localRotation = Quaternion.LookRotation((-target * offset).normalized, transform.forward);

        transform.position = (-transform.position.normalized * offset);

    }
}
