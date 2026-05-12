using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<Target> OnDestroyedEvent;

    void OnDestroy()
    {
        OnDestroyedEvent?.Invoke(this);
    }
}