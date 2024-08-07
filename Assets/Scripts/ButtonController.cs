using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, 
                                IPointerClickHandler, 
                                IPointerEnterHandler, 
                                IPointerExitHandler, 
                                IBeginDragHandler {   
    private string label = "Path";
    UIManager um;

    private void Start() {
        um = UIManager.Instance;
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

        if (Input.GetKey("s")) {
            GameObject selectedObj = um.getTileByTag("Start");
            um.setTile(selectedObj, "Unselected", Color.white);

            this.tag = "Start";
            image.color = Color.green;

        } else if (Input.GetKey("e")) {
            GameObject selectedObj = um.getTileByTag("End");
            um.setTile(selectedObj, "Unselected", Color.white);
            
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
        um.ChangeLabel(label);
    }
    public void OnPointerExit(PointerEventData pointerEventData) {
        um.ChangeLabel("None");
    }
    public void OnBeginDrag(PointerEventData pointerEventData) {
        OnPointOrDrag();
    }
}