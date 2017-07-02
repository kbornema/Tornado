using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour 
{
    public static RoundTimer Instance { get; private set; }

    public int RemainingSeconds { get; private set; }

    [SerializeField]
    private Text _text;

    [SerializeField]
    private GameOverScreen _gameoverScreen;

	private void Awake()
    {
        Instance = this;

        if (_gameoverScreen.gameObject.activeSelf)
            _gameoverScreen.gameObject.SetActive(false);

        StartCoroutine(RoundTimerRoutine(GameManager.Instance.SecondsOfRound + GameManager.Instance.MinutesOfRound * 60));
    }
	
	private IEnumerator RoundTimerRoutine(int totalTimeSeconds)
    {
        SetTime(totalTimeSeconds);

        while(totalTimeSeconds > 0)
        {   
            yield return new WaitForSeconds(1.0f);

            totalTimeSeconds--;

            SetTime(totalTimeSeconds);
        }
        
        _gameoverScreen.Init();
    }

    private void SetTime(int remainingSeconds)
    {
        RemainingSeconds = remainingSeconds;

        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;

        string minutesText = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        string secondstext = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        _text.text = "Zeit: " + minutesText + ":" + secondstext;
    }

    public void StopRound()
    {
        _gameoverScreen.Init("Wasser ist b�se!");
    }
}
