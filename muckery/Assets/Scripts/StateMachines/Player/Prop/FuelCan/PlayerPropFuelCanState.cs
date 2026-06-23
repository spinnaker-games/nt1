using UnityEngine;

public class PlayerPropGasCanState : PlayerBaseState
{
    public PlayerPropGasCanState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.AbilityActivateEvent += OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;

        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.GasCan.SetActive(true);
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
        _stateMachine.InputReader.AbilityActivateEvent -= OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;

        _stateMachine.GasCan.SetActive(false);
    }

    void OnAbilityActivate()
    {
        if ( _stateMachine.OilPuddle == null )
            return;
        //TODO: Object pooling
        GameObject.Instantiate(
            _stateMachine.OilPuddle,
            _stateMachine.transform.position,
            Quaternion.identity
        );
    }

    void OnMorphSlot( int slot )
    {
        _stateMachine.SwitchState( new PlayerMorphingState( _stateMachine, slot + 1 ) );
    }

}