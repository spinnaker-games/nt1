using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InvisibilityMask : MonoBehaviour
{
    [SerializeField] Renderer[] _renderers;
    [SerializeField] Material _invisibilityMaterial;

    Material[][] _originalMaterials;
    bool _isInvisible = false;

    InputActions _inputActions;

    // Event: listeners get the current invisibility state
    public static event Action<bool> OnInvisibilityMaskToggled;

    void Awake()
    {
        _inputActions = new InputActions();

        _originalMaterials = new Material[_renderers.Length][];
        for (int i = 0; i < _renderers.Length; i++)
            _originalMaterials[i] = _renderers[i].materials;
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.UseMask.performed += OnUseMaskPerformed;
    }

    void OnDisable()
    {
        _inputActions.Player.UseMask.performed -= OnUseMaskPerformed;
        _inputActions.Player.Disable();
    }

    void OnUseMaskPerformed(InputAction.CallbackContext context)
    {
        ToggleInvisibility();
    }

    void ToggleInvisibility()
    {
        _isInvisible = !_isInvisible;
        Debug.Log("Invisibility toggled: " + _isInvisible);

        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_isInvisible)
            {
                Material[] invisMats = new Material[_renderers[i].materials.Length];
                for (int j = 0; j < invisMats.Length; j++)
                    invisMats[j] = _invisibilityMaterial;
                _renderers[i].materials = invisMats;
            }
            else
            {
                _renderers[i].materials = _originalMaterials[i];
            }
        }

        // Notify subscribers
        OnInvisibilityMaskToggled?.Invoke(_isInvisible);
    }
}