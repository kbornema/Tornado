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

    private float _startMaxVelocity;

    [SerializeField]
    private float _dpsVelocity = 10.0f;

    [SerializeField]
    private Collider _myCollider;

    public Collider MyCollider { get { return _myCollider; } }

    [SerializeField]
    private State _currentState = State.OnObstacle;
    public State CurrentState { get { return _currentState; } }

    [SerializeField]
    private float _destroyScale = 0.5f;

    [SerializeField]
    private float _maxDealingDmg = 100.0f; 


    private float _totalCausedDmg;

    private Vector3 _startScale;

    private void Reset()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
    }

    private void Awake()
    {
        _startMaxVelocity = _maxVelocity;
        _startScale = gameObject.transform.lossyScale;
        SetState(State.OnObstacle);
        enabled = false;
    }

    public void SetTornadoScale(float scale)
    {
        _maxVelocity = _startMaxVelocity * scale;
    }

    public void SetState(State state)
    {   
        if(state == State.Attracted)
        {
            gameObject.layer = LayerMask.NameToLayer("AttractedObstacle");
            _rigidBody.useGravity = true;

            AddRandomTorque(Random.value * 360.0f, ForceMode.Impulse);
            transform.localScale = _startScale * _destroyScale;

            if (!enabled)
                enabled = true;
        }

        else if(state == State.Free)
        {
            if (gameObject == null)
                return;

            gameObject.layer = LayerMask.NameToLayer("FreeObstacle");
            _rigidBody.useGravity = true;
            transform.localScale = _startScale * _destroyScale;

            if (!enabled)
                enabled = true;
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
        float sqrMag = dir.sqrMagnitude;
        if (sqrMag == float.NaN || sqrMag == float.PositiveInfinity || sqrMag == float.NegativeInfinity)
        {
            
            Debug.LogWarning("Vector3 is NAN!!");
            return;
        }

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
            _totalCausedDmg += damage;

            d.ReceiveDamage(damage, null);

            if(_totalCausedDmg >= _maxDealingDmg)
            {
                Destroy(gameObject);
            }
        }
    }


}
