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

    [SerializeField]
    private float _doubleClickInterval = 0.5f;

    private float _lastClickTime;

    private Vector3 _lastPos;
    private Vector3 _curPos;

    public void Update(Tornado tornado, float deltaTime)
    {
        if(Input.GetMouseButtonDown(0))
        {
            _lastPos = Input.mousePosition;
            _curPos = _lastPos;

            float curClick = Time.time;

            if (Mathf.Abs(curClick - _lastClickTime) < _doubleClickInterval)
            {
                OnDoubleTap(tornado);
            }

            _lastClickTime = curClick;
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

            tornado.Rotate(0.0f, deltaPos.y * Time.deltaTime, 0.0f);

            tornado.AddForce(new Vector3(deltaPos.x, 0.0f, deltaPos.y) * deltaTime * _speed, ForceMode.Impulse);
        }
    }

    private void OnDoubleTap(Tornado tornado)
    {
        tornado.ReleaseAllAttractedObjects();
    }
}
