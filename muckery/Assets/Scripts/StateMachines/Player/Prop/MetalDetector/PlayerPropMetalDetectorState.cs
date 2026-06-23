using UnityEngine;
using System.Collections;

public class PlayerPropMetalDetectorState : PlayerBaseState
{
    public PlayerPropMetalDetectorState(PlayerStateMachine stateMachine, bool shouldFadeAnim = true) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnTarget;
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;
        //_stateMachine.InputReader.AbilityActivateEvent += OnAbilityActivate;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.MetalDetector.SetActive(true);

        WeaponDamage weapon = _stateMachine.MetalDetector.GetComponent<WeaponDamage>();

        if ( weapon == null )
        {
            weapon = _stateMachine.MetalDetector.GetComponentInChildren<WeaponDamage>( true ); // the true is an overload because GetComponentInChildren ignores inactive objects 
        }
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputReader.TargetEvent -= OnTarget;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;
        _stateMachine.InputReader.AbilityActivateEvent -= OnAbilityActivate;

        _stateMachine.MetalDetector.SetActive(false);
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
    }
}