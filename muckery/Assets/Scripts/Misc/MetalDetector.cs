using UnityEngine;
using System.Collections.Generic;

public class MetalDetector : MonoBehaviour
{
    [SerializeField] GameObject _metalDetectorCompass;

    readonly HashSet<Transform> _metalsInRange = new();

    Transform _currentTarget;

    void Update()
    {
        UpdateClosestMetal();
        FaceMetal();
    }

    void OnTriggerEnter( Collider other )
    {
        if ( other.CompareTag( "Metal" ) )
        {
            _metalsInRange.Add( other.transform.root ); //TODO: This setup breaks when metal objects are grouped
            _metalDetectorCompass.SetActive( true );
        }
    }

    void OnTriggerExit( Collider other )
    {
        if ( other.CompareTag( "Metal" ) )
        {
            _metalsInRange.Remove( other.transform.root );

            if ( _metalsInRange.Count == 0 )
            {
                _metalDetectorCompass.SetActive( false );
                _currentTarget = null;
            }
        }
    }

    void UpdateClosestMetal()
    {
        Transform detector = _metalDetectorCompass.transform;

        float bestDist = float.MaxValue;
        Transform closest = null;

        foreach ( var metal in _metalsInRange )
        {
            if ( metal == null ) { continue; }

            float dist = ( metal.position - detector.position ).sqrMagnitude;

            if ( dist < bestDist )
            {
                bestDist = dist;
                closest = metal;
            }
        }

        _currentTarget = closest;
    }

    void FaceMetal()
    {
        if ( _currentTarget == null ) { return; }

        Vector3 lookPos = _currentTarget.position - _metalDetectorCompass.transform.position;
        lookPos.y = 0f;

        if ( lookPos.sqrMagnitude < 0.0001f ) { return; }

        Quaternion baseRotation = Quaternion.LookRotation( lookPos );
        Quaternion xOffset = Quaternion.Euler( 90f, 0f, 0f );

        _metalDetectorCompass.transform.rotation = baseRotation * xOffset;
    }
}