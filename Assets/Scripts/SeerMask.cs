using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class SeerMask : MonoBehaviour
{
    [SerializeField] Volume _postProcessVolume;
    bool _isActive = false;

    InputActions _inputActions;

    public static Action<bool> OnMaskToggled;

    void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.UseMask.performed += OnMaskAbilityPerformed;
    }

    void Start()
    {
        if (_postProcessVolume == null)
        {
            Debug.LogError("Assign a Volume in SeerMask!");
            enabled = false;
            return;
        }

        _postProcessVolume.enabled = false;
    }

    void OnEnable()
    {
        // Enable the input only if this mask is active
        _inputActions.Player.Enable();
    }

    void OnDisable()
    {
        // Disable the input so it cannot trigger while component is off
        _inputActions.Player.Disable();

        if (_isActive)
        {
            _isActive = false;
            _postProcessVolume.enabled = false;
            OnMaskToggled?.Invoke(false);
        }
    }

    void OnMaskAbilityPerformed(InputAction.CallbackContext context)
    {
        // Only toggle if the GameObject is active and input is enabled
        if (!_inputActions.Player.enabled || !isActiveAndEnabled)
            return;

        _isActive = !_isActive;
        _postProcessVolume.enabled = _isActive;
        OnMaskToggled?.Invoke(_isActive);
    }
}