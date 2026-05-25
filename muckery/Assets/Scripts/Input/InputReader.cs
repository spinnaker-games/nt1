using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputActions.IPlayerActions
{
    public bool IsAttacking { get; set; }
    public bool IsBlocking { get; set; }
    public Vector2 MovementValue { get; set; }
    public Vector2 LookValue { get; set; }

    public event Action JumpActivateEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action AimActivateEvent;
    public event Action AimCancelEvent;
    public event Action VantagePointActivateEvent;
    public event Action VantagePointCancelEvent;
    public event Action TopDownActivateEvent;
    public event Action TopDownCancelEvent;
    public event Action SideScrollActivateEvent;
    public event Action SideScrollCancelEvent;
    public event Action ChaseCameraActivateEvent;
    public event Action ChaseCameraCancelEvent;
    public event Action MorphActivateEvent;
    public event Action AbilityActivateEvent;
    public event Action PauseActivateEvent;

    InputActions _inputActions;

    bool _vantageActive = false;
    bool _topDownActive = false;
    bool _sideScrollActive = false;
    bool _chaseCameraActive = false;


    void Start()
    {
        _inputActions = new InputActions();
        _inputActions.Player.SetCallbacks(this); // this hooks up the funtions below to their callback counterparts in InputActions.cs
        _inputActions.Player.Enable();
    }

    void OnDestroy()
    {
        _inputActions.Player.Disable();
    }

    public void OnAbilityActivate(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        AbilityActivateEvent?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        JumpActivateEvent?.Invoke();
    }

    public void OnMorph(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        MorphActivateEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        PauseActivateEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        DodgeEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookValue = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        TargetEvent?.Invoke();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AimActivateEvent?.Invoke();
        }
        else if (context.canceled)
        {
            AimCancelEvent?.Invoke();
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBlocking = true;
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }

    public void OnVantagePoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _vantageActive = !_vantageActive;

            if (_vantageActive)
            {
                VantagePointActivateEvent?.Invoke();
            }
            else
            {
                VantagePointCancelEvent?.Invoke();
            }
        }
    }

    public void OnTopDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _topDownActive = !_topDownActive;

            if (_topDownActive)
            {
                TopDownActivateEvent?.Invoke();
            }
            else
            {
                TopDownCancelEvent?.Invoke();
            }
        }
    }

    public void OnSideScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _sideScrollActive = !_sideScrollActive;

            if (_sideScrollActive)
            {
                SideScrollActivateEvent?.Invoke();
            }
            else
            {
                SideScrollCancelEvent?.Invoke();
            }
        }
    }

    public void OnChaseCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _chaseCameraActive = !_chaseCameraActive;

            if (_chaseCameraActive)
            {
                ChaseCameraActivateEvent?.Invoke();
            }
            else
            {
                ChaseCameraCancelEvent?.Invoke();
            }
        }
    }
}