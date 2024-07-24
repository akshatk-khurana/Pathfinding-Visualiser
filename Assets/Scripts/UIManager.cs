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

    [SerializeField] public GameObject toolTipBox;
    [SerializeField] private TextMeshProUGUI toolTipLabel;
    GameManager gm;

    private void Awake() {
        if (Instance == null) Instance = this;
    }
    
    void Start() {
        gm = GameManager.Instance;
        gm.onStart.AddListener(DeactivateStartMenu);
        toolTipBox.SetActive(false);
    }

    public void DeactivateStartMenu() {
        startScreen.SetActive(false);
        selectionScreen.SetActive(true);
        tileScreen.SetActive(true);
    }

    public void ChangeToolTipPos(float x, float y, string label) {
        var toolTipPos = toolTipBox.transform.position;
        float z = toolTipPos.z;

        toolTipLabel.text = label;
        toolTipBox.transform.position = new Vector3(x + 0.25f, y + 0.25f, z);
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
