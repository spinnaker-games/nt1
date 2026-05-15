using UnityEngine;

public class PlayerPropPropaneJumpingState : PlayerBaseState
{
    readonly int JumpBeginAnimHash = Animator.StringToHash("JumpBegin");

    Vector3 _momentum; //TODO: better name????

    const float CrossFadeDuration = 0.2f;

    public PlayerPropPropaneJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.ForceReceiver.Jump(_stateMachine.JumpForce);

        _momentum = _stateMachine.CharacterController.velocity;
        _momentum.y = 0;

        _stateMachine.Animator.CrossFadeInFixedTime(JumpBeginAnimHash, CrossFadeDuration);

        _stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;

        _stateMachine.PropaneTank.SetActive(true);
    }

    public override void Tick(float deltaTime)
    {
        Move(_momentum, deltaTime);

        if (_stateMachine.CharacterController.velocity.y <= 0)
        {
            _stateMachine.SwitchState(new PlayerPropKnifeFallingState(_stateMachine));
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