using UnityEngine;
using UnityEngine.InputSystem;

public class MaskCycler : MonoBehaviour
{
    [Header("Masks")]
    [SerializeField] GameObject[] _masks;

    InputActions _inputActions;
    int _currentMaskIndex = -1; // -1 = no mask

    void Awake()
    {
        _inputActions = new InputActions();
        DisableAllMasks();
    }

    void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Next.performed += OnNextPerformed;
        _inputActions.Player.Previous.performed += OnPreviousPerformed;
    }

    void OnDisable()
    {
        _inputActions.Player.Next.performed -= OnNextPerformed;
        _inputActions.Player.Previous.performed -= OnPreviousPerformed;
        _inputActions.Player.Disable();
    }

    void OnNextPerformed(InputAction.CallbackContext context)
    {
        CycleMask(1);
    }

    void OnPreviousPerformed(InputAction.CallbackContext context)
    {
        CycleMask(-1);
    }

    void CycleMask(int direction)
    {
        int maskCount = _masks.Length;

        if (maskCount == 0)
            return;

        // Deactivate current mask if there is one
        if (_currentMaskIndex >= 0)
            _masks[_currentMaskIndex].SetActive(false);

        // Calculate new index
        _currentMaskIndex += direction;

        // Wrap around including -1 for no mask
        if (_currentMaskIndex > maskCount - 1)
            _currentMaskIndex = -1;
        else if (_currentMaskIndex < -1)
            _currentMaskIndex = maskCount - 1;

        // Activate new mask if not -1
        if (_currentMaskIndex >= 0)
            _masks[_currentMaskIndex].SetActive(true);
    }

    void DisableAllMasks()
    {
        foreach (var mask in _masks)
            mask.SetActive(false);
    }

    public void SetMaskIndex(int index)
    {
        // Optional helper to set mask directly
        if (_currentMaskIndex >= 0)
            _masks[_currentMaskIndex].SetActive(false);

        _currentMaskIndex = index;

        if (_currentMaskIndex >= 0 && _currentMaskIndex < _masks.Length)
            _masks[_currentMaskIndex].SetActive(true);
    }
}