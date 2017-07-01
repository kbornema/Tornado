using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementHandler : MonoBehaviour
{
    public AchievementUI AVHandler;

    public Achievement[] Achievements;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.B))
	    {
	        Statistics.NotifyDamage("test", 1);
	    }

	    foreach (var achievement in Achievements)
	    {
	        if (achievement.Evaluate())
	        {
                AVHandler.AddAchievement(achievement);

            }

        }
	}
}
