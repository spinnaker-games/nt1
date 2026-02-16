using UnityEngine;
using UnityEngine.InputSystem;

public class InvisibilityMask : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material invisibilityMaterial;

    private Material[][] originalMaterials;
    private bool isInvisible = false;

    private void Awake()
    {
        // Save original materials
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    private void Update()
    {
        // Check if "2" key was pressed this frame using new Input System
        if (Keyboard.current != null && Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ToggleInvisibility();
        }
    }

    private void ToggleInvisibility()
    {
        isInvisible = !isInvisible;
        Debug.Log("Invisibility toggled: " + isInvisible);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (isInvisible)
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
    }
}