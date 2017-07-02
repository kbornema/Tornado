using UnityEngine;


public class WeatherManager : MonoBehaviour
{
    #region member vars

    public GameObject Camera;

    public GameObject RainBox;

    public Light SunLight;

    [Range(0, 3000)]
    public int SlowRainNum;

    [Range(3000, 6000)]
    public int SummerRainNum;

    [Range(6000, 30000)]
    public int HeavyRainNum;

    [Range(0, 8)]
    public float SunLightIntensity;

    [Range(0, 8)]
    public float SlowRainLightIntensity;

    [Range(0, 8)]
    public float SummerRainLightIntensity;

    [Range(0, 8)]
    public float HeavyRainLightIntensity;

    [Range(0, 18)]
    public float SunFogIntensity;

    [Range(18, 25)]
    public float SlowRainFogIntensity;

    [Range(18, 25)]
    public float SummerRainFogIntensity;

    [Range(18, 25)]
    public float HeavyRainFogIntensity;

    [Range(18, 25)]
    public float ThunderFogIntensity;

    [Range(0, 1)]
    public float FadeWeatherSpeed;

    private float _ligthIntensity;

    private float _ligthIntensityTarget;

    private int _rainNum;

    private int _rainNumTarget;

    private float _fogIntensity;

    private float _fogIntensityTarget;

    private bool _fading = true;

    private ParticleSystem _rainBoxSystem;

    private ParticleSystem.EmissionModule _em;

    //private GlobalFog _fogController;


    #endregion

    #region methods

    /// <summary>
    /// Énabled selected weather and stops old weather elements
    /// </summary>
    /// <param name="weather"></param>
    public void SwitchWeather(Weather weather)
    {
        ClearWeather();
        switch (weather)
        {
            case Weather.Sun:
                PeformSun();
                break;
            case Weather.HeavyRain:
                PeformHeavyRain();
                break;
            case Weather.SummerRain:
                PeformSummerRain();
                break;
            case Weather.SlowRain:
                PeformSlowRain();
                break;
            case Weather.Thunder:
                PeformThunder();
                break;
            default:
                PeformSun();
                break;
        }
        _fading = true;
    }

    private void ClearWeather()
    {
    }

    private void PeformHeavyRain()
    {
        RainBox.SetActive(true);

        _ligthIntensityTarget = HeavyRainLightIntensity;
        _rainNumTarget = HeavyRainNum;
        _fogIntensityTarget = HeavyRainFogIntensity;

    }

    private void PeformSlowRain()
    {
        RainBox.SetActive(true);


        _ligthIntensityTarget = SlowRainLightIntensity;
        _rainNumTarget = SlowRainNum;
        _fogIntensityTarget = SlowRainFogIntensity;
    }

    private void PeformSummerRain()
    {
        RainBox.SetActive(true);

        _ligthIntensityTarget = SummerRainLightIntensity;
        _rainNumTarget = SummerRainNum;
        _fogIntensityTarget = SummerRainFogIntensity;
    }

    private void PeformSun()
    {
        RainBox.SetActive(false);

        _ligthIntensityTarget = SunLightIntensity;
        _rainNumTarget = 42;
        _fogIntensityTarget = SunFogIntensity;
    }

    private void PeformThunder()
    {
        RainBox.SetActive(true);

        _ligthIntensityTarget = HeavyRainLightIntensity;
        _rainNumTarget = HeavyRainNum;
        _fogIntensityTarget = ThunderFogIntensity;
    }

    private void FadeWeather()
    {
        _ligthIntensity = _ligthIntensityTarget * FadeWeatherSpeed + _ligthIntensity * (1 - FadeWeatherSpeed);
        SunLight.intensity = _ligthIntensity;

        _rainNum = (int)((float)_rainNumTarget * FadeWeatherSpeed + (float)_rainNum * (1 - FadeWeatherSpeed));
        _em.rate = new ParticleSystem.MinMaxCurve(_rainNum);

        _fogIntensity = (_fogIntensityTarget * FadeWeatherSpeed/2 + _fogIntensity * (1 - FadeWeatherSpeed/2));
        //_fogController.height = _fogIntensity;

        //Check if fading is nearly complete to reduce slowly _em.rate placing stuff;
        if (Mathf.Abs(_rainNum - _rainNumTarget) <= 100 && Mathf.Abs(_ligthIntensity - _ligthIntensityTarget) <= 0.1f   &&   Mathf.Abs(_fogIntensity - _fogIntensityTarget) <= 0.2f)
            _fading = false;

        Debug.Log("Still fading");

    }

    // Use this for initialization
    void Start()
    {
        RainBox.SetActive(false);
        _rainBoxSystem = RainBox.GetComponent<ParticleSystem>();
        _em = _rainBoxSystem.emission;
        PeformThunder();

        /*_fogController = Camera.GetComponent<GlobalFog>();
        _fogController.enabled = true;
        _fogController.height = 0;*/
    }

    // Update is called once per frame
    void Update()
    {
        if(_fading)
            FadeWeather();

        //Debug Handling

        if (Input.GetKeyDown(KeyCode.Alpha0)) SwitchWeather(Weather.Sun);
        else if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeather(Weather.SlowRain);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeather(Weather.SummerRain);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeather(Weather.HeavyRain);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeather(Weather.Thunder);
    }

    #endregion
}

public enum Weather
{
    Sun,
    SlowRain,
    SummerRain,
    HeavyRain,
    Thunder
}