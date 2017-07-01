using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class AVAnimation : MonoBehaviour
{

    public float speed;
    public AnimationCurve Curve;
    public float Offset;

    private float progress;
    private RectTransform mRectTransform;
    private float startHeight;

    public bool Finished = true;

    // Use this for initialization
    void Start ()
    {
        mRectTransform = this.gameObject.GetComponent<RectTransform>();
        startHeight = mRectTransform.anchoredPosition.y;

    }
	
	// Update is called once per frame
	void Update ()
	{

	    if (Finished)
	        return;

        mRectTransform.anchoredPosition = new Vector2(mRectTransform.anchoredPosition.x, startHeight + Curve.Evaluate(progress)* Offset);

        progress += speed * (Time.deltaTime / 1000);

	    if (progress > 1)
	        Finished = true;

	}

    public void Reset()
    {
        progress = 0;
        Finished = false;
    }
}
