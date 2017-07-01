using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MobileInput : AInputDevice
{
    [SerializeField]
    private float _maxDeltaPos = 4.0f;

    [SerializeField]
    private float _doubleClickInterval = 0.5f;

    private float _lastClickTime;

    private Vector3 _lastPos;
    private Vector3 _curPos;

    private Vector2 _inputVector;

    private bool _isMoving;

    protected override void Update(float deltaTime)
    {
        _isMoving = false;

        if(Input.GetMouseButtonDown(0))
        {
            _lastPos = Input.mousePosition;
            _curPos = _lastPos;

            float curClick = Time.time;

            if (Mathf.Abs(curClick - _lastClickTime) < _doubleClickInterval)
            {
                OnDoubleTap();
            }

            _lastClickTime = curClick;
        }

        else if(Input.GetMouseButton(0))
        {
            _isMoving = true;

            _lastPos = _curPos;
            _curPos = Input.mousePosition;

            Vector3 deltaPos = -(_lastPos - _curPos);

            float deltaMag = deltaPos.magnitude;
            deltaMag = deltaMag / _maxDeltaPos;

            deltaMag = Mathf.Clamp(deltaMag, 0.0f, 1.0f);

            deltaPos = deltaPos.normalized * deltaMag;

            _inputVector = new Vector2(deltaPos.x, deltaPos.y);
        }
    }

    private void OnDoubleTap()
    {
        ReleaseAttractedObjects();
    }

    protected override Vector2 GetInput()
    {
        return _inputVector;
    }

    protected override bool IsMoving()
    {
        return _isMoving;
    }
}
