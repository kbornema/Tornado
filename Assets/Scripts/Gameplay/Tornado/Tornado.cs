using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tornado : MonoBehaviour 
{
    [SerializeField]
    private GameObject[] _segments;
    [SerializeField]
    private GameObject _segmentRoot;
    [SerializeField]
    private int _numSegments;

    [SerializeField]
    private GameObject _segmentPrefab;

    [SerializeField]
    private float _colliderScale = 1.0f;

    [SerializeField]
    private CapsuleCollider _collider;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _curConfPercent = 0.0f;

    [SerializeField]
    private Rigidbody _tornadoRigidbody;

    [SerializeField]
    private TornadoConf _configurationMin;
    [SerializeField]
    private TornadoConf _configurationMax;

    private TornadoConf _configurationCurrent;

    [SerializeField]
    private AttractingParams _attractingParams;

    [SerializeField]
    private TornadoEnergy _energy;
    public TornadoEnergy Energy { get { return _energy; } }

    private List<AttractingObject> _attractedGameObjects = new List<AttractingObject>();

    public void SetConfPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);

        _curConfPercent = percent;
        _configurationCurrent = TornadoConf.Interpolate(_configurationMin, _configurationMax, percent);

        _collider.center = new Vector3(0.0f, GetCenterHeight(), 0.0f);
        _collider.height = GetTotalHeight();

        _collider.radius = GetCurrentRadius(); 
    }

    public void ChangeConfPercent(float deltaPercent)
    {
        SetConfPercent(_curConfPercent + deltaPercent);
    }

    private float _lifeTime;

    private void CreateSegments()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            Destroy(_segments[i]);
        }

        _segments = new GameObject[_numSegments];

        for (int i = 0; i < _numSegments; i++)
        {
            _segments[i] = Instantiate(_segmentPrefab);
            _segments[i].transform.SetParent(_segmentRoot.transform);
        }
    }

    public void AddForce(Vector3 force, ForceMode mode)
    {
        _tornadoRigidbody.AddForce(force, mode);
    }

    public void AddForce(Vector3 dir, float scale, ForceMode mode)
    {
        AddForce(dir * scale, mode);
    }
	
    void Start()
    {
        CreateSegments();
        SetConfPercent(0.0f);
    }

	private void Update() 
    {
        if (_numSegments != _segments.Length)
            CreateSegments();

         _lifeTime += Time.deltaTime;

        SetConfPercent(_curConfPercent);

        UpdateTornadoSegments(_configurationCurrent);

        UpdateAttractedObjects(Time.deltaTime);

        if (_energy)
            _energy.UpdateDrain(_configurationCurrent._energyDrainPerSecond);
	}

    private void UpdateAttractedObjects(float deltaTime)
    {
        Vector3 center = gameObject.transform.position + new Vector3(0.0f, GetCenterHeight(), 0.0f);

        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            Vector3 customCenter = center + Random.onUnitSphere * Random.Range(_attractingParams.randomOffsetMin, _attractingParams.randomOffsetMax);
            AttractingObject obj = _attractedGameObjects[i];

            float distSq = (obj.transform.position - customCenter).sqrMagnitude;

            float inverseDistSq = 1.0f / distSq;

            Vector3 toCenter = customCenter - obj.transform.position;

            float speed = Mathf.Clamp(inverseDistSq * _attractingParams.distScale, _attractingParams.minSpeed, _attractingParams.maxSpeed);

            Vector3 dir = toCenter.normalized * speed;

            obj.AddForce(dir, ForceMode.Impulse);
        }
    }

    public void Rotate(float x, float y, float z, ForceMode mode)
    {
        //transform.Rotate(x, y, z);
        //_tornadoRigidbody.AddTorque(x, y, z, mode);

        
    }

    private void UpdateTornadoSegments(TornadoConf conf)
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            float curHeight = GetHeight(i);

            _segments[i].transform.localPosition = new Vector3(0.0f, curHeight, 0.0f);
            
            float percent = (float)i / (float)_numSegments;

            float curWidth = Mathf.Lerp(conf._bottomWidth, conf._topWidth, percent);

            float curRotation = Mathf.Lerp(conf._rotationSpeedBottom, conf._rotationSpeedTop, percent) * _lifeTime;

            _segments[i].transform.localRotation = Quaternion.Euler(0.0f, curRotation, 0.0f);

            _segments[i].transform.localScale = new Vector3(curWidth, conf._cubeHeight, curWidth);
        }
    }

    public float GetHeight(int segment)
    {
        return (segment + 1) * _configurationCurrent._spacing * _configurationCurrent._cubeHeight * 0.5f;
    }

    public float GetTotalHeight()
    {
        return GetHeight(_numSegments);
    }

    public float GetCenterHeight()
    {
        return GetTotalHeight() * 0.5f;
    }

    private float GetCurrentRadius()
    {
        return _configurationCurrent._topWidth * 0.5f * _colliderScale;
    }
    
    public void OnObstacleHit(Destroyable d)
    {
        d.ReceiveDamage(_configurationCurrent._damagePerSecond * Time.deltaTime, this);
    }

    public void AddAttractedObject(AttractingObject obj)
    {
        Debug.Assert(obj.CurrentState != AttractingObject.State.Attracted);

        obj.SetState(AttractingObject.State.Attracted);
        _attractedGameObjects.Add(obj);
    }

    public void ReleaseAllObectsForward()
    {
        Vector3 forward;
        Vector3 right;

        GameCamera.Instance.GetMovement(out forward, out right);


        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            _attractedGameObjects[i].SetState(AttractingObject.State.Free);


            Vector3 velocity = _attractedGameObjects[i].GetVelocity();

            float velocityForce = velocity.magnitude;

            velocity = forward * velocityForce;


            _attractedGameObjects[i].SetVelocity(velocity);
        }

        _attractedGameObjects.Clear();
    }

    public void ReleaseAllObjectsVelocity()
    {
        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            _attractedGameObjects[i].SetState(AttractingObject.State.Free);


        }

        _attractedGameObjects.Clear();
    }

    [System.Serializable]
    public class TornadoConf
    {
        public float _bottomWidth = 0.2f;
        public float _topWidth = 2.0f;
        public float _spacing = 0.1f;
        public float _cubeHeight = 0.2f;
        public float _rotationSpeedBottom = 45.0f;
        public float _rotationSpeedTop = 360.0f;

        public float _damagePerSecond = 0.0f;

        public float _speedFactor = 1.0f;
        public float _energyDrainPerSecond = 0.0f;

        public TornadoConf()
        {
        }

        public TornadoConf(float bottomWidth, float topWidth, float spacing, float cubeHeight, float rotationSpeedBottom, float rotationSpeedTop, float dmgPerSecond, float speedFactor, float energyDrainPerSecond)
        {
            this._bottomWidth = bottomWidth;
            this._topWidth = topWidth;
            this._spacing = spacing;
            this._cubeHeight = cubeHeight;
            this._rotationSpeedBottom = rotationSpeedBottom;
            this._rotationSpeedTop = rotationSpeedTop;
            this._damagePerSecond = dmgPerSecond;
            this._speedFactor = speedFactor;
            this._energyDrainPerSecond = energyDrainPerSecond;
        }

        public static TornadoConf Interpolate(TornadoConf lhs, TornadoConf rhs, float f)
        {
            float bottomWidth = Mathf.Lerp(lhs._bottomWidth, rhs._bottomWidth, f);
            float topWidth = Mathf.Lerp(lhs._topWidth, rhs._topWidth, f);
            float spacing = Mathf.Lerp(lhs._spacing, rhs._spacing, f);
            float cubeHeight = Mathf.Lerp(lhs._cubeHeight, rhs._cubeHeight, f);
            float rotationSpeedBottom = Mathf.Lerp(lhs._rotationSpeedBottom, rhs._rotationSpeedBottom, f);
            float rotationSpeedTop = Mathf.Lerp(lhs._rotationSpeedTop, rhs._rotationSpeedTop, f);
            float dps = Mathf.Lerp(lhs._damagePerSecond, rhs._damagePerSecond, f);
            float speedFactor = Mathf.Lerp(lhs._speedFactor, rhs._speedFactor, f);
            float energyDrain = Mathf.Lerp(lhs._energyDrainPerSecond, rhs._energyDrainPerSecond, f);

            return new TornadoConf(bottomWidth, topWidth, spacing, cubeHeight, rotationSpeedBottom, rotationSpeedTop, dps, speedFactor, energyDrain);
        }
    }

    [System.Serializable]
    public class AttractingParams
    {
        public float minSpeed = 1.0f;
        public float maxSpeed = 10.0f;
        public float distScale = 10.0f;

        public float randomOffsetMin = 0.25f;
        public float randomOffsetMax = 0.5f;
    }
}
