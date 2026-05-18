using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetExit : MonoBehaviour
{
    [SerializeField] string _gameOverSceneName;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Target")) return;

        Debug.Log("TARGET HAS ESCAPED! MISSION FAILED");
        LoadSceneByName();
    }

    void LoadSceneByName()
    {
        if ( string.IsNullOrWhiteSpace( _gameOverSceneName ) )
        {
            Debug.LogWarning( "Scene name is empty." );
            return;
        }

        SceneManager.LoadScene( _gameOverSceneName );
    }
}