using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MobileInput : IInputDevice
{
    [SerializeField]
    private float _maxDeltaPos;

    [SerializeField]
    private float _speed = 10.0f;

    //private bool _isSwiping;

    private Vector3 _lastPos;
    private Vector3 _curPos;

    public void Update(Tornado tornado, float deltaTime)
    {
        if(Input.GetMouseButtonDown(0))
        {
            _lastPos = Input.mousePosition;
            _curPos = _lastPos;
        }

        else if(Input.GetMouseButton(0))
        {
            _lastPos = _curPos;
            _curPos = Input.mousePosition;

            Vector3 deltaPos = -(_lastPos - _curPos);

            float deltaMag = deltaPos.magnitude;
            deltaMag = deltaMag / _maxDeltaPos;

            deltaMag = Mathf.Clamp(deltaMag, 0.0f, 1.0f);

            deltaPos = deltaPos.normalized * deltaMag;

            tornado.AddForce(new Vector3(deltaPos.x, 0.0f, deltaPos.y) * deltaTime * _speed);
        }



        /*
        Debug.Log(Input.touchCount);

        if (Input.touchCount == 0)
        {
            
            return;
        }

        Vector2 deltaPos = Input.GetTouch(0).deltaPosition;

        float deltaMagnitude = deltaPos.magnitude;

        float scale = deltaMagnitude / _maxDeltaPos;

        deltaPos = deltaPos.normalized * scale;
        
        tornado.AddForce(new Vector3(deltaPos.x, 0.0f, deltaPos.y));
        */
        /*
        if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
        {
            if (_isSwiping == false)
            {
                _isSwiping = true;
                _lastPos = Input.GetTouch(0).position;
                return;
            }
        }
        */
    }
}
