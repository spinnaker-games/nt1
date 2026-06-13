using UnityEngine;

public class PlayerPropUmbrellaFallingState : PlayerBaseState
{
    readonly int JumpEndAnimHash = Animator.StringToHash("UmbrellaJumpEnd");

    const float CrossFadeDuration = 0.2f;

    const float _umbrellaGravityScale = 0.1f;
    const float UmbrellaSpinSpeed = 90f;

    float _defaultGravityScale;

    public PlayerPropUmbrellaFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _defaultGravityScale = _stateMachine.ForceReceiver.GravityScale;
        _stateMachine.ForceReceiver.GravityScale = _umbrellaGravityScale;

        _stateMachine.Animator.CrossFadeInFixedTime(JumpEndAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);
        //FaceMovementDirection(movement, deltaTime);

        SpinUmbrella( deltaTime );


        if (_stateMachine.CharacterController.isGrounded)
        {
            if (_stateMachine.Targeter.CurrentTarget != null)
            {
                //_stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerPropUmbrellaState(_stateMachine));
            }
        }

        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;

        FaceTarget();
    }

    public override void Exit()
    {
        _stateMachine.ForceReceiver.GravityScale = _defaultGravityScale;
        _stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        //_stateMachine.SwitchState(new PlayerHangingState(_stateMachine, ledgeForward, closestPoint));
    }

    void SpinUmbrella( float deltaTime )
    {
        _stateMachine.Umbrella.transform.Rotate(
            Vector3.forward,
            UmbrellaSpinSpeed * deltaTime,
            Space.Self );
    }
}