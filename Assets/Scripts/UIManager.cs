using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject errorBox;
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject tileScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI toolTipLabel;
    GameManager gm;

    public int rows = 15;
    public int cols = 32;

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

        var resultsTuple = convertTilesTo2DArray();
        string[,] convertedArray = resultsTuple.Item2;

        if (checkEmpty(convertedArray)) {
            errorText.text = "Add some walls. Only start and end points present!";
            errorBox.SetActive(true);
        } else {
            string[,] solvedArray = new string[rows, cols];
            Tuple<int, int> startPos = resultsTuple.Item1;
            Debug.Log($"Starting position: {startPos.Item1} {startPos.Item2}");
            solvedArray = Algorithms.breadthFirstSearch(convertedArray, startPos);
            
            // switch (buttonName) {
            //     case "DFSButton":
            //         solvedArray = Algorithms.depthFirstSearch(convertedArray, startPos);
            //         break;

            //     case "BFSButton":
            //         solvedArray = Algorithms.breadthFirstSearch(convertedArray, startPos);
            //         break;

            //     case "A*Button":
            //         solvedArray = Algorithms.aStarSearch(convertedArray, startPos);
            //         break;
            // }

            displayResults(solvedArray);
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
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                Transform tileTransform = tileScreen.transform.Find($"{j} {i}");
                GameObject tile = tileTransform.gameObject;
                
                switch (tilesArray[i, j]) {
                    case ".":
                        setTile(tile, "Unselected", Color.white);
                        break;
                    case ",":
                        setTile(tile, "Solved", Color.yellow);
                        break;
                    case "x":
                        setTile(tile, "Selected", Color.black);
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

    private Tuple<Tuple<int, int>, string[,]> convertTilesTo2DArray() {
        string[,] tilesArray = new string[rows, cols];
        Tuple<int, int> start = new Tuple<int, int>(0, 0);

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                Transform tile = tileScreen.transform.Find($"{j} {i}");

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
                        start = new Tuple<int, int>(i, j);
                        break;
                    case "End":
                        tilesArray[i, j] = "!";
                        break;
                }
            }
        }

        Tuple<Tuple<int, int>, string[,]> returnInfo = new Tuple<Tuple<int, int>, string[,]>(start, tilesArray);
        return returnInfo;
    }

    private bool checkEmpty(string[,] tileArray) {
        bool empty = true;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) { 
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
// , is solved