using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsObservable : MonoBehaviour
{

    public float  Value = 0;
    public List<ObservablePair> pairs = new List<ObservablePair>();

    public StatisticsObservable(float val)
    {
        Value = val;
    }
    public StatisticsObservable()
    {
        Value = 0;
    }
}

public struct ObservablePair
{
    public string name;
    public float value;
}
