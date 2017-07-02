using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour 
{
    private static GameCamera _instance;
    public static GameCamera Instance { get { return _instance; } }

    [SerializeField]
    private Tornado _target;

    public Tornado Target { get { return _target; } }

    [SerializeField]
    private Camera _camera;
    public Camera TheCam { get { return _camera; } }


    private Vector3 curPosVelocity;
    private Vector3 _localOffset;
    private Vector3 _targetPos;

    private bool _isFocusing;
    private float _curFovTime;
    private float _rumblePower;
    private float _rumbleTimeMax;

    private bool _isRumbling;
    private float _curRumbleTime;

    private void Awake()
    {
        _instance = this;

        _localOffset = transform.localPosition;
    }

    private void Start()
    {
        transform.SetParent(null);
    }
    
    private void LateUpdate()
    {

        Vector3 curVelocity = _target.GetVelocity();

        if(curVelocity.magnitude > 1.0f)
        {
            Vector3 projectedDirection = Util.GetVectorPlaneProjection(curVelocity.normalized, Vector3.up);

            float mag = projectedDirection.magnitude;

            if (mag > 0.0f)
            {

                //print(projectedDirection + " -> " + mag);

                Vector3 globalOffset = Quaternion.LookRotation(projectedDirection, Vector3.up) * (_localOffset * _target.transform.localScale.y * _target.transform.localScale.y);

                _targetPos = GetTargetPos() + globalOffset;


                

                //Debug.DrawLine(_target.transform.position, _target.transform.position + projectedDirection, Color.black, Time.deltaTime, false);
            }
        }





        transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref curPosVelocity, 1.0f);

        LookAtTarget();
    }

    private Vector3 GetTargetPos()
    {
        Vector3 target = _target.transform.position + new Vector3(0.0f, _target.GetCenterHeight(true), 0.0f);
        return target;
    }

    private void LookAtTarget()
    {

        transform.LookAt(GetTargetPos(), Vector3.up);
    }

    public void Rumble(float power, float time)
    {
        _rumbleTimeMax = Mathf.Max(_rumbleTimeMax, time);
        _rumblePower = Mathf.Max(power, _rumblePower);
        _curRumbleTime = 0.0f;

        if (!_isRumbling)
        {
            StartCoroutine(RumbleRoutine());
        }
    }

    private IEnumerator RumbleRoutine()
    {
        _isRumbling = true;

        while (_curRumbleTime <= _rumbleTimeMax)
        {
            gameObject.transform.position += Random.insideUnitSphere * _rumblePower * Random.value;

            _curRumbleTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _isRumbling = false;
    }

    public void Focus()
    {
        const float FOV_TIME = 0.25f;
        const float FOV_TARGET = 40.0f;

        _curFovTime = 0.0f;

        if (!_isFocusing)
        {
            StartCoroutine(FocusRoutine(FOV_TARGET, FOV_TIME));
        }
    }

    private IEnumerator FocusRoutine(float targetFov, float time)
    {
        _isFocusing = true;

        float startFov = _camera.fieldOfView;

        while (_curFovTime < time)
        {
            float t = _curFovTime / time;

            float sin = Mathf.Sin(t * Mathf.PI);

            _camera.fieldOfView = Mathf.Lerp(startFov, targetFov, sin);

            _curFovTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _camera.fieldOfView = startFov;

        _isFocusing = false;
    }

    public void GetMovement(out Vector3 forward, out Vector3 right)
    {
        Vector3 euler = transform.rotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(0.0f, euler.y, 0.0f);

        forward = rotation * Vector3.forward;
        right = rotation * Vector3.right;
    }
}
