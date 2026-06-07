using UnityEngine;

public class LoopScale : MonoBehaviour
{
    [SerializeField] float _minScale = 0.9f;
    [SerializeField] float _maxScale = 1.1f;
    [SerializeField] float _time = 1f;

    float _timer;

    void Update()
    {
        if ( _time <= 0f )
            return;

        _timer += Time.deltaTime;

        float t = Mathf.PingPong( _timer / _time, 1f );
        float scale = Mathf.Lerp( _minScale, _maxScale, t );

        transform.localScale = Vector3.one * scale;
    }
}