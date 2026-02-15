using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SeerMaskHiddenItem : MonoBehaviour
{
    MeshRenderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false; // start hidden
    }

    void OnEnable()
    {
        SeerMask.OnMaskToggled += HandleMaskToggle;
    }

    void OnDisable()
    {
        SeerMask.OnMaskToggled -= HandleMaskToggle;
    }

    void HandleMaskToggle(bool isActive)
    {
        _meshRenderer.enabled = isActive;
    }
}