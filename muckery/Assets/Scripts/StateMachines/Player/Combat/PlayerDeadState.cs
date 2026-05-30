using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeadState : PlayerBaseState
{
    readonly int DeathAnimHash = Animator.StringToHash( "Death" );

    const float CrossFadeDuration = 0.2f;

    bool _isAnimationFinished;
    bool _hasStartedCoroutine;

    public PlayerDeadState( PlayerStateMachine stateMachine ) : base( stateMachine )
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime( DeathAnimHash, CrossFadeDuration );
        _isAnimationFinished = false;
        _hasStartedCoroutine = false;

        _stateMachine.Knife.SetActive(false);
        _stateMachine.Barrel.SetActive(false);
        _stateMachine.Spring.SetActive(false);
        _stateMachine.PlayerModel.SetActive(true);

        _stateMachine.DeathSFX.Play();
    }

    public override void Tick( float deltaTime )
    {
        Move( deltaTime );

        if ( !_isAnimationFinished )
        {
            var stateInfo = _stateMachine.Animator.GetCurrentAnimatorStateInfo( 0 );

            if ( stateInfo.shortNameHash == DeathAnimHash &&
                 stateInfo.normalizedTime >= 1f )
            {
                _isAnimationFinished = true;
            }
        }

        if ( _isAnimationFinished && !_hasStartedCoroutine )
        {
            _hasStartedCoroutine = true;
            _stateMachine.StartCoroutine( DeathRoutine() );
        }
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds( _stateMachine.PostDeathDelay );

        SceneManager.LoadScene(_stateMachine.GameOverSceneName); //TODO: Tight Coupling??? + Hash string
    }

    public override void Exit()
    {
    }
}