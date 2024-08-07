using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, 
                                IPointerClickHandler, 
                                IPointerEnterHandler, 
                                IPointerExitHandler,
                                IDragHandler,
                                IEndDragHandler,
                                IBeginDragHandler {   
    private string label = "Path";
    UIManager um;
    GameManager gm;

    private void Start() {
        um = UIManager.Instance;
        gm = GameManager.Instance;
        ChangeThisLabel();
    }
    public void ChangeThisLabel() {
        string currentTag = this.tag;

        switch (currentTag) {
            case "Unselected":
                label = "Path";
                break;
            case "Selected":
                label = "Wall";
                break;
            default:
                label = currentTag;
                break;
        }
    }
    private void OnPointOrDrag() {
        string currentTag = this.tag;
        Image image = this.GetComponent<Image>();
        GameObject selectedObj;

        if (Input.GetKey("s")) {
            selectedObj = um.GetTileByTag("Start");
            um.SetTile(selectedObj, "Unselected", Color.white);

            this.tag = "Start";
            image.color = Color.green;

        } else if (Input.GetKey("e")) {
            selectedObj = um.GetTileByTag("End");
            um.SetTile(selectedObj, "Unselected", Color.white);
            
            this.tag = "End";
            image.color = Color.red;

        } else {
            if (currentTag == "Unselected") {
                gameObject.tag = "Selected";
                image.color = Color.black;
            } else if (currentTag == "Selected") {
                gameObject.tag = "Unselected";
                image.color = Color.white;
            }
        }

        ChangeThisLabel();
        um.ChangeLabel(label);
    }
    public void OnPointerClick(PointerEventData pointerEventData) {
        OnPointOrDrag();
    }
    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (gm.pointerDragging) {
            OnPointOrDrag();
        }
        um.ChangeLabel(label);
    }
    public void OnPointerExit(PointerEventData pointerEventData) {
        um.ChangeLabel("None");
    }
    public void OnBeginDrag(PointerEventData data) {
        gm.pointerDragging = true;
        OnPointOrDrag();
    }
    public void OnDrag(PointerEventData data) {

    }
    public void OnEndDrag(PointerEventData data) {
        gm.pointerDragging = false;
    }
}