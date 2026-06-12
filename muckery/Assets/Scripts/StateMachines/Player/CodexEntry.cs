using UnityEngine;

public class CodexEntry : MonoBehaviour
{
    [SerializeField] string _codexDescription;

    public string GetCodexEntry()
    {
        return _codexDescription;
    }
}
