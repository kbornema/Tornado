using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEnergyBar : MonoBehaviour 
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Tornado _target;

    private void Update()
    {
        SetPercent(_target.Energy.EnergyPercent);
    }

	public void SetPercent(float value)
    {
        _slider.value = value;
    }
}
