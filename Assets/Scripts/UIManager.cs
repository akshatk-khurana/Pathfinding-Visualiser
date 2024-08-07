using UnityEngine;
using UnityEngine.EventSystems;
using System; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

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

    public void SelectionButtonHandler() {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        var resultsTuple = ConvertTilesToArray();
        string[,] convertedArray = resultsTuple.Item2;

        if (CheckEmpty(convertedArray)) {
            errorText.text = "Add some walls. Only start and end points present!";
            errorBox.SetActive(true);
        } else {
            string[,] solvedArray = new string[cols, rows];
            Tuple<int, int> startPos = resultsTuple.Item1;
            
            switch (buttonName) {
                case "DFSButton":
                    solvedArray = Algorithms.DepthFirstSearch(convertedArray, startPos);
                    break;

                case "BFSButton":
                    solvedArray = Algorithms.BreadthFirstSearch(convertedArray, startPos);
                    break;

                case "A*Button":
                    solvedArray = Algorithms.AStarSearch(convertedArray, startPos);
                    break;
            }

            DisplayResults(solvedArray);
        }
    }

    public void ChangeLabel(string label) {
        toolTipLabel.text = label;
    }

    public void setTile(GameObject tile, string tag, Color colour) {
        ButtonController buttonScript = tile.GetComponent<ButtonController>();
        Image image = tile.GetComponent<Image>();

        tile.tag = tag;
        buttonScript.ChangeThisLabel();
        image.color = colour;
    }

    public GameObject GetTileByTag(string tag) {
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

    public void ResetTileGrid() {
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

    public void ResetSolvedAndExplored() {
        Transform tileTransform = tileScreen.transform;
        int tileCount = tileTransform.childCount;

        for (int i = 0; i < tileCount; i++) {
            GameObject child = tileTransform.GetChild(i).gameObject;

            if (child.tag == "Solved" && child.tag == "Explored") {
                setTile(child, "Unselected", Color.white);
            }
        }
    }

    private void NameTiles() {
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
    public void DisplayResults(string[,] tilesArray) {
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {

                Transform tileTransform = tileScreen.transform.Find($"{i} {j}");
                GameObject tile = tileTransform.gameObject;

                switch (tilesArray[i, j]) {
                    case ".":
                        setTile(tile, "Unselected", Color.white);
                        break;
                    case ",":
                        setTile(tile, "Solved", Color.yellow);
                        break;
                    case "^":
                        setTile(tile, "Explored", Color.blue);
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
    private Tuple<Tuple<int, int>, string[,]> ConvertTilesToArray() {
        string[,] tilesArray = new string[cols, rows];
        Tuple<int, int> start = new Tuple<int, int>(0, 0);

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

    private bool CheckEmpty(string[,] tileArray) {
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
// , is solved
// ^ is explored