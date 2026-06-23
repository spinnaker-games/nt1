using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    readonly int AimingSpeedHash = Animator.StringToHash("AimingSpeed"); //integers are processed faster than strings.
    readonly int AimingBlendTreeHash = Animator.StringToHash("AimingBlendTree");

    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.ZoomCancelEvent += OnCancel;
        _stateMachine.InputReader.ZoomCancelEvent += OnAttack;

        Vector3 cameraForward = _stateMachine.MainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        _stateMachine.transform.forward = cameraForward;

        _stateMachine.Animator.Play(AimingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.TargetingMovementSpeed, deltaTime);
        //Rotate Camera relative to input
        //Rotate player relative to camera
    }

    public override void Exit()
    {
        _stateMachine.InputReader.ZoomCancelEvent -= OnCancel;
        _stateMachine.InputReader.ZoomCancelEvent -= OnAttack;
    }

    void OnCancel()
    {
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }

    void OnAttack()
    {
    }
}