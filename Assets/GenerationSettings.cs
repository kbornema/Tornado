using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSettings : MonoBehaviour
{
    public static GenerationSettings Instance { get; private set; }

    public bool isValley = false;

    public int seed = 0;

    public void RandomizeSeed()
    {
        seed = Random.Range(0, 10000);
    }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }



}
