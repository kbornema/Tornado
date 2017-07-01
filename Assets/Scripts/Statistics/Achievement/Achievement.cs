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

    public StatisticsObservable Observable;
    public float ValueLargerThan;

    public bool Evaluate()
    {
        if (!Reached && Observable.Value > ValueLargerThan)
        {
            Reached = true;

            Debug.Log(Name);
            return true;
        }
        return false;
    }

}
