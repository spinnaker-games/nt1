using UnityEngine;

public class CameraOrientedSprite : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Cache the main camera for performance
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // Make the sprite face the camera
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
        }
    }
}