using UnityEngine;

public class StopMotionEffect : MonoBehaviour
{
    public Animator _animator;
    public float _stepFPS = 12f;

    float _step;
    float _accumulatedTime;
    float _normalizedTime;

    void Start()
    {
        _step = 1f / _stepFPS;
        _normalizedTime = 0f;

        _animator.speed = 0f; // freeze Unity interpolation
    }

    void Update()
    {
        _accumulatedTime += Time.deltaTime;

        if (_accumulatedTime >= _step)
        {
            _accumulatedTime -= _step;

            _normalizedTime += _step / GetCurrentClipLength();
            _normalizedTime %= 1f; // loop safely

            _animator.Play(0, 0, _normalizedTime);
            _animator.Update(0f); // force pose refresh
        }
    }

    float GetCurrentClipLength()
    {
        var clips = _animator.GetCurrentAnimatorClipInfo(0);
        return clips.Length > 0 ? clips[0].clip.length : 1f;
    }
}