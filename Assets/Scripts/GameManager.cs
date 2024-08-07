using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {   
    public static GameManager Instance;
    
    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public bool pointerDragging = false;
    public UnityEvent onStart = new UnityEvent();

    public void StartVisualiser() {
        onStart.Invoke();
    }
}
