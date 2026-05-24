using UnityEngine;

public class PlayerPropSpringJumpingState : PlayerBaseState
{
    readonly int JumpBeginAnimHash = Animator.StringToHash("SpringJumpBegin");

    const float CrossFadeDuration = 0.2f;

    public PlayerPropSpringJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.ForceReceiver.Jump(_stateMachine.JumpForce);

        _stateMachine.Animator.CrossFadeInFixedTime(JumpBeginAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;

        _stateMachine.Spring.SetActive(true);

        _stateMachine.JumpSFX.Play();
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);
        FaceMovementDirection(movement, deltaTime);

        if (_stateMachine.CharacterController.velocity.y <= 0)
        {
            _stateMachine.SwitchState(new PlayerPropSpringFallingState(_stateMachine));
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
        //_stateMachine.SwitchState(new PlayerHangingState(_stateMachine, ledgeForward, closestPoint));
    }
}