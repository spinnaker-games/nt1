using UnityEngine;
using System.Collections;

public class PlayerMorphingState : PlayerBaseState
{

    public PlayerMorphingState(PlayerStateMachine stateMachine, bool shouldFadeAnim = true) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.MorphVFX.Play();
        _stateMachine.MorphSFX.Play();
        _stateMachine.StartCoroutine( MorphRoutine() );
    }

    public override void Tick(float deltaTime)
    {    
        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime); //TODO: Do we want movement while morphing?

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
    }

    IEnumerator MorphRoutine()
    {
        Morphable target = _stateMachine.CurrentMorphable;

        if (target == null)
            target = _stateMachine.LastMorphable;

        if (target == null)
            yield break;

        yield return new WaitForSeconds( _stateMachine.MorphDuration );

        if (_stateMachine.Health.IsDead)
        {
            //_stateMachine.SwitchState(new PlayerDeadState(_stateMachine));
            yield break;
        }

        if (_stateMachine.IsMorphed && _stateMachine.CurrentMorphable == null)
        {
            _stateMachine.SwitchState( new PlayerFreeLookState( _stateMachine ) );
            yield break;
        }

        switch (target.Type)
        {
            case Morphable.MorphableType.Knife:
                _stateMachine.SwitchState( new PlayerPropKnifeState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Spring:
                _stateMachine.SwitchState( new PlayerPropSpringState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Barrel:
                _stateMachine.SwitchState( new PlayerPropBarrelState( _stateMachine ) );
                break;
        }
    }
}