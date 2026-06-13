using UnityEngine;

public class PlayerPropLockPickState : PlayerBaseState
{

    public PlayerPropLockPickState(PlayerStateMachine stateMachine, bool shouldFadeAnim = true) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.MorphActivateEvent += OnMorph;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.LockPick.SetActive(true);
        _stateMachine.IsLockPick = true;
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);

        
        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputReader.MorphActivateEvent -= OnMorph;

        _stateMachine.LockPick.SetActive(false);
        _stateMachine.IsLockPick = false;
    }

    void OnMorph()
    {
        _stateMachine.SwitchState(new PlayerMorphingState(_stateMachine));
    }

}