using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour 
{
    private const float DMG_RUMBLE_SCALE = 0.25f;

    [SerializeField]
    private float _pointsOnDestroy = 100.0f;

    private List<AttractingObject> _shakingObjects;
    private Vector3[] _shakingObjectsStartPos;

    [SerializeField]
    private float _health = 100.0f;

    [SerializeField]
    private float _curHealth = 0.0f;

    public float Health { get { return _curHealth; } }

    public float HealthPercent { get { return _curHealth / _health; } }

    private float _rumblePower;
    private float _rumbleTime;

    private float _deltaDestroyPercent;

    [SerializeField]
    private Rigidbody _myBody;

    private void Reset()
    {
        _myBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _myBody.isKinematic = true;

        _shakingObjects = new List<AttractingObject>(GetComponentsInChildren<AttractingObject>());

        _curHealth = _health;

        _deltaDestroyPercent = (1.0f / _shakingObjects.Count);

        _shakingObjectsStartPos = new Vector3[_shakingObjects.Count];

        for (int i = 0; i < _shakingObjectsStartPos.Length; i++)
        {
            _shakingObjectsStartPos[i] = _shakingObjects[i].transform.position;
        }
    }

    public void ReceiveDamage(float dmg, Tornado attacker)
    {
        _curHealth -= dmg;

        //TODO: received damage: through event:

        _rumblePower = dmg * DMG_RUMBLE_SCALE;
        _rumbleTime = 0.1f;

        float nextDestroyPercent = _deltaDestroyPercent * (_shakingObjects.Count - 1);

        if (HealthPercent < nextDestroyPercent && _shakingObjects.Count > 0)
        {
            OnNextSubObjectDestroyed(attacker);
        }

        if(_curHealth <= 0.0f)
        {
            //TODO: receive points:
            Destroy(gameObject);
        }
    }

    private void OnNextSubObjectDestroyed(Tornado tornado)
    {
        int id = _shakingObjects.Count - 1;

        AttractingObject curDestroyObj = _shakingObjects[id];

        curDestroyObj.transform.SetParent(null);
        _shakingObjects.RemoveAt(id);

        if(tornado)
        {
            tornado.AddAttractedObject(curDestroyObj);
        }

        else
        {
            curDestroyObj.SetState(AttractingObject.State.Free);
            curDestroyObj.AddRandomTorque();
        }
        
    }


    private void FixedUpdate()
    {
        if(_rumbleTime >= 0.0f)
        {
            _rumbleTime -= Time.deltaTime;

            for (int i = 0; i < _shakingObjects.Count; i++)
            {
                Vector3 randOffset = new Vector3();

                if (_rumbleTime > 0.0f)
                    randOffset = Random.onUnitSphere * _rumblePower;
            
                _shakingObjects[i].transform.position = _shakingObjectsStartPos[i] + randOffset;
            }
        }
    }
	
}
