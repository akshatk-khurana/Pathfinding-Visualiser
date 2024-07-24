using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject tileScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private TextMeshProUGUI toolTipLabel;
    GameManager gm;

    private void Awake() {
        if (Instance == null) Instance = this;
    }
    
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
    public void changeLabel(string label) {
        toolTipLabel.text = label;
    }
}
