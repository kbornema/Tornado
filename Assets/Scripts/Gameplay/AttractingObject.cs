using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractingObject : MonoBehaviour 
{
    public enum State { OnObstacle, Attracted, Free }

    [SerializeField]
    private Rigidbody _rigidBody;

    [SerializeField]
    private float _maxVelocity = 15.0f;

    [SerializeField]
    private Collider _myCollider;

    [SerializeField]
    private State _currentState = State.OnObstacle;
    public State CurrentState { get { return _currentState; } }

    private Vector3 _startScale;

    private void Reset()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _startScale = gameObject.transform.localScale;
        SetState(State.OnObstacle);
    }

    public void SetState(State state)
    {
        if(state == State.Attracted)
        {
            gameObject.layer = LayerMask.NameToLayer("AttractedObstacle");
            _rigidBody.useGravity = true;
            _myCollider.enabled = true;

            AddRandomTorque(Random.value * 360.0f, ForceMode.Impulse);
            transform.localScale = _startScale * 0.5f;
        }

        else if(state == State.Free)
        {
            gameObject.layer = LayerMask.NameToLayer("FreeObstacle");
            _myCollider.enabled = true;
        }

        else if(state == State.OnObstacle)
        {
            gameObject.layer = LayerMask.NameToLayer("None");
            _rigidBody.useGravity = false;
            _myCollider.enabled = false;
        }

        _currentState = state;
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

    public void AddRandomTorque(float force, ForceMode mode)
    {
        _rigidBody.AddTorque(Random.onUnitSphere * force, mode);
    }
}
