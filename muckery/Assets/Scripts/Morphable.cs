#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Morphable : MonoBehaviour //TODO: create IMorphable interface
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
        ScanCamera
    }

    [SerializeField] MorphableType _morphableType;

    public MorphableType Type => _morphableType;

    [Header( "Visuals" )]
    [SerializeField] GameObject _knife;
    [SerializeField] GameObject _spring;
    [SerializeField] GameObject _barrel;
    [SerializeField] GameObject _metalDetector;
    [SerializeField] GameObject _sniperRifel;
    [SerializeField] GameObject _binoculars;
    [SerializeField] GameObject _umbrella;
    [SerializeField] GameObject _scanCamera;

    [Header( "UI" )]
    [SerializeField] GameObject _morphButton;

    void Awake()
    {
        UpdateVisuals();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if ( _morphButton != null ) _morphButton.SetActive(true);

        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();

        if (player != null)
        {
            player.CurrentMorphable = this;
            player.LastMorphable = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if ( _morphButton != null ) _morphButton.SetActive(false);

        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();

        if (player != null && player.CurrentMorphable == this)
        {
            player.CurrentMorphable = null;
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
        if ( _morphButton != null ) _morphButton.SetActive(false);

        if ( _knife != null ) _knife.SetActive( _morphableType == MorphableType.Knife );

        if ( _spring != null ) _spring.SetActive( _morphableType == MorphableType.Spring );

        if ( _barrel != null ) _barrel.SetActive( _morphableType == MorphableType.Barrel );

        if ( _barrel != null ) _metalDetector.SetActive( _morphableType == MorphableType.MetalDetector );

        if ( _barrel != null ) _sniperRifel.SetActive( _morphableType == MorphableType.SniperRifle );

        if ( _binoculars != null ) _binoculars.SetActive( _morphableType == MorphableType.Binoculars );

        if ( _umbrella != null ) _umbrella.SetActive( _morphableType == MorphableType.Umbrella );

        if ( _scanCamera != null ) _scanCamera.SetActive( _morphableType == MorphableType.ScanCamera );
    }
}