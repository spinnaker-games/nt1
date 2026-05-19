using UnityEngine;

public class PlayerPropBarrelPeelFallingState : PlayerBaseState
{
    readonly int JumpEndAnimHash = Animator.StringToHash("JumpEnd");

    Vector3 _momentum; //TODO: better name????

    const float CrossFadeDuration = 0.2f;

    public PlayerPropBarrelPeelFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _momentum = _stateMachine.CharacterController.velocity;
        _momentum.y = 0;

        _stateMachine.Animator.CrossFadeInFixedTime(JumpEndAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(_momentum, deltaTime);

        if (_stateMachine.CharacterController.isGrounded)
        {
            if (_stateMachine.Targeter.CurrentTarget != null)
            {
                //_stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerPropBarrelPeelState(_stateMachine));//TODO: Add support for returning to other camera states by caching lastKnownCameraState
            }
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