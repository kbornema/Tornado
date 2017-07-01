using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;

using UnityEngine;

public static class Statistics {

    public delegate void CountInformation (string code, float value);
    public delegate void StateInformation(string code, float value);

    public static event CountInformation CollectObstacle;
    public static event CountInformation ThrowObject;
    public static event CountInformation DestroyObject;
    public static event CountInformation DestroyHouse;
    public static event CountInformation DestroyTree;
    public static event CountInformation Damage;
    public static event CountInformation TimeTornadoBigMode;
    public static event CountInformation DistanceThrown;

    public static event StateInformation NumberOfObstacleInTornado;


    public static void NotifyCollectObstacle(string code, float value)
    {
        if (CollectObstacle != null) CollectObstacle.Invoke(code, value);
    }
    public static void NotifyThrowObject(string code, float value)
    {
        if (ThrowObject != null) ThrowObject.Invoke(code, value);
    }
    public static void NotifyDestroyObject(string code, float value)
    {
        if (DestroyObject != null) DestroyObject.Invoke(code, value);
    }

    public static void NotifyDestroyTree(string code, float value)
    {
        if(DestroyTree != null) DestroyTree.Invoke(code, value);
    }
    public static void NotifyDestroyHouse(string code, float value)
    {
        if (DestroyHouse != null) DestroyHouse.Invoke(code, value);
    }

    public static void NotifyDamage(string code, float value)
    {
        if (Damage != null) Damage.Invoke(code, value);
    }
    public static void NotifyTimeTornadoBigMode(string code, float value)
    {
        if (TimeTornadoBigMode != null) TimeTornadoBigMode.Invoke(code, value);
    }

    public static void NotifyDistanceThrown(string code, float value)
    {
        if (DistanceThrown != null) DistanceThrown.Invoke(code, value);
    }

    public static void NotifyNumberOfObstacleInTornado(string code, float value)
    {
        if (NumberOfObstacleInTornado != null) NumberOfObstacleInTornado.Invoke(code, value);
    }
}

