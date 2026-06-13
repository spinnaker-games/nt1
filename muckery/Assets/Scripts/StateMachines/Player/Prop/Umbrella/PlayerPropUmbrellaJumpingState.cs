using UnityEngine;

public class PlayerPropUmbrellaJumpingState : PlayerBaseState
{
    readonly int JumpBeginAnimHash = Animator.StringToHash("UmbrellaJumpBegin");

    const float CrossFadeDuration = 0.2f;

    public PlayerPropUmbrellaJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.ForceReceiver.Jump(_stateMachine.UmbrellaJumpForce);

        _stateMachine.Animator.CrossFadeInFixedTime(JumpBeginAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;

        _stateMachine.Umbrella.SetActive(true);

        _stateMachine.UmbrellaJumpSFX.Play();
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);
        FaceMovementDirection(movement, deltaTime);

        if (_stateMachine.CharacterController.velocity.y <= 0)
        {
            _stateMachine.SwitchState(new PlayerPropUmbrellaFallingState(_stateMachine));
            return;
        }

         _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;

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