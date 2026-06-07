using UnityEngine;
using System.Collections.Generic;

public class MetalDetector : MonoBehaviour
{
    [SerializeField] GameObject MetalDetectorCompass;

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
            _metalsInRange.Add( other.transform.root );
            MetalDetectorCompass.SetActive( true );
        }
    }

    void OnTriggerExit( Collider other )
    {
        if ( other.CompareTag( "Metal" ) )
        {
            _metalsInRange.Remove( other.transform.root );

            if ( _metalsInRange.Count == 0 )
            {
                MetalDetectorCompass.SetActive( false );
                _currentTarget = null;
            }
        }
    }

    void UpdateClosestMetal()
    {
        Transform detector = MetalDetectorCompass.transform;

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

        Vector3 lookPos = _currentTarget.position - MetalDetectorCompass.transform.position;
        lookPos.y = 0f;

        if ( lookPos.sqrMagnitude < 0.0001f ) { return; }

        Quaternion baseRotation = Quaternion.LookRotation( lookPos );
        Quaternion xOffset = Quaternion.Euler( 90f, 0f, 0f );

        MetalDetectorCompass.transform.rotation = baseRotation * xOffset;
    }
}