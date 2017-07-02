using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGenerator : MonoBehaviour
{
    public InputField Seed;
    public InputField Scene;
    public Button Button;
    public Toggle Toggle;

    public InputField Minutes;
    public InputField Seconds;


	// Use this for initialization
	void Start () 
    {   
        if(Scene)
        {
	        Scene.text = Scene.GetComponentsInChildren<Text>().First(t => t.name == "Placeholder").text;
        }
    }

    public void OnStart()
    {
        int selectedSeed = 0;

        if (Seed)
        {
            int.TryParse(Seed.text, out selectedSeed);
        }

        bool isValley = false;

        if(Toggle)
            isValley = Toggle.isOn;


        int minutes = 5;
        int.TryParse(Minutes.text, out minutes);

        int seconds = 0;
        int.TryParse(Seconds.text, out seconds);

        GenerationSettings.Instance.MinutesOfRound = minutes;
        GenerationSettings.Instance.SecondsOfRound = seconds;

        GenerationSettings.Instance.seed = selectedSeed;
        GenerationSettings.Instance.isValley = isValley;

        if (string.IsNullOrEmpty(Scene.text))
        {
            SceneManager.LoadScene("gerd");
        }

        else
        {
            SceneManager.LoadScene(Scene.text);
        }

    }
	

}
