using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour {


    void OnCollisionEnter(Collision collision)
    {
        Tornado t = collision.collider.gameObject.GetComponent<Tornado>();
        if (t == null)
        {
            GameObject.Destroy(collision.gameObject);
        }
        else
        {
        }

    }
}
