using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Tornado : MonoBehaviour 
{
    [SerializeField]
    private TornadoSegment[] _segments;
    [SerializeField]
    private GameObject _segmentRoot;
    [SerializeField]
    private int _numSegments;

    [SerializeField]
    private TornadoSegment _segmentPrefab;

    [SerializeField]
    private float _colliderScale = 1.0f;

    [SerializeField]
    private SphereCollider _physicalCollider;

    [SerializeField]
    private CapsuleCollider _attractionCollider;

    [SerializeField]
    private CharacterController _characterController;
    private Vector3 _characterControllerMovement;

    //[SerializeField, Range(0.0f, 1.0f)]
    //private float _curConfPercent = 0.0f;

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

        //_curConfPercent = percent;
        _configurationCurrent = TornadoConf.Interpolate(_configurationMin, _configurationMax, percent);

        _attractionCollider.center = new Vector3(0.0f, GetCenterHeight(false), 0.0f);
        _attractionCollider.height = GetTotalHeight();
        _attractionCollider.radius = GetCurrentRadius();


        if (_physicalCollider)
        {
            _physicalCollider.radius = GetCurrentRadius();
            _physicalCollider.center = new Vector3(0.0f, _physicalCollider.radius, 0.0f);

        }

        if(_characterController)
        {
            _characterController.center = _attractionCollider.center;
            _characterController.height = _attractionCollider.height;
            _characterController.radius = _attractionCollider.radius;
        }

        float scale = _configurationCurrent._totalScale;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    /*
    public void ChangeConfPercent(float deltaPercent)
    {
        SetConfPercent(_curConfPercent + deltaPercent);
    }
    */

    private float _lifeTime;

    private void CreateSegments()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            if (Application.isPlaying)
                Destroy(_segments[i].gameObject);
            else
                DestroyImmediate(_segments[i].gameObject);
        }

        _segments = new TornadoSegment[_numSegments];

        for (int i = 0; i < _numSegments; i++)
        {
            _segments[i] = Instantiate(_segmentPrefab.gameObject).GetComponent<TornadoSegment>();
            _segments[i].transform.SetParent(_segmentRoot.transform);
            _segments[i].transform.localPosition = Vector3.zero;
        }
    }

    public void AddForce(Vector3 force, ForceMode mode)
    {

        if(_tornadoRigidbody)
            _tornadoRigidbody.AddForce(force, mode);

        if (_characterController)
        {
            _characterControllerMovement += force;
        }
           
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

        if(Application.isPlaying)
            _lifeTime += Time.deltaTime;

        SetConfPercent(_energy.EnergyPercent);

        UpdateTornadoSegments(_configurationCurrent);

        UpdateAttractedObjects(Time.deltaTime);





        //Debug.DrawRay(gameObject.transform.position, GetProjectedMovement() * 100.0f, Color.black, 100.0f);


        if (_energy && Application.isPlaying)
            _energy.UpdateDrain(_configurationCurrent._energyDrainPerSecond);

        if (_characterController)
        {
            _characterController.SimpleMove(_characterControllerMovement);
        }
	}


    private void UpdateAttractedObjects(float deltaTime)
    {
        Vector3 center = gameObject.transform.position + new Vector3(0.0f, GetCenterHeight(true), 0.0f);

        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            Vector3 customCenter = center + Random.onUnitSphere * Random.Range(_attractingParams.randomOffsetMin, _attractingParams.randomOffsetMax) * transform.localScale.y;
            AttractingObject obj = _attractedGameObjects[i];

            float distSq = (obj.transform.position - customCenter).sqrMagnitude;

            float inverseDistSq = 1.0f / distSq;

            Vector3 toCenter = customCenter - obj.transform.position;

            float speed = Mathf.Clamp(inverseDistSq * _attractingParams.distScale, _attractingParams.minSpeed, _attractingParams.maxSpeed) * transform.localScale.y;

            Vector3 dir = toCenter.normalized * speed;

            obj.AddForce(dir, ForceMode.Impulse);
        }
    }

    private void UpdateTornadoSegments(TornadoConf conf)
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            float curHeight = GetHeight(i);

            _segments[i].MeshObj.transform.localPosition = new Vector3(0.0f, curHeight, 0.0f);
            
            float percent = (float)i / (float)_numSegments;

            float curWidth = Mathf.Lerp(conf._bottomWidth, conf._topWidth, percent);

            float curRotation = Mathf.Lerp(conf._rotationSpeedBottom, conf._rotationSpeedTop, percent) * _lifeTime;

            _segments[i].MeshObj.transform.localRotation = Quaternion.Euler(0.0f, curRotation, 0.0f);

            _segments[i].MeshObj.transform.localScale = new Vector3(curWidth, conf._cubeHeight, curWidth);
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

    public float GetCenterHeight(bool worldScaled)
    {
        float scale = 1.0f;

        if (worldScaled)
            scale = transform.localScale.y;

        return GetTotalHeight() * 0.5f * scale;
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

        obj.SetTornadoScale(transform.localScale.y);
        
        _attractedGameObjects.Add(obj);
    }

    public void ReleaseAllObectsForward()
    {
        Vector3 forward;
        Vector3 right;

        GameCamera.Instance.GetMovement(out forward, out right);

        GameCamera.Instance.Focus();

        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            _attractedGameObjects[i].SetState(AttractingObject.State.Free);

            Vector3 velocity = _attractedGameObjects[i].GetVelocity();

            float velocityForce = velocity.magnitude;

            velocity = forward * velocityForce * 2.0f;


            _attractedGameObjects[i].SetVelocity(velocity);
        }

        PreventFromAttractingCollision();

        _attractedGameObjects.Clear();
    }

    public void ReleaseAllObjectsVelocity()
    {
        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            _attractedGameObjects[i].SetState(AttractingObject.State.Free);
        }

        PreventFromAttractingCollision();

        _attractedGameObjects.Clear();
    }

    private void PreventFromAttractingCollision()
    {
        for (int i = 0; i < _attractedGameObjects.Count; i++)
        {
            Physics.IgnoreCollision(_attractionCollider, _attractedGameObjects[i].MyCollider, true);
            StartCoroutine(UnignoreCollision(_attractionCollider, _attractedGameObjects[i].MyCollider));
        }
    }

    private IEnumerator UnignoreCollision(CapsuleCollider _collider, Collider collider)
    {
        yield return new WaitForSeconds(0.5f);

        Physics.IgnoreCollision(_collider, collider, false);
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

        public float _totalScale = 1.0f;

        public TornadoConf(float bottomWidth, float topWidth, float spacing, float cubeHeight, float rotationSpeedBottom, float rotationSpeedTop, float dmgPerSecond, float speedFactor, float energyDrainPerSecond, float totalScale)
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
            this._totalScale = totalScale;
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
            float totalScale = Mathf.Lerp(lhs._totalScale, rhs._totalScale, f);

            return new TornadoConf(bottomWidth, topWidth, spacing, cubeHeight, rotationSpeedBottom, rotationSpeedTop, dps, speedFactor, energyDrain, totalScale);
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

    public float GetVelocityLength()
    {
        return GetVelocity().magnitude;
    }

    public Vector3 GetVelocity()
    {
        if (_characterController)
        {
            return _characterController.velocity;
        }

        else if(_tornadoRigidbody)
            return _tornadoRigidbody.velocity;

        else
        {
            Debug.Assert(false);
            return Vector3.zero;
        }
    }
}
