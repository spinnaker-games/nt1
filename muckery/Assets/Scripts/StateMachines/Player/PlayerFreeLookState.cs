using UnityEngine;

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
        _stateMachine.InputReader.AimActivateEvent += OnAim;
        _stateMachine.InputReader.VantagePointActivateEvent += OnVantagePointActivate;
        _stateMachine.InputReader.TopDownActivateEvent += OnTopDownActivate;
        _stateMachine.InputReader.SideScrollActivateEvent += OnSideScrollActivate;
        _stateMachine.InputReader.ChaseCameraActivateEvent += OnChaseCameraActive;
        _stateMachine.InputReader.JumpActivateEvent += OnJump;
        _stateMachine.InputReader.MorphActivateEvent += OnMorph;

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
    }

    Vector3 CalculateMovement()//TODO: Investigate adding this to base class
    {
        Vector3 forward = _stateMachine.MainCameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _stateMachine.MainCameraTransform.right;
        right.y = 0;
        right.Normalize();

        // Combine camera forward/right directions with input so movement is relative to the camera's facing direction
        return (forward * _stateMachine.InputReader.MovementValue.y) + (right * _stateMachine.InputReader.MovementValue.x);
    }

    void FaceMovementDirection(Vector3 movement, float deltaTime) //TODO: Investigate adding this to base class
    {
        _stateMachine.transform.rotation = Quaternion.Lerp(
            _stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * _stateMachine.RotationDamping);
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
        _stateMachine.SwitchState(new PlayerJumpingState(_stateMachine));
    }

    void OnMorph()
    {
        Interactable target = _stateMachine.CurrentInteractable;

        if (target == null)
            target = _stateMachine.LastInteractable;

        if (target == null)
            return;

        // switch based on cached interactable type
        switch (target.Type)
        {
            case Interactable.InteractableType.Knife:
                _stateMachine.SwitchState(new PlayerPropKnifeState(_stateMachine));
                break;

            case Interactable.InteractableType.PropaneTank:
                _stateMachine.SwitchState(new PlayerPropPropaneState(_stateMachine));
                break;

            case Interactable.InteractableType.BananaPeel:
                // whatever your banana state is
                break;
        }
    }
}