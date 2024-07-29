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
            errorText.text = "Add some walls. Only start and end points present!";
            errorBox.SetActive(true);
        } else {
            string[,] solvedArray;
            int[] startPos = new int[2];
            int[] endPos = new int[2];

            switch (buttonName) {
                case "DFSButton":
                    solvedArray = Algorithms.depthFirstSearch(convertedArray, startPos, endPos);
                    break;

                case "BFSButton":
                    solvedArray = Algorithms.breadthFirstSearch(convertedArray, startPos, endPos);
                    break;

                case "A*Button":
                    solvedArray = Algorithms.aStarSearch(convertedArray, startPos, endPos);
                    break;
            }
        }
    }

    public void changeLabel(string label) {
        toolTipLabel.text = label;
    }

    public void setTile(GameObject tile, string tag, Color colour) {
        ButtonController buttonScript = tile.GetComponent<ButtonController>();
        Image image = tile.GetComponent<Image>();

        tile.tag = tag;
        buttonScript.changeThisLabel();
        image.color = colour;
    }

    public GameObject getTileByTag(string tag) {
        GameObject chosenTile = tileScreen;

        for (int i = 0; i < tileScreen.transform.childCount; i++) {
            GameObject child = tileScreen.transform.GetChild(i).gameObject;

            if (child.tag == tag) {
                chosenTile = child;
                break;
            }
        }

        return chosenTile;
    }

    public void resetTileGrid() {
        Transform tileTransform = tileScreen.transform;
        int tileCount = tileTransform.childCount;

        for (int i = 0; i < tileCount; i++) {
            GameObject child = tileTransform.GetChild(i).gameObject;

            if (i == 0) {
                setTile(child, "Start", Color.green);
            } else if (i == tileCount - 1) {
                setTile(child, "End", Color.red);
            } else {
                setTile(child, "Unselected", Color.white);
            }
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

    public void displayResults(string[,] tilesArray) {
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                Transform tileTransform = tileScreen.transform.Find($"{i} {j}");
                GameObject tile = tileTransform.gameObject;
                
                switch (tilesArray[i, j]) {
                    case ".":
                        setTile(tile, "Path", Color.white);
                        break;
                    case "x":
                        setTile(tile, "Wall", Color.black);
                        break;
                    case "*":
                        setTile(tile, "Start", Color.green);
                        break;
                    case "!":
                        setTile(tile, "End", Color.red);
                        break;
                }
            }
        }
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
                if (currTile == "x") {
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