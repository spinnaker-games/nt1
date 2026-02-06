using UnityEngine;

public class ChickenHop : MonoBehaviour
{
    [Header("Hop Settings")]
    [SerializeField] float _minHeight = 0.1f;
    [SerializeField] float _maxHeight = 0.4f;

    [SerializeField] float _minDuration = 0.4f;
    [SerializeField] float _maxDuration = 1.2f;

    Vector3 _startPos;
    float _timer;
    float _duration;
    float _height;

    void Awake()
    {
        _startPos = transform.localPosition;
        PickNewHop();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        float t = _timer / _duration;
        if ( t >= 1f )
        {
            PickNewHop();
            t = 0f;
        }

        // Smooth hop curve
        float yOffset = Mathf.Sin( t * Mathf.PI ) * _height;

        Vector3 pos = _startPos;
        pos.y += yOffset;
        transform.localPosition = pos;
    }

    void PickNewHop()
    {
        _timer = 0f;
        _duration = Random.Range( _minDuration, _maxDuration );
        _height = Random.Range( _minHeight, _maxHeight );
    }
}