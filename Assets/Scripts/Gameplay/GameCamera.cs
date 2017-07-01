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

    private bool _isFocusing;

    private float _curFovTime;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        transform.SetParent(null);
    }
    
    private void LateUpdate()
    {
        Vector3 target = _target.transform.position;
        transform.LookAt(target);
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
