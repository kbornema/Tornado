using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour {

    public delegate void TornadoAction(string code);
    public delegate void CountInformation (string code, float value);


    public static event TornadoAction Ent;

    public static event CountInformation CollectObstacle;
    public static event CountInformation DestroyObject;
    public static event CountInformation DestroyHouse;
    public static event CountInformation DestroyTree;
    //public static event 

    //Schaden, wegschießen, entfernung, 


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
