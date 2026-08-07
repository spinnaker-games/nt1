using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    readonly int JumpBeginAnimHash = Animator.StringToHash("JumpBegin");

    const float CrossFadeDuration = 0.2f;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.ForceReceiver.Jump(_stateMachine.JumpForce);

        _stateMachine.Animator.CrossFadeInFixedTime(JumpBeginAnimHash, CrossFadeDuration);

        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;

        _stateMachine.JumpSFX.Play();
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        Move(movement * _stateMachine.PlayerConfig.FreeLookMovementSpeed, deltaTime);
        FaceMovementDirection(movement, deltaTime);


        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;

        if (_stateMachine.CharacterController.velocity.y <= 0)
        {
            _stateMachine.SwitchState(new PlayerFallingState(_stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
        _stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        _stateMachine.SwitchState(new PlayerHangingState(_stateMachine, ledgeForward, closestPoint));
    }
}