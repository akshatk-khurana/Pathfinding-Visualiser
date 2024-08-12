using UnityEngine;
using UnityEngine.EventSystems;
using System; 
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private GameObject errorBox;
    [SerializeField] private GameObject selectionScreen;
    [SerializeField] private GameObject tileScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private TextMeshProUGUI toolTipLabel;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI algorithmText;

    GameManager gm;
    public int rows = 15;
    public int cols = 32;

    private void Awake() {
        if (Instance == null) Instance = this;
    }
    void Start() {
        gm = GameManager.Instance;
        gm.onStart.AddListener(DeactivateStartMenu);
        NameTiles();
    }
    public void DeactivateStartMenu() {
        startScreen.SetActive(false);
        selectionScreen.SetActive(true);
        tileScreen.SetActive(true);
    }
    public void StartButtonHandler() {
        ResetTileGrid();
        gm.StartVisualiser();
    }
    public void SelectionButtonHandler() {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        var resultsTuple = ConvertTilesToArray();
        string[,] convertedArray = resultsTuple.Item2;
        string[,] original = ConvertTilesToArray().Item2;

        if (CheckEmpty(convertedArray)) {
            errorText.text = "Add some walls. Only start and end points present!";
            errorBox.SetActive(true);
        } else {
            string[,] solvedArray = new string[cols, rows];

            Tuple<string[,], Tuple<int, float>> pathResults = null;

            Tuple<Tuple<int, int>, Tuple<int, int>> positions = resultsTuple.Item1;
            Tuple<int, int> startPos = positions.Item1;
            Tuple<int, int> endPos = positions.Item2;
            
            switch (buttonName) {
                case "DFS":
                    pathResults = Algorithms.DepthFirstSearch(convertedArray, startPos);
                    break;

                case "BFS":
                    pathResults = Algorithms.BreadthFirstSearch(convertedArray, startPos);
                    break;

                case "A*":
                    pathResults = Algorithms.AStarSearch(convertedArray, startPos, endPos);
                    break;
            }

            solvedArray = pathResults.Item1;
            
            if (CheckEqual(solvedArray, original)) {
                Debug.Log("No solution!");

                errorText.text = "No solution found!";
                errorBox.SetActive(true);
            } else {
                int exploredCount = pathResults.Item2.Item1;
                float timeTaken = pathResults.Item2.Item2;

                countText.text = exploredCount.ToString();
                timeText.text = timeTaken.ToString();

                resultScreen.SetActive(true);
                DisplayResults(solvedArray);
            }
        }
    }
    public void ChangeLabel(string label) {
        toolTipLabel.text = label;
    }
    public void SetTile(GameObject tile, string tag, Color colour) {
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
            Color color = Color.white;
            string tag = "Unselected";
            
            if (i == 0) {
                tag = "Start";
                color = Color.green;
            }
            
            if (i == tileCount - 1) {
                tag = "End";
                color = Color.red;
            }

            SetTile(child, tag, color);
        }
    }
    public void ResetSolvedAndExplored() {
        Transform tileTransform = tileScreen.transform;
        int tileCount = tileTransform.childCount;

        for (int i = 0; i < tileCount; i++) {
            GameObject child = tileTransform.GetChild(i).gameObject;

            if (child.tag == "Solved") {
                SetTile(child, "Unselected", Color.white);
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

                Color color = Color.white;
                string tag = "Unselected";

                switch (tilesArray[i, j]) {
                    case ",":
                        tag = "Solved";
                        color = Color.yellow;
                        break;
                    case "x":
                        tag = "Selected";
                        color = Color.black;
                        break;
                    case "*":
                        tag = "Start";
                        color = Color.green;
                        break;
                    case "!":
                        tag = "End";
                        color = Color.red;
                        break;
                }

                SetTile(tile, tag, color);
            }
        }
    }
    private Tuple<Tuple<Tuple<int, int>, Tuple<int, int>>, string[,]> ConvertTilesToArray() {
        string[,] tilesArray = new string[cols, rows];
        Tuple<int, int> start = new Tuple<int, int>(0, 0);
        Tuple<int, int> end = new Tuple<int, int>(31, 14);

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
                        end = new Tuple<int, int>(i, j);
                        break;
                }
            }
        }

        Tuple<Tuple<int, int>, Tuple<int, int>> positions = new Tuple<Tuple<int, int>, Tuple<int, int>>(start, end);
        Tuple<
            Tuple<
                Tuple<int, int>, 
                Tuple<int, int>
            >,
            string[,]
        > returnInfo = new Tuple<
                               Tuple<
                                   Tuple<int, int>, 
                                   Tuple<int, int>
                                   >, 
                                   string[,]
                            >(positions, tilesArray);
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
    private bool CheckEqual(string[,] array1, string[,] array2) {
        int rows = array1.GetLength(0);
        int cols = array1.GetLength(1);

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (array1[i, j] != array2[i, j]) {
                    return false;
                }
            }
        }
        return true;
    }
}

// . is path
// x is a wall
// * is the start
// ! is the end
// , is solved