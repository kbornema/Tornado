using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour 
{
    private const float DMG_RUMBLE_SCALE = 0.25f;

    [SerializeField]
    private GameObject _root;

    [SerializeField]
    private GameObject[] _shakingObjects;
    private Vector3[] _shakingObjectsStartPos;

    [SerializeField]
    private float _health = 100.0f;

    private float _rumblePower;
    private float _rumbleTime;


    private void Start()
    {
        _shakingObjectsStartPos = new Vector3[_shakingObjects.Length];

        for (int i = 0; i < _shakingObjectsStartPos.Length; i++)
        {
            _shakingObjectsStartPos[i] = _shakingObjects[i].transform.position;
        }
    }

    public void ReceiveDamage(float dmg)
    {
        _health -= dmg;

        _rumblePower = dmg * DMG_RUMBLE_SCALE;
        _rumbleTime = 0.1f;

        if(_health <= 0.0f)
        {
            Destroy(_root);
        }
    }


    private void FixedUpdate()
    {
        if(_rumbleTime >= 0.0f)
        {
            _rumbleTime -= Time.deltaTime;

            for (int i = 0; i < _shakingObjects.Length; i++)
            {
                Vector3 randOffset = new Vector3();

                if (_rumbleTime > 0.0f)
                    randOffset = Random.onUnitSphere * _rumblePower;
            
                _shakingObjects[i].transform.position = _shakingObjectsStartPos[i] + randOffset;
            }
        }
    }
	
}
