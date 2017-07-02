using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSegment : MonoBehaviour 
{
    [SerializeField]
    private GameObject _meshObject;

    public GameObject MeshObj { get { return _meshObject; } }
}
