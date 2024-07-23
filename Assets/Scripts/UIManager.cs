using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject tileScreen;
    [SerializeField] private GameObject helpScreen;
    GameManager gm;
    void Start() {
        gm = GameManager.Instance;
        gm.onStart.AddListener(DeactivateStartMenu);
    }

    public void DeactivateStartMenu() {
        startScreen.SetActive(false);
        selectionScreen.SetActive(true);
        tileScreen.SetActive(true);
    }

    public void StartButtonHandler() {
        gm.StartVisualiser();
    }

    public void selectionButtonHandler() {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        switch (buttonName) {
            case "DFSButton":
                gm.SetMode("DFS");
                break;

            case "BFSButton":
                gm.SetMode("BFS");
                break;

            case "A*Button":
                gm.SetMode("A*");
                break;
        }
    }
}
