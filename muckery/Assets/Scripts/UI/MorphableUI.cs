using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MorphableUI : MonoBehaviour
{
    [SerializeField] List<GameObject> _gamepadIcons = new();
    [SerializeField] List<GameObject> _keyboardIcons = new();

    void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        UpdateIcons();
    }

    void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    void OnActionChange( object obj, InputActionChange change )
    {
        if ( change == InputActionChange.ActionPerformed )
        {
            UpdateIcons();
        }
    }

    void UpdateIcons()
    {
        bool isGamepad = Gamepad.current != null && InputSystem.GetDevice<Gamepad>() != null && Gamepad.current.wasUpdatedThisFrame;

        InputDevice lastUsedDevice = GetLastUsedDevice();

        if ( lastUsedDevice != null )
        {
            isGamepad = lastUsedDevice is Gamepad;
        }

        SetIcons( _gamepadIcons, isGamepad );
        SetIcons( _keyboardIcons, !isGamepad );
    }

    void SetIcons( List<GameObject> icons, bool active )
    {
        foreach ( GameObject icon in icons )
        {
            if ( icon != null )
            {
                icon.SetActive( active );
            }
        }
    }

    InputDevice GetLastUsedDevice()
    {
        if ( Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame )
        {
            return Gamepad.current;
        }

        if ( Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame )
        {
            return Keyboard.current;
        }

        if ( Mouse.current != null && Mouse.current.wasUpdatedThisFrame )
        {
            return Mouse.current;
        }

        return null;
    }
}