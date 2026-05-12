using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine _stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        _stateMachine.CharacterController.Move((movement + _stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (_stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = _stateMachine.Targeter.CurrentTarget.transform.position - _stateMachine.transform.position;
        lookPos.y = 0;

        _stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void ReturnToLocomotion()
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