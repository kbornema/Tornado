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
    private InputMode _mode;

    private List<IInputDevice> _deviceInput;

    private void Start()
    {

        _deviceInput = new List<IInputDevice>();

#if UNITY_EDITOR

    if (_mode == InputMode.Mobile || _mode == InputMode.Both)
    {
        _deviceInput.Add(_mobileInput);
    }

    if (_mode == InputMode.Desktop || _mode == InputMode.Both)
    {
        _deviceInput.Add(_desktopInput);
    }

#else

    #if UNITY_STANDALONE
            _deviceInput.Add(_desktopInput);
    #else
            _deviceInput.Add(_mobileInput);
    #endif
#endif
    }

    private void Update()
    {
        for (int i = 0; i < _deviceInput.Count; i++)
        {
            _deviceInput[i].Update(_tornado, Time.deltaTime);
        }

    }
}
