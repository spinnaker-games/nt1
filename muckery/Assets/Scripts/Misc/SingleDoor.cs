using UnityEngine;

public class SingleDoor : MonoBehaviour
{
    [SerializeField] GameObject _doorButton;
    [SerializeField] Animator _animator;
    [SerializeField] InputReader _inputReader;

    readonly int DoorOpenInwardHash = Animator.StringToHash( "SingleDoorOpenInward" );
    readonly int DoorOpenOutwardHash = Animator.StringToHash( "SingleDoorOpenOutward" );
    readonly int DoorCloseHash = Animator.StringToHash( "SingleDoorClose" );

    bool _playerInRange;
    bool _isOpen;


    void OnTriggerEnter( Collider other )
    {
        if ( !other.CompareTag( "Player" ) ) return;

        _playerInRange = true;

        if ( _doorButton != null )
        {
            _doorButton.SetActive( true );
        }

        _inputReader.InteractActivateEvent += OnInteract;
    }

    void OnTriggerExit( Collider other )
    {
        if ( !other.CompareTag( "Player" ) ) return;

        _playerInRange = false;

        if ( _doorButton != null )
        {
            _doorButton.SetActive( false );
        }

        _inputReader.InteractActivateEvent -= OnInteract;
    }

    void OnInteract()
    {
        if ( !_playerInRange ) return;

        _isOpen = !_isOpen;

        if ( _isOpen )
        {
            _animator.Play( DoorOpenInwardHash );
        }
        else
        {
            _animator.Play( DoorCloseHash );
        }
    }
}