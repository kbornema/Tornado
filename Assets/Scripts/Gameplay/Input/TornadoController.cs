using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour 
{
    public enum InputMode { Mobile, Desktop, Both }

    [SerializeField]
    private Tornado _tornado;

    [SerializeField]
    private DesktopInput _desktopInput;
    [SerializeField]
    private MobileInput _mobileInput;

    [SerializeField]
    private InputMode _inputMode = InputMode.Both;

    private List<AInputDevice> _deviceInput;

    private void Start()
    {
        _deviceInput = new List<AInputDevice>();

#if UNITY_EDITOR

        if (_inputMode == InputMode.Mobile || _inputMode == InputMode.Both)
            AddDevice(_mobileInput);

        if (_inputMode == InputMode.Desktop || _inputMode == InputMode.Both)
            AddDevice(_desktopInput);

#elif UNITY_STANDALONE
        AddDevice(_desktopInput);
#else
        AddDevice(_mobileInput);
#endif

    }

    private void AddDevice(AInputDevice device)
    {
        device.SetTornado(_tornado);
        _deviceInput.Add(device);
    }

    private void Update()
    {
        for (int i = 0; i < _deviceInput.Count; i++)
        {
            _deviceInput[i].UpdateDevice(Time.deltaTime);
        }

    }
}
