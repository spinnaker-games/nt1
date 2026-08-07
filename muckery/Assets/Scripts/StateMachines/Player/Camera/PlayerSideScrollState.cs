using UnityEngine;

public class PlayerSideScrollState : PlayerBaseState
{
    readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); //integers are processed faster than strings.
    readonly int SideScrollBlendTreeHash = Animator.StringToHash("SideScrollBlendTree");
    const float AnimatorDampTime = 0.075f;
    public PlayerSideScrollState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.SideScrollCancelEvent += OnSideScrollCancel;

        Vector3 cameraForward = _stateMachine.MainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        _stateMachine.transform.forward = cameraForward;

        _stateMachine.Animator.Play(SideScrollBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackState(_stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.PlayerConfig.FreeLookMovementSpeed, deltaTime);

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
        _stateMachine.InputReader.SideScrollCancelEvent -= OnSideScrollCancel;
    }

    void OnSideScrollCancel()
    {
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }
}