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
        _stateMachine.InputReader.TopDownCancelEvent -= OnChaseCameraCancel;
    }

    void OnChaseCameraCancel()
    {
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }

    Vector3 CalculateMovement()
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
}