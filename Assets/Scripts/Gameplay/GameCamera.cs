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

    private Vector3 _offset;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _offset = gameObject.transform.position;
    }
    
    private void LateUpdate()
    {
        Vector3 target = _target.transform.position;
        transform.LookAt(target);


    }

    public void GetMovement(out Vector3 forward, out Vector3 right)
    {
        Vector3 euler = transform.rotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(0.0f, euler.y, 0.0f);

        forward = rotation * Vector3.forward;
        right = rotation * Vector3.right;
    }
}
