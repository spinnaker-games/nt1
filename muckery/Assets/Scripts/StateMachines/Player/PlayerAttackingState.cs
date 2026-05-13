using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    bool _forceAlreadyApplied = false;

    Attack _attack;

    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        _attack = _stateMachine.Attacks[attackIndex];
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
            if (normalizedTime > _attack.ForceTime)
            {
                TryApplyForce();
            }

            if (_stateMachine.InputReader.IsAttacking)//TODO: Investigate timed button press over holding the button.
            {
                TryComboAttack(normalizedTime);
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

    void TryComboAttack(float normalizedTime)
    {
        if (_attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < _attack.ComboAttackTime) { return; }

        _stateMachine.SwitchState( new PlayerAttackState(_stateMachine, _attack.ComboStateIndex));
    }

    
    void TryApplyForce()
    {
        if (_forceAlreadyApplied) { return; }

        _stateMachine.ForceReceiver.AddForce(_stateMachine.transform.forward * _attack.Force);

        _forceAlreadyApplied = true;
    }
}