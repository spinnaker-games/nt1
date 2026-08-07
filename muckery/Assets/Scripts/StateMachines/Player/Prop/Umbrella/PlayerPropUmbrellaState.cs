using UnityEngine;
using System.Collections;

public class PlayerPropUmbrellaState : PlayerBaseState
{

    public PlayerPropUmbrellaState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnTarget;
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;
        _stateMachine.InputReader.AbilityActivateEvent += OnAbilityActivate;


        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.Umbrella.SetActive(true);
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.PlayerConfig.FreeLookMovementSpeed, deltaTime);

        _stateMachine.IsMoving = _stateMachine.InputReader.MovementValue != Vector2.zero;

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputReader.TargetEvent -= OnTarget;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;
        _stateMachine.InputReader.AbilityActivateEvent -= OnAbilityActivate;

        _stateMachine.Umbrella.SetActive(false);
    }

    void OnTarget()
    {
        if (!_stateMachine.Targeter.SelectTarget()) { return; }

        _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
    }

    void OnMorphSlot( int slot )
    {
        _stateMachine.SwitchState( new PlayerMorphingState( _stateMachine, slot + 1 ) );
    }

    void OnAbilityActivate()
    {
        _stateMachine.SwitchState( new PlayerPropUmbrellaJumpingState( _stateMachine ) );
    }
}