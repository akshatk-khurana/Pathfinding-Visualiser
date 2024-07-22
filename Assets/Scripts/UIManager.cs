using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject selectionScreen;
    GameManager gm;
    void Start()
    {
        gm = GameManager.Instance;
        gm.onStart.AddListener(DeactivateStartMenu);
    }

    public void DeactivateStartMenu() {
        startScreen.SetActive(false);
        selectionScreen.SetActive(true);
    }

    public void StartButtonHandler() {
        gm.StartVisualiser();
    }
}
