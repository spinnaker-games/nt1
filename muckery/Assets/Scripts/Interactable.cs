#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Knife,
        PropaneTank,
        BananaPeel
    }

    [SerializeField] InteractableType _interactableType;

    public InteractableType Type => _interactableType;

    [Header( "Visuals" )]
    [SerializeField] GameObject _knife;
    [SerializeField] GameObject _propaneTank;
    [SerializeField] GameObject _bananaPeel;

    [Header( "UI" )]
    [SerializeField] GameObject _interactButton;

    void Awake()
    {
        UpdateVisuals();
    }

    void OnTriggerEnter(Collider other)
    {
        if ( _interactButton != null ) _interactButton.SetActive(true);

        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();

        if (player != null)
        {
            player.CurrentInteractable = this;
            player.LastInteractable = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ( _interactButton != null ) _interactButton.SetActive(false);

        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();

        if (player != null && player.CurrentInteractable == this)
        {
            player.CurrentInteractable = null;
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
        if ( _interactButton != null ) _interactButton.SetActive(false);

        if ( _knife != null ) _knife.SetActive( _interactableType == InteractableType.Knife );

        if ( _propaneTank != null ) _propaneTank.SetActive( _interactableType == InteractableType.PropaneTank );

        if ( _bananaPeel != null ) _bananaPeel.SetActive( _interactableType == InteractableType.BananaPeel );
    }
}