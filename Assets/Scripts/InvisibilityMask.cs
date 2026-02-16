using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InvisibilityMask : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material invisibilityMaterial;

    private Material[][] originalMaterials;
    private bool _isInvisible = false;

    // Event: listeners get the current invisibility state
    public static event Action<bool> OnInvisibilityMaskToggled;

    private void Awake()
    {
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
            originalMaterials[i] = renderers[i].materials;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ToggleInvisibility();
        }
    }

    private void ToggleInvisibility()
    {
        _isInvisible = !_isInvisible;
        Debug.Log("Invisibility toggled: " + _isInvisible);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (_isInvisible)
            {
                Material[] invisMats = new Material[renderers[i].materials.Length];
                for (int j = 0; j < invisMats.Length; j++)
                    invisMats[j] = invisibilityMaterial;
                renderers[i].materials = invisMats;
            }
            else
            {
                renderers[i].materials = originalMaterials[i];
            }
        }

        // Notify subscribers
        OnInvisibilityMaskToggled?.Invoke(_isInvisible);
    }
}