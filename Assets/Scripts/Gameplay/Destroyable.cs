using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour 
{
    private const float DMG_RUMBLE_SCALE = 0.25f;

    [SerializeField]
    private GameObject _root;

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


    private void Start()
    {
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

        _rumblePower = dmg * DMG_RUMBLE_SCALE;
        _rumbleTime = 0.1f;

        float nextDestroyPercent = _deltaDestroyPercent * (_shakingObjects.Count - 1);

        if (HealthPercent < nextDestroyPercent && _shakingObjects.Count > 0)
        {
            OnNextSubObjectDestroyed(attacker);
        }

        if(_curHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnNextSubObjectDestroyed(Tornado tornado)
    {
        int id = _shakingObjects.Count - 1;
        //Destroy(_shakingObjects[id]);

        AttractingObject curDestroyObj = _shakingObjects[id];

        curDestroyObj.transform.SetParent(null);
        _shakingObjects.RemoveAt(id);

        tornado.AddAttractedObject(curDestroyObj);
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
