using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isValley = false;

    public int seed = 0;

    public int MinutesOfRound = 5;
    public int SecondsOfRound = 0;

    public void RandomizeSeed()
    {
        seed = Random.Range(0, 10000);
    }

    private void Awake()
    {
        Application.targetFrameRate = 30;

        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }



}
