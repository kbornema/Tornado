using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsObserver : MonoBehaviour
{

    public StatisticsObservable fullObstacleThrownDistance;
    public StatisticsObservable timeTornadoInBigMode;
    public StatisticsObservable fullDamage;
    public StatisticsObservable numberOfDestroyedTrees;
    public StatisticsObservable numberOfDestroyedHouses;
    public StatisticsObservable numberOfDestroyObjects;
    public StatisticsObservable numberOfThrownObject;
    public StatisticsObservable numberOfCollectObstacle;
    public StatisticsObservable points;

    public StatisticsObservable numberOfMaximumObstaclesInTornado;

    private static bool _started = false;

    void Start () {

            Statistics.CollectObstacle += Statistics_CollectObstacle;
            Statistics.ThrowObject += Statistics_ThrowObject;
            Statistics.DestroyObject += Statistics_DestroyObject;
            Statistics.DestroyHouse += Statistics_DestroyHouse;
            Statistics.DestroyTree += Statistics_DestroyTree;
            Statistics.Damage += Statistics_Damage;
            Statistics.TimeTornadoBigMode += Statistics_TimeTornadoBigMode;
            Statistics.DistanceThrown += Statistics_DistanceThrown;
            Statistics.CollectPoints += Statistics_CollectPoints;

            Statistics.NumberOfObstacleInTornado += Statistics_NumberOfObstacleInTornado;
    }

    private void Statistics_CollectPoints(string code, float value)
    {
        points.Value += value;
    }

    private void Statistics_NumberOfObstacleInTornado(string code, float value)
    {
        if (numberOfMaximumObstaclesInTornado.Value < value)
            numberOfMaximumObstaclesInTornado.Value = value;
    }

    private void Statistics_DistanceThrown(string code, float value)
    {
        fullObstacleThrownDistance.Value += value;
    }

    private void Statistics_TimeTornadoBigMode(string code, float value)
    {
        timeTornadoInBigMode.Value += value;
    }

    private void Statistics_Damage(string code, float value)
    {
        fullDamage.Value += value;
    }

    private void Statistics_DestroyTree(string code, float value)
    {
        numberOfDestroyedTrees.Value += value;
        Statistics.NotifyDestroyObject(code, value);
    }

    private void Statistics_DestroyHouse(string code, float value)
    {
        numberOfDestroyedHouses.Value += value;
        Statistics.NotifyDestroyObject(code, value);
    }

    private void Statistics_DestroyObject(string code, float value)
    {
        numberOfDestroyObjects.Value += value;
    }

    private void Statistics_ThrowObject(string code, float value)
    {
        numberOfThrownObject.Value += value;
    }

    private void Statistics_CollectObstacle(string code, float value)
    {
        numberOfCollectObstacle.Value += value;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
