using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractingObject : MonoBehaviour 
{
    //public enum FordeMode { UseMass, NoMass }

    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private float _maxVelocity = 15.0f;

    public void EnableGravity(bool val)
    {
        _rigidBody.useGravity = val;
    }

    public void AddForce(Vector3 dir, ForceMode forceMode)
    {
        _rigidBody.AddForce(dir, forceMode);

        float velocity = _rigidBody.velocity.magnitude;

        if (velocity > _maxVelocity)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * _maxVelocity;
        }
    }
}
