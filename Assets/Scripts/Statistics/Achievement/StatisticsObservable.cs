using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsObservable : MonoBehaviour
{

    public float Value = 0;

    public StatisticsObservable(float val)
    {
        Value = val;
    }
    public StatisticsObservable()
    {
        Value = 0;
    }
}
