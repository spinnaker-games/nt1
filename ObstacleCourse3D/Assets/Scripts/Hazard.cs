using UnityEditor;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hazard Hit");
        }
    }
}