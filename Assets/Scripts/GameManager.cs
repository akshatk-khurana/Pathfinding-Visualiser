using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    public static string searchMode = "BFS";
    private void Awake() {
        if (Instance == null) Instance = this;
    }
    public UnityEvent onStart = new UnityEvent();

    public void StartVisualiser() {
        onStart.Invoke();
    }
}
