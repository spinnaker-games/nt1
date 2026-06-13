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
        Morphable target = _stateMachine.MorphableSlot;

        if ( target == null )
            yield break;

        yield return new WaitForSeconds( _stateMachine.MorphDuration );

        if ( _stateMachine.Health.IsDead )
            yield break;

        if ( _stateMachine.IsMorphed )
        {
            if ( _stateMachine.CurrentMorphable == target )
            {
                _stateMachine.IsMorphed = false;
                _stateMachine.CurrentMorphable = null;
                _stateMachine.SwitchState( new PlayerFreeLookState( _stateMachine ) );
                yield break;
            }
        }

        _stateMachine.IsMorphed = true; //one call for all morphables
        _stateMachine.CurrentMorphable = target;

        switch (target.Type) //TODO: replace switch with lookup table
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

            case Morphable.MorphableType.MetalDetector:
                _stateMachine.SwitchState( new PlayerPropMetalDetectorState( _stateMachine ) );
                break;

            case Morphable.MorphableType.SniperRifle:
                _stateMachine.SwitchState( new PlayerPropSniperRifleState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Binoculars:
                _stateMachine.SwitchState( new PlayerPropBinocularsState( _stateMachine ) );
                break;

            case Morphable.MorphableType.Umbrella:
                _stateMachine.SwitchState( new PlayerPropUmbrellaState( _stateMachine ) );
                break;

            case Morphable.MorphableType.ScanCamera:
                _stateMachine.SwitchState( new PlayerPropScanCameraState( _stateMachine ) );
                break;

            case Morphable.MorphableType.LockPick:
                _stateMachine.SwitchState( new PlayerPropLockPickState( _stateMachine ) );
                break;
        }
    }
}