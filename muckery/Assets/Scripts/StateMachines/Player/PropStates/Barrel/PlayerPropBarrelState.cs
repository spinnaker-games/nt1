using UnityEngine;

public class PlayerPropBarrelPeelState : PlayerBaseState
{
    bool _shouldFadeAnim;
    readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    readonly int FreeLookZoomBlendTreeHash = Animator.StringToHash("FreeLookZoomBlendTree");
    const float AnimatorDampTime = 0.075f;
    const float CrossFadeDuration = 0.2f;

    public PlayerPropBarrelPeelState(PlayerStateMachine stateMachine, bool shouldFadeAnim = true) : base(stateMachine)
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

        if (_shouldFadeAnim)
        {
            _stateMachine.Animator.CrossFadeInFixedTime(FreeLookZoomBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            _stateMachine.Animator.Play(FreeLookZoomBlendTreeHash);
        }

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.Barrel.SetActive(true);

        _stateMachine.IsDisguised = true;//TODO: EXPOSE
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);

        
        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;
        //Debug.Log("Player Movement = " + _stateMachine.IsMoving + "Disguise " + _stateMachine.IsDisguised);

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

        _stateMachine.Barrel.SetActive(false);
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
        _stateMachine.SwitchState(new PlayerPropBarrelPeelJumpingState(_stateMachine));
    }

    void OnMorph()
    {
        _stateMachine.SwitchState( new PlayerFreeLookState( _stateMachine ) );//TODO: Create Pre-Morph State
    }
}