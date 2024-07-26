using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    // GameObjects
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject errorBox;
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject tileScreen;
    [SerializeField] private GameObject helpScreen;
    
    // Text objects
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI toolTipLabel;
    GameManager gm;

    private int rows = 15;
    private int cols = 32;

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
        resetTileGrid();
        gm.StartVisualiser();
    }

    public void selectionButtonHandler() {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        string[,] convertedArray = convertTilesTo2DArray();

        if (checkEmpty(convertedArray)) {
            Debug.Log("Empty!");

            errorText.text = "Add some walls. Only start and end points present!";
            errorBox.SetActive(true);
        }

        switch (buttonName) {
            case "DFSButton":
                break;

            case "BFSButton":
                break;

            case "A*Button":
                break;
        }
    }

    public void changeLabel(string label) {
        toolTipLabel.text = label;
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

    private void resetTileGrid() {
        Transform tileTransform = tileScreen.transform;
        int tileCount = tileTransform.childCount;

        for (int i = 0; i < tileCount; i++) {
            GameObject child = tileTransform.GetChild(i).gameObject;
            ButtonController buttonScript = child.GetComponent<ButtonController>();

            Image childImage = child.GetComponent<Image>();

            child.tag = "Unselected";
            buttonScript.changeThisLabel();

            childImage.color = Color.white;
        }

        GameObject startTile = tileTransform.GetChild(0).gameObject;
        GameObject endTile = tileTransform.GetChild(tileCount - 1).gameObject;

        startTile.tag = "Start";
        endTile.tag = "End";

        startTile.GetComponent<Image>().color = Color.green;
        endTile.GetComponent<Image>().color = Color.red;
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

    private string[,] convertTilesTo2DArray() {
        string[,] tilesArray = new string[cols, rows];
        int[] start = new int[2];

        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                Transform tile = tileScreen.transform.Find($"{i} {j}");

                string tileTag = tile.gameObject.tag;

                switch (tileTag) {
                    case "Unselected":
                        tilesArray[i, j] = ".";
                        break;
                    case "Selected":
                        tilesArray[i, j] = "x";
                        break;
                    case "Start":
                        tilesArray[i, j] = "*";

                        start[0] = i;
                        start[1] = j;
                        break;
                    case "End":
                        tilesArray[i, j] = "!";
                        break;
                }
            }
        }

        return tilesArray;
    }

    private bool checkEmpty(string[,] tileArray) {
        bool empty = true;

        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) { 
                string currTile = tileArray[i, j];
                if (tileArray[i, j] == "x") {
                    empty = false;
                }
            }
        }

        return empty;
    }
}

// . is path
// x is a wall
// * is the start
// ! is the end