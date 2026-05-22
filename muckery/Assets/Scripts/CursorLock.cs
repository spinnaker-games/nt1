using UnityEngine;

public class CursorLock : MonoBehaviour
{
    [SerializeField] bool _lockCursor = true;

    void Start()
    {
        ApplyCursorState();
    }

    void Update()
    {
        ApplyCursorState();
    }

    void ApplyCursorState()
    {
        if ( _lockCursor )
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetLock( bool value )
    {
        _lockCursor = value;
        ApplyCursorState();
    }
}