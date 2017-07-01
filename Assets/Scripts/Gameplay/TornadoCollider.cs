using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class TornadoCollider : MonoBehaviour 
{
    [SerializeField]
    private Tornado _tornado;

	public void OnTriggerStay(Collider collider)
    {
        Destroyable d = collider.GetComponent<Destroyable>();

        if(d)
        {
            _tornado.OnObstacleHit(d);
            return;
        }

        AttractingObject obj = collider.GetComponent<AttractingObject>();

        if (obj && obj.CurrentState != AttractingObject.State.Attracted)
        {
            _tornado.AddAttractedObject(obj);
            return;
        }

    }
}
