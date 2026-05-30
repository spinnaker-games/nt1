using UnityEngine;

public class PlayerVantagePointState : PlayerBaseState
{
    readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); //integers are processed faster than strings.
    readonly int VantagePointBlendTreeHash = Animator.StringToHash("VantagePointBlendTree");
    const float AnimatorDampTime = 0.075f;
    public PlayerVantagePointState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.VantagePointCancelEvent += OnVantagePointCancel;

        Vector3 cameraForward = _stateMachine.MainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        _stateMachine.transform.forward = cameraForward;

        _stateMachine.Animator.Play(VantagePointBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackState(_stateMachine, 0));
            return;
        }

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
        _stateMachine.InputReader.VantagePointCancelEvent -= OnVantagePointCancel;
    }

    void OnVantagePointCancel()
    {
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }
}