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

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = _stateMachine.MainCameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _stateMachine.MainCameraTransform.right;
        right.y = 0;
        right.Normalize();

        // Combine camera forward/right directions with input so movement is relative to the camera's facing direction
        return (forward * _stateMachine.InputReader.MovementValue.y) + (right * _stateMachine.InputReader.MovementValue.x);
    }

    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        if (movement.sqrMagnitude < 0.0001f) return; //TODO: Understand why the viewing vector is zero log spam was occuring 

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        _stateMachine.transform.rotation = Quaternion.Lerp(
            _stateMachine.transform.rotation,
            targetRotation,
            deltaTime * _stateMachine.RotationDamping);
    }

    protected void Move(Vector3 movement, float deltaTime) //TODO: Possibly rename this to something else. View PlayerDeathState for ambiguity problem
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