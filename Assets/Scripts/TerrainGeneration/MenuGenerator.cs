using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGenerator : MonoBehaviour
{
    public InputField XScale;
    public InputField YScale;
    public InputField ZScale;

    public InputField Seed;
    public InputField Scene;
    public Button Button;
    public Toggle Toggle;


    [HideInInspector]
    public float SelectedScaleX;
    [HideInInspector]
    public float SelectedScaleY;
    [HideInInspector]
    public float SelectedScaleZ;

    [HideInInspector]
    public int SelectedSeed;
    [HideInInspector]
    public bool IsValley;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
	    Scene.text = Scene.GetComponentsInChildren<Text>().First(t => t.name == "Placeholder").text;
        XScale.text = XScale.GetComponentsInChildren<Text>().First(t => t.name == "Placeholder").text;
        YScale.text = YScale.GetComponentsInChildren<Text>().First(t => t.name == "Placeholder").text;
        ZScale.text = ZScale.GetComponentsInChildren<Text>().First(t => t.name == "Placeholder").text;
    }

    public void OnStart()
    {
        SelectedSeed = 0;

        int.TryParse(Seed.text, out SelectedSeed);
        IsValley = Toggle.isOn;

        float.TryParse(XScale.text, out SelectedScaleX);
        float.TryParse(YScale.text, out SelectedScaleY);
        float.TryParse(ZScale.text, out SelectedScaleZ);

        string scene;
        if (string.IsNullOrEmpty(Scene.text))
            SceneManager.LoadScene("gerd");
        else
        {
            SceneManager.LoadScene(Scene.text);
        }

    }
	

}
