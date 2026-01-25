using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float _xAngle = 0f;
    [SerializeField] float _yAngle = 1f;
    [SerializeField] float _zAngle = 0f;
    void Update()
    {
        transform.Rotate(_xAngle, _yAngle, _zAngle);
    }
}