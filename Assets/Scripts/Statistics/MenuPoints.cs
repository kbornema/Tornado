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

    public AnimationCurve Curve;
    public float AnimationSpeed;
    public float ShakeAmount;

    
	// Use this for initialization
	void Start ()
	{
	    _text = this.GetComponent<Text>();
	    _lastPoints = 0;

	}
	
	// Update is called once per frame
	void Update ()
	{

	    if (_lastPoints != PointObservable.Value)
	    {
	        _lastPoints = PointObservable.Value;
	        StartCoroutine(PointShaking());
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

        while (_animationProgress < 1)
        {
            _animationProgress += AnimationSpeed * Time.deltaTime / 1000;
            _text.gameObject.transform.localScale = new Vector3(1* Curve.Evaluate(_animationProgress), 1 * Curve.Evaluate(_animationProgress), 1 * Curve.Evaluate(_animationProgress));
            yield return new WaitForEndOfFrame();
        }
        _animationProgress = 0;
    }
}
