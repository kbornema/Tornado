using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGenerator : MonoBehaviour
{

    public InputField Seed;
    public InputField Scene;
    public Button Button;
    public Toggle Toggle;

    [HideInInspector]
    public int SelectedSeed;
    [HideInInspector]
    public bool IsValley;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void OnStart()
    {
        SelectedSeed = 0;

        int.TryParse(Seed.text, out SelectedSeed);
        IsValley = Toggle.isOn;

        string scene;
        if (string.IsNullOrEmpty(Scene.text))
            SceneManager.LoadScene("gerd");
        else
        {
            SceneManager.LoadScene(Scene.text);
        }

    }
	

}
