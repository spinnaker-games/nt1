using UnityEngine;
using System.Collections;

public class PlayerPropKnifeState : PlayerBaseState
{
    bool _shouldFadeAnim;
    readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    readonly int FreeLookZoomBlendTreeHash = Animator.StringToHash("FreeLookZoomBlendTree");
    const float AnimatorDampTime = 0.075f;
    const float CrossFadeDuration = 0.2f;

    public PlayerPropKnifeState(PlayerStateMachine stateMachine, bool shouldFadeAnim = true) : base(stateMachine)
    {
        this._shouldFadeAnim = shouldFadeAnim;
    }

    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnTarget;
        _stateMachine.InputReader.AimActivateEvent += OnAim;
        _stateMachine.InputReader.VantagePointActivateEvent += OnVantagePointActivate;
        _stateMachine.InputReader.TopDownActivateEvent += OnTopDownActivate;
        _stateMachine.InputReader.SideScrollActivateEvent += OnSideScrollActivate;
        _stateMachine.InputReader.ChaseCameraActivateEvent += OnChaseCameraActive;
        _stateMachine.InputReader.JumpActivateEvent += OnJump;
        _stateMachine.InputReader.MorphActivateEvent += OnMorph;
        _stateMachine.InputReader.AttackEvent += OnAttack;

        if (_shouldFadeAnim)
        {
            _stateMachine.Animator.CrossFadeInFixedTime(FreeLookZoomBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            _stateMachine.Animator.Play(FreeLookZoomBlendTreeHash);
        }

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.Knife.SetActive(true);

        WeaponDamage weapon = _stateMachine.Knife.GetComponent<WeaponDamage>();

        if ( weapon == null )
        {
            weapon = _stateMachine.Knife.GetComponentInChildren<WeaponDamage>( true ); // the true is an overload because GetComponentInChildren ignores inactive objects 
        }

        weapon.SetAttack(_stateMachine.KnifeDamageAmount, _stateMachine.KnifeKnockback);
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);

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
        _stateMachine.InputReader.AimActivateEvent -= OnAim;
        _stateMachine.InputReader.VantagePointActivateEvent -= OnVantagePointActivate;
        _stateMachine.InputReader.TopDownActivateEvent -= OnTopDownActivate;
        _stateMachine.InputReader.SideScrollActivateEvent -= OnSideScrollActivate;
        _stateMachine.InputReader.ChaseCameraActivateEvent -= OnChaseCameraActive;
        _stateMachine.InputReader.JumpActivateEvent -= OnJump;
        _stateMachine.InputReader.MorphActivateEvent -= OnMorph;
        _stateMachine.InputReader.AttackEvent -= OnAttack;

        _stateMachine.Knife.SetActive(false);
    }

    void OnTarget()
    {
        if (!_stateMachine.Targeter.SelectTarget()) { return; }

        _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
    }

    void OnAim()
    {
        _stateMachine.SwitchState(new PlayerAimingState(_stateMachine));
    }

    void OnVantagePointActivate()
    {
        _stateMachine.SwitchState(new PlayerVantagePointState(_stateMachine));
    }

    void OnTopDownActivate()
    {
        _stateMachine.SwitchState(new PlayerTopDownState(_stateMachine));
    }

    void OnSideScrollActivate()
    {
        _stateMachine.SwitchState(new PlayerSideScrollState(_stateMachine));
    }

    void OnChaseCameraActive()
    {
        _stateMachine.SwitchState(new PlayerChaseCameraState(_stateMachine));
    }

    void OnJump()
    {
        _stateMachine.SwitchState(new PlayerPropKnifeJumpingState(_stateMachine));
    }

    void OnMorph()
    {
        _stateMachine.Knife.SetActive(false);
        _stateMachine.MorphVFX.Play();
        _stateMachine.StartCoroutine( MorphRoutine() ); // states are not mono behaviours, so we are borrowing this
    }

    IEnumerator MorphRoutine()
    {
        Morphable target = _stateMachine.CurrentMorphable;

        if (target == null)
            target = _stateMachine.LastMorphable;

        if (target == null)
            yield break;

        yield return new WaitForSeconds( _stateMachine.MorphDuration );

        switch (target.Type)
        {
            case Morphable.MorphableType.Knife:
                _stateMachine.SwitchState( new PlayerFreeLookState( _stateMachine ) );
                break;

            case Morphable.MorphableType.PropaneTank:
                _stateMachine.SwitchState( new PlayerPropPropaneState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Barrel:
                _stateMachine.SwitchState( new PlayerPropBarrelState( _stateMachine ) );
                break;
        }
    }

    void OnAttack()
    {
        _stateMachine.SwitchState( new PlayerPropKnifeAttackingState( _stateMachine ) );//TODO: Create Pre-Morph State
    }
}