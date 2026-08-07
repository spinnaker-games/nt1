using UnityEngine;

public class PlayerChaseCameraState : PlayerBaseState
{
    readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed"); //integers are processed faster than strings.
    readonly int ChaseBlendTreeHash = Animator.StringToHash("ChaseBlendTree");
    const float AnimatorDampTime = 0.075f;

    public PlayerChaseCameraState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.TopDownCancelEvent += OnChaseCameraCancel;

        Vector3 cameraForward = _stateMachine.MainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        _stateMachine.transform.forward = cameraForward;

        _stateMachine.Animator.Play(ChaseBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
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
        _stateMachine.InputReader.TopDownCancelEvent -= OnChaseCameraCancel;
    }

    void OnChaseCameraCancel()
    {
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }
}