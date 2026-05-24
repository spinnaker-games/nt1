using UnityEngine;

public class PlayerPropBarrelJumpingState : PlayerBaseState
{
    readonly int JumpBeginAnimHash = Animator.StringToHash("JumpBegin");

    const float CrossFadeDuration = 0.2f;

    public PlayerPropBarrelJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.ForceReceiver.Jump(_stateMachine.JumpForce);

        _stateMachine.Animator.CrossFadeInFixedTime(JumpBeginAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;

        _stateMachine.Barrel.SetActive(true);

        _stateMachine.JumpSFX.Play();
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        //Call Move Twice: One for jumping and one for directional motion
        //Move(_momentum, deltaTime);
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);
        FaceMovementDirection(movement, deltaTime);

        if (_stateMachine.CharacterController.velocity.y <= 0)
        {
            _stateMachine.SwitchState(new PlayerPropBarrelFallingState(_stateMachine));
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