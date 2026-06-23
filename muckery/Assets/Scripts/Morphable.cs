#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Morphable : MonoBehaviour
{
    public enum MorphableType
    {
        Knife,
        Spring,
        Barrel,
        MetalDetector,
        SniperRifle,
        Binoculars,
        Umbrella,
        ScanCamera,
        LockPick,
        GasCan,
        BabyOil
    }

    [SerializeField] InputReader _inputReader;
    [SerializeField] MorphableType _morphableType;

    public MorphableType Type => _morphableType;

    PlayerStateMachine player;

    [Header("Visuals")]
    [SerializeField] GameObject _knife;
    [SerializeField] GameObject _spring;
    [SerializeField] GameObject _barrel;
    [SerializeField] GameObject _metalDetector;
    [SerializeField] GameObject _sniperRifel;
    [SerializeField] GameObject _binoculars;
    [SerializeField] GameObject _umbrella;
    [SerializeField] GameObject _scanCamera;
    [SerializeField] GameObject _lockPick;
    [SerializeField] GameObject _gasCan;
    [SerializeField] GameObject _babyOil;

    [Header("UI")]
    [SerializeField] GameObject _morphButton;

    void Awake()
    {
        UpdateVisuals();
    }

    void OnTriggerEnter( Collider other )
    {
        if ( !other.CompareTag( "Player" ) )
            return;

        player = other.GetComponent<PlayerStateMachine>();

        _inputReader.MorphSlotActivateEvent += OnMorphSlot;

        if ( _morphButton != null )
            _morphButton.SetActive( true );
    }

    void OnTriggerExit( Collider other )
    {
        if ( !other.CompareTag( "Player" ) )
            return;

        _inputReader.MorphSlotActivateEvent -= OnMorphSlot;

        if ( _morphButton != null )
            _morphButton.SetActive( false );

        player = null;
    }

    void OnMorphSlot( int slot )
    {
        if ( player == null )
            return;

        switch ( slot )
        {
            case 0:
                player.MorphableSlotA = this;
                Debug.Log( $"Assigned {Type} to Slot A" );
                break;

            case 1:
                player.MorphableSlotB = this;
                Debug.Log( $"Assigned {Type} to Slot B" );
                break;

            case 2:
                player.MorphableSlotC = this;
                Debug.Log( $"Assigned {Type} to Slot C" );
                break;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        EditorApplication.delayCall += DelayedValidate;
    }

    void DelayedValidate()
    {
        if ( this == null )
            return;

        UpdateVisuals();
    }
#endif

    void UpdateVisuals()
    {
        if ( _morphButton != null )
            _morphButton.SetActive( false );

        if ( _knife != null ) _knife.SetActive( _morphableType == MorphableType.Knife );
        if ( _spring != null ) _spring.SetActive( _morphableType == MorphableType.Spring );
        if ( _barrel != null ) _barrel.SetActive( _morphableType == MorphableType.Barrel );
        if ( _metalDetector != null ) _metalDetector.SetActive( _morphableType == MorphableType.MetalDetector );
        if ( _sniperRifel != null ) _sniperRifel.SetActive( _morphableType == MorphableType.SniperRifle );
        if ( _binoculars != null ) _binoculars.SetActive( _morphableType == MorphableType.Binoculars );
        if ( _umbrella != null ) _umbrella.SetActive( _morphableType == MorphableType.Umbrella );
        if ( _scanCamera != null ) _scanCamera.SetActive( _morphableType == MorphableType.ScanCamera );
        if ( _lockPick != null ) _lockPick.SetActive( _morphableType == MorphableType.LockPick );
        if ( _gasCan != null ) _gasCan.SetActive( _morphableType == MorphableType.GasCan );
        if ( _babyOil != null ) _babyOil.SetActive( _morphableType == MorphableType.BabyOil );
    }
}