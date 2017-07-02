using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour 
{
    [SerializeField]
    private Text _pointsText;

    [SerializeField]
    private StatisticsObservable _pointsObservable;

    [SerializeField]
    private Button _repeatButton;
    [SerializeField]
    private Button _mainMenuButton;
    [SerializeField]
    private Button _nextButton;
    

    public void SetPoints(int totalPoints, string addText)
    {
        _pointsText.text = "- Du hast " + totalPoints.ToString() + " Punkte erspielt! -";

        if(addText != null && addText.Length > 0)
        {
            _pointsText.text += "\n" + addText;
        }
    }

    public void Init(string additionalText = "")
    {
        _repeatButton.onClick.AddListener(OnRepeat);
        _mainMenuButton.onClick.AddListener(OnMainMenu);
        _nextButton.onClick.AddListener(OnNext);

        gameObject.SetActive(true);
        Time.timeScale = 0.0f;

        SetPoints((int)_pointsObservable.Value, additionalText);
    }

    private void OnButton()
    {
        Time.timeScale = 1.0f;
    }

    private void OnNext()
    {
        GameManager.Instance.RandomizeSeed();
        OnButton();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMainMenu()
    {
        OnButton();
        SceneManager.LoadScene("menu");
    }

    private void OnRepeat()
    {   
        OnButton();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
