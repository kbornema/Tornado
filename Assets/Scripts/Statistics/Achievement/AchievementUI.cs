using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{

    public GameObject AchievementUIHolder;

    private Queue<Achievement> remainingAchievements = new Queue<Achievement>();

    private bool runningAnimation = false;

    void Update()
    {
        if (remainingAchievements.Count > 0)
        {
            if(!runningAnimation)
                StartCoroutine(StartAchievement(remainingAchievements.Dequeue()));
            
        }

        
    }

    public void AddAchievement(Achievement e)
    {
        remainingAchievements.Enqueue(e);
    }




    private IEnumerator StartAchievement(Achievement e)
    {
        runningAnimation = true;
        var av = AchievementUIHolder.GetComponent<AVAnimation>();
        av.Reset();

        var texts = av.GetComponentsInChildren<Text>();

        texts.First(t => t.name == "Name").text = e.Name;
        texts.First(t => t.name == "Description").text = e.Description;


        while (!av.Finished)
        {
            yield return new WaitForEndOfFrame();
        }

        runningAnimation = false;
    }
}
