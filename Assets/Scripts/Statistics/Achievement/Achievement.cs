using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Achievement
{
    public string Name;
    public string Description;
    public bool Reached;
    public int NumberOfPoints;

    public StatisticsObservable Observable;
    public float ValueLargerThan;

    public bool Evaluate()
    {
        if (!Reached && Observable.Value > ValueLargerThan)
        {
            Reached = true;
            Statistics.NotifyCollectPoints("DP", NumberOfPoints);
            return true;
        }
        return false;
    }

}
