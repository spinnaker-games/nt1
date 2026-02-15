using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class SeerMask : MonoBehaviour
{
    [SerializeField] Volume _postProcessVolume;
    bool _isActive = false;

    // Public static action for other scripts to subscribe to
    public static Action<bool> OnMaskToggled;

    void Start()
    {
        if (_postProcessVolume == null)
        {
            Debug.LogError("Assign a Volume in SeerMask!");
            enabled = false;
            return;
        }

        _postProcessVolume.enabled = false; // start off
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.digit1Key.wasPressedThisFrame) //TODO: Replace with proper input action
        {
            _isActive = !_isActive;
            _postProcessVolume.enabled = _isActive;

            // Invoke the action for subscribers
            OnMaskToggled?.Invoke(_isActive);
        }
    }
}