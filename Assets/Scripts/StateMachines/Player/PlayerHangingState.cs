using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    Vector3 _ledgeForward;
    Vector3 _closestPoint;

    readonly int HangingAnimHash = Animator.StringToHash("Hanging");

    const float CrossFadeDuration = 0.2f;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this._ledgeForward = ledgeForward;
        this._closestPoint = closestPoint;
    }

    public override void Enter()
    {
        _stateMachine.transform.rotation = Quaternion.LookRotation(_ledgeForward, Vector3.up);//makes player face ledge


        _stateMachine.CharacterController.enabled = false;
        _stateMachine.transform.position = _closestPoint - (_stateMachine.LedgeDetector.transform.position - _stateMachine.transform.position); //accounts for the fact that hands are in a different position
        _stateMachine.CharacterController.enabled = true;

        _stateMachine.Animator.CrossFadeInFixedTime(HangingAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.MovementValue.y > 0f)//if player uses forward input
        {
            _stateMachine.SwitchState(new PlayerHangClimbState(_stateMachine));            
        }
        else if (_stateMachine.InputReader.MovementValue.y < 0f)//if player uses backward input
        {
            _stateMachine.CharacterController.Move(Vector3.zero);
            _stateMachine.ForceReceiver.Reset();
            _stateMachine.SwitchState(new PlayerFallingState(_stateMachine));            
        }
    }

    public override void Exit()
    {
    }
}