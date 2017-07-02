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

        GameManager.Instance.seed = selectedSeed;
        GameManager.Instance.isValley = isValley;

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
