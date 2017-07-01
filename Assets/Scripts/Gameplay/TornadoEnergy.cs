using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoEnergy : MonoBehaviour 
{
    [SerializeField]
    private float _energy = 0.0f;
    public float CurEnergy { get { return _energy; } }

    [SerializeField]
    private float _energyMax = 100.0f;
    public float MaxEnergy { get { return _energyMax; } }

    public float EnergyPercent { get { return _energy / _energyMax; } }

    public void AddEnergy(float delta)
    {
        _energy = Mathf.Clamp(_energy + delta, 0.0f, _energyMax);
    }

    public void UpdateDrain(float drainPerSecond)
    {
        float delta = drainPerSecond * Time.deltaTime;
        AddEnergy(delta);
    }
    
}
