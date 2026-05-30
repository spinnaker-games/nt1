using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    readonly int BlockAnimHash = Animator.StringToHash("Block");

    const float CrossFadeDuration = 0.2f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Health.SetInvulnerable(true);//TODO: Find a more complex blocking solution. This currently blocks from all directions
        _stateMachine.Animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime); //although our feet are planted, we still want forces applied for knockback and gravity

        if (!_stateMachine.InputReader.IsBlocking)
        {
            _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            return;
        }
        if (_stateMachine.Targeter.CurrentTarget == null)
        {
            _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        _stateMachine.Health.SetInvulnerable(true);
    }
}