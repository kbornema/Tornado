using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalPoints : MonoBehaviour
{

    private Text _text;
    private RectTransform _rectPosition;
    private float _additionalPoints;
    private float _animationProgress;
    public Vector2 _startPosition;
    private float _pointMultiplier;

    public StatisticsObservable PointObservable;
    public StatisticsObservable CollectablePointsObservable;

    public AnimationCurve BlowUpCurve;
    public AnimationCurve MoveUpurve;
    public float AnimationSpeed;
    public float ShakeAmount;
    
    // Use this for initialization
    void Start()
    {
        _text = this.GetComponent<Text>();
        _additionalPoints = 0;
        _rectPosition = _text.gameObject.GetComponent<RectTransform>();
        _startPosition = _text.gameObject.GetComponent<RectTransform>().anchoredPosition;
        _pointMultiplier = 0;


    }

    // Update is called once per frame
    void Update()
    {

        if (CollectablePointsObservable.pairs.Count > 0)
        {

 
            for (int i = CollectablePointsObservable.pairs.Count - 1; i >= 0; i--)
            {
                if (CollectablePointsObservable.pairs[i].name == "AP")
                {
                    PointObservable.Value += (int)CollectablePointsObservable.pairs[i].value;
                    _additionalPoints += (int)CollectablePointsObservable.pairs[i].value;
                    CollectablePointsObservable.pairs.RemoveAt(i);
                    StartCoroutine(PointShaking());
                }

            }

        }
        
        if (_additionalPoints == 0)
            _text.text = " ";

        if (_additionalPoints > 0)
        {
            _text.text = "+ " + _additionalPoints + " x " + _pointMultiplier;
            _animationProgress += AnimationSpeed * Time.deltaTime / 1000;
        }
        if (_animationProgress > 1)
        {
            this.transform.parent.GetComponent<MenuPoints>().SetAdditionalPoints( (_additionalPoints* _pointMultiplier) - _additionalPoints);
            _additionalPoints = 0;
            _animationProgress = 0;
            _pointMultiplier = 0;
        }

    }

    public IEnumerator PointShaking()
    {
        _pointMultiplier +=1;


        _animationProgress = 0;
        while (_animationProgress <= 1)
        {
            
            _text.gameObject.transform.localScale = new Vector3(1 * BlowUpCurve.Evaluate(_animationProgress), 1 * BlowUpCurve.Evaluate(_animationProgress), 1 * BlowUpCurve.Evaluate(_animationProgress));
            _rectPosition.anchoredPosition = new Vector2(_startPosition.x, _startPosition.y + MoveUpurve.Evaluate(_animationProgress)*450);
            _text.text = "+ " + _additionalPoints + " x "+ _pointMultiplier;
            yield return new WaitForEndOfFrame();
        }

    }
}
