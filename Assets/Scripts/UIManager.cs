using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject resultScreen;
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
        nameTiles();
    }

    public void DeactivateStartMenu() {
        startScreen.SetActive(false);
        selectionScreen.SetActive(true);
        tileScreen.SetActive(true);
    }

    public void StartButtonHandler() {
        gm.StartVisualiser();
    }

    public void resetTile(string tag) {
        for (int i = 0; i < tileScreen.transform.childCount; i++) {
            GameObject child = tileScreen.transform.GetChild(i).gameObject;
            ButtonController buttonScript = child.GetComponent<ButtonController>();
            Image image = child.GetComponent<Image>();

            if (child.tag == tag) {
                child.tag = "Unselected";
                buttonScript.changeThisLabel();
                
                image.color = Color.white;
                break;
            }
        }
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

    private void clearTileGrid() {
        for (int i = 0; i < tileScreen.transform.childCount; i++) {
            GameObject child = tileScreen.transform.GetChild(i).gameObject;
            ButtonController buttonScript = child.GetComponent<ButtonController>();

            Image childImage = child.GetComponent<Image>();

            child.tag = "Unselected";
            buttonScript.changeThisLabel();

            childImage.color = Color.white;
        }
    }

    private void nameTiles() {
        int x = 0;
        int y = 0;

        for (int i = 0; i < tileScreen.transform.childCount; i++) {
            GameObject child = tileScreen.transform.GetChild(i).gameObject;
            child.name = $"{x} {y}";

            if (x == 31) {
                x = 0;
                if (y == 14) {
                    break;
                } else {
                    y++;
                }
            } else {
                x++;
            }
        }
    }

    public void displayResults() {
        // to display results on the display screen
    }
}
