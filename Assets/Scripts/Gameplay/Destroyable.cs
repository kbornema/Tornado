using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour 
{
    private const float DMG_RUMBLE_SCALE = 0.25f;

    [SerializeField]
    private float _pointsOnDestroy = 100.0f;

    [SerializeField]
    private float _energyOnDestroy = 5.0f;

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

    private bool _dead = false;

    [SerializeField]
    private Rigidbody _myBody;

    private void Reset()
    {
        _myBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        enabled = false;

        _myBody.isKinematic = true;
        _myBody.useGravity = false;

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
        if (dmg == float.NaN || float.IsInfinity(dmg) || dmg == 0.0f)
            return;

        _curHealth -= dmg;

        Statistics.NotifyDamage("", dmg);
        Statistics.NotifyCollectPoints("DP", dmg);

        _rumblePower = dmg * DMG_RUMBLE_SCALE;
        _rumbleTime = 0.1f;

        if(!enabled)
            enabled = true;

        float nextDestroyPercent = _deltaDestroyPercent * (_shakingObjects.Count - 1);

        if (HealthPercent < nextDestroyPercent && _shakingObjects.Count > 0)
        {
            OnNextSubObjectDestroyed(attacker);
        }

        if (!_dead && _curHealth <= 0.0f)
        {
            _dead = true;
            Statistics.NotifyCollectPoints("AP", _pointsOnDestroy);

            switch (transform.tag)
            {
                case "Tree":
                    Statistics.NotifyDestroyTree("", 1);
                    break;
                case "House":
                    Statistics.NotifyDestroyHouse("", 1);
                    break;

            }

            if(attacker && attacker.Energy)
            {
                attacker.Energy.AddEnergy(_energyOnDestroy);
            }

            Destroy(gameObject);

            GameCamera.Instance.Rumble(1.0f, 0.2f);
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
            if(!tornado.AddAttractedObject(curDestroyObj))
            {
                curDestroyObj.SetState(AttractingObject.State.Free);
                curDestroyObj.AddRandomTorque();
            }
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

            if (float.IsNaN(_rumblePower) || float.IsInfinity(_rumblePower))
            {
                return;
            }

            for (int i = 0; i < _shakingObjects.Count; i++)
            {
                Vector3 randOffset = new Vector3();

                if (_rumbleTime > 0.0f)
                    randOffset = Random.onUnitSphere * _rumblePower;
            
                _shakingObjects[i].transform.position = _shakingObjectsStartPos[i] + randOffset;
            }

            if (_rumbleTime < 0.0f)
                enabled = false;
        }
    }
}
