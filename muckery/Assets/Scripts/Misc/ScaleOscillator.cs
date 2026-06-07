using UnityEngine;

public class ScaleOscillator : MonoBehaviour
{
    [SerializeField] float _minScale = 0.9f;
    [SerializeField] float _maxScale = 1.1f;
    [SerializeField] float _cycleTime = 1f;

    float _timer;

    
    public float MinScale  { get; set; }
    public float MaxScale  { get; set; }
    public float CycleTime  { get; set; }


    Vector3 _baseScale;

    void Awake()
    {
        _baseScale = transform.localScale;
        MinScale = _minScale;
        MaxScale = _maxScale;
        CycleTime = _cycleTime;
    }

    void Update()
    {
        if ( _cycleTime <= 0f )
            return;

        _timer += Time.deltaTime;

        float t = Mathf.PingPong( _timer / _cycleTime, 1f );
        float multiplier = Mathf.Lerp( _minScale, _maxScale, t );

        transform.localScale = _baseScale * multiplier;
    }
}