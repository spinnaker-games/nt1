using UnityEngine;
using System.Collections;
using System;

public class PlayerFreeLookState : PlayerBaseState
{
    bool _shouldFadeAnim;
    readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); //integers are processed faster than strings.
    readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    const float AnimatorDampTime = 0.075f;
    const float CrossFadeDuration = 0.2f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFadeAnim = true) : base(stateMachine)
    {
        this._shouldFadeAnim = shouldFadeAnim;
    }

    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnTarget;
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;
        _stateMachine.InputReader.PauseActivateEvent += OnPause;

        _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f); //prevents player from being in the middle of another animation when this state begins

        if (_shouldFadeAnim)
        {
            _stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            _stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }

        _stateMachine.PlayerModel.SetActive(true);

        _stateMachine.IsDisguised = false;

        _stateMachine.IsMorphed = false;

        _stateMachine.SlimeTrail.SetActive(true);
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.PlayerConfig.FreeLookMovementSpeed, deltaTime);

        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;
        //Debug.Log("Player Movement = " + _stateMachine.IsMoving + "Disguise " + _stateMachine.IsDisguised);

        HandleMoveSFX();

        if (_stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime); //TODO: Fix magic numbers
            return;
        }

        _stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime); //TODO: Fix magic numbers

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputReader.TargetEvent -= OnTarget;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;
        _stateMachine.InputReader.PauseActivateEvent -= OnPause;

        _stateMachine.MoveSFX.Stop();

        _stateMachine.SlimeTrail.SetActive(false);
    }

    void OnTarget()
    {
        if (!_stateMachine.Targeter.SelectTarget()) { return; }

        _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
    }

    void OnMorphSlot( int slot )
    {
        _stateMachine.SwitchState( new PlayerMorphingState( _stateMachine, slot + 1 ) );
    }

    void OnPause()
    {
        _stateMachine.SwitchState( new PlayerPauseState(_stateMachine) );
    }

        void HandleMoveSFX()
    {
        if (_stateMachine.IsMoving)
        {
            if (!_stateMachine.MoveSFX.isPlaying)
                _stateMachine.MoveSFX.Play();
        }
        else
        {
            if (_stateMachine.MoveSFX.isPlaying)
                _stateMachine.MoveSFX.Stop();
        }
    }
}