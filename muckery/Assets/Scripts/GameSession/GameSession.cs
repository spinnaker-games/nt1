using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public bool TargetEliminated { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void MarkTargetEliminated()
    {
        TargetEliminated = true;
    }
}