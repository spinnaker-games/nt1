using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HatManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _hats = new();
    [SerializeField] int _selectedHatIndex;

#if UNITY_EDITOR
    void OnValidate()
    {
        EditorApplication.delayCall += DelayedValidate;
    }

    void DelayedValidate()
    {
        if ( this == null )
        {
            return;
        }

        UpdateHatVisibility();
    }
#endif

    void Awake()
    {
        UpdateHatVisibility();
    }

    public void SelectHat( int hatIndex )
    {
        _selectedHatIndex = hatIndex;
        UpdateHatVisibility();
    }

    void UpdateHatVisibility()
    {
        for ( int i = 0; i < _hats.Count; i++ )
        {
            if ( _hats[ i ] == null )
            {
                continue;
            }

            _hats[ i ].SetActive( i == _selectedHatIndex );
        }
    }
}