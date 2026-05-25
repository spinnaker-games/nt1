using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] string _sceneName;

    public void LoadSceneByName()
    {
        if ( string.IsNullOrWhiteSpace( _sceneName ) )
        {
            Debug.LogWarning( "Scene name is empty." );
            return;
        }

        SceneManager.LoadScene( _sceneName );
    }
}