using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerExit : MonoBehaviour
{
    [SerializeField] string _victorySceneName;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!GameSession.Instance.TargetEliminated)
        {
            Debug.Log("CANNOT EXTRACT! TARGET IS STILL ALIVE!");
            return;
        }

        LoadSceneByName();
    }

    void LoadSceneByName()
    {
        if ( string.IsNullOrWhiteSpace( _victorySceneName ) )
        {
            Debug.LogWarning( "Scene name is empty." );
            return;
        }

        SceneManager.LoadScene( _victorySceneName );
    }
}