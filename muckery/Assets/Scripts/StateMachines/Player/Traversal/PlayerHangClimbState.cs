using UnityEngine;

public class PlayerHangClimbState : PlayerBaseState
{
    readonly int HangClimbAnimHash = Animator.StringToHash("HangClimb");

    readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);//TODO: find better name

    const float CrossFadeDuration = 0.2f;

    public PlayerHangClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(HangClimbAnimHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedAnimTime(_stateMachine.Animator, "Climbing") < 1) { return; }


        _stateMachine.CharacterController.enabled = false;//TODO: Investigate more elegant way to translate player character controller
        _stateMachine.transform.Translate(Offset, Space.Self); //TODO: Magic numbers
        _stateMachine.CharacterController.enabled = true;

        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine, false));
    }

    public override void Exit()
    {
        _stateMachine.CharacterController.Move(Vector3.zero);
        _stateMachine.ForceReceiver.Reset();
    }
}