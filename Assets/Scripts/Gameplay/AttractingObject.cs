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
    private float _dpsVelocity = 10.0f;

    [SerializeField]
    private Collider _myCollider;

    public Collider MyCollider { get { return _myCollider; } }

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
        _startScale = gameObject.transform.lossyScale;
        SetState(State.OnObstacle);
    }

    public void SetState(State state)
    {   
        if(state == State.Attracted)
        {
            gameObject.layer = LayerMask.NameToLayer("AttractedObstacle");
            _rigidBody.useGravity = true;

            AddRandomTorque(Random.value * 360.0f, ForceMode.Impulse);
            transform.localScale = _startScale * 0.5f;
        }

        else if(state == State.Free)
        {
            gameObject.layer = LayerMask.NameToLayer("FreeObstacle");
            _rigidBody.useGravity = true;
            transform.localScale = _startScale * 0.5f;
        }

        else if(state == State.OnObstacle)
        {
            gameObject.layer = LayerMask.NameToLayer("None");
            _rigidBody.useGravity = false;
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

    public Vector3 GetVelocity()
    {
        return _rigidBody.velocity;
    }

    public void SetVelocity(Vector3 v)
    {
        _rigidBody.velocity = v;
    }

    public void ResetVelocity()
    {
        SetVelocity(Vector3.zero);
    }

    public void AddRandomTorque(float force, ForceMode mode)
    {
        _rigidBody.AddTorque(Random.onUnitSphere * force, mode);
    }

    public void AddRandomTorque()
    {
        AddRandomTorque(Random.value * 360.0f, ForceMode.Impulse);
    }

    private void OnCollisionStay(Collision coll)
    {
        Destroyable d = coll.collider.GetComponent<Destroyable>();

        if(d)
        {
            float scale = 1.0f;

            if (_currentState == State.Attracted)
                scale = 0.5f;

            float damage = _rigidBody.velocity.magnitude * _dpsVelocity * Time.fixedDeltaTime * scale;
            d.ReceiveDamage(damage, null);
        }
    }


}
