using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{

    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime(_stateMachine.Animator, "Attack");//TODO: Does this cause performance issues?

        if (normalizedTime < 1f)//TODO: investigate if _previousFrameTime check is even necessary
        {
            if (_stateMachine.InputReader.IsAttacking)//TODO: Investigate timed button press over holding the button.
            {
            }
        }
        else
        {
            if (_stateMachine.Targeter.CurrentTarget != null)
            {
                _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));//TODO: Add support for returning to other camera states by caching lastKnownCameraState
            }
        }
    }


    public override void Exit()
    {
    }
}