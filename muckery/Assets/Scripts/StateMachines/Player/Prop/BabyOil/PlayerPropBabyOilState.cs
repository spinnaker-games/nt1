using UnityEngine;

public class PlayerPropBabyOilState : PlayerBaseState
{
    public PlayerPropBabyOilState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.InputReader.AbilityActivateEvent += OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent += OnMorphSlot;
        _stateMachine.PlayerModel.SetActive(false);
        _stateMachine.BabyOil.SetActive(true);
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
        _stateMachine.InputReader.AbilityActivateEvent -= OnAbilityActivate;
        _stateMachine.InputReader.MorphSlotActivateEvent -= OnMorphSlot;
        _stateMachine.BabyOil.SetActive(false);
    }

    void OnAbilityActivate()
    {
        if ( _stateMachine.BabyOilPuddle == null )
            return;
        //TODO: Object pooling
        GameObject.Instantiate(
            _stateMachine.BabyOilPuddle,
            _stateMachine.transform.position,
            Quaternion.identity
        );
    }

    void OnMorphSlot( int slot )
    {
        _stateMachine.SwitchState( new PlayerMorphingState( _stateMachine, slot + 1 ) );
    }
}