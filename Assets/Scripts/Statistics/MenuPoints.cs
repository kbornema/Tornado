using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPoints : MonoBehaviour
{

    private Text _text;
    private float _lastPoints;
    private float _animationProgress;

    public StatisticsObservable PointObservable;
    public StatisticsObservable CollectablePointsObservable;

    public AnimationCurve Curve;
    public float AnimationSpeed;
    public float ShakeAmount;

    private bool shaking = false;

    
	// Use this for initialization
	void Start ()
	{
	    _text = this.GetComponent<Text>();
	    _lastPoints = 0;

	}
	
	// Update is called once per frame
	void Update ()
	{

	    /*if (_lastPoints != PointObservable.Value)
	    {
	        _lastPoints = PointObservable.Value;
	        StartCoroutine(PointShaking());
	    }
        */

	    if (CollectablePointsObservable.pairs.Count > 0)
	    {

	        float points = 0;
	        for (int i = CollectablePointsObservable.pairs.Count - 1; i >= 0; i--)
	        {
	            if (CollectablePointsObservable.pairs[i].name == "DP")
	            {
	                points += CollectablePointsObservable.pairs[i].value;
                    CollectablePointsObservable.pairs.RemoveAt(i);
                }

            }
	        if ((int)points > 0)
	        {
	            _lastPoints += (int)points;
	            PointObservable.Value += (int)points;
	            StartCoroutine(PointShaking());
	        }
	    }

	    _text.text = "Points: "+ PointObservable.Value;
	}

    public void SetAdditionalPoints(float ammount)
    {
        _lastPoints += ammount;
        PointObservable.Value = _lastPoints;
        StartCoroutine(PointShaking());
    }

    public IEnumerator PointShaking()
    {
        if(_animationProgress>0.6)
            _animationProgress /= 2;
        if (!shaking)
        {
            shaking = true;
            while (_animationProgress < 1)
            {
                _animationProgress += AnimationSpeed * Time.deltaTime / 1000;
                _text.gameObject.transform.localScale = new Vector3(1 * Curve.Evaluate(_animationProgress), 1 * Curve.Evaluate(_animationProgress), 1 * Curve.Evaluate(_animationProgress));
                yield return new WaitForEndOfFrame();
            }
            _animationProgress = 0;
            shaking = false;
        }
    }
}
