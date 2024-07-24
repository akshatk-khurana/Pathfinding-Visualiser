using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{   
    private string label = "Path";
    UIManager um;

    private void Start() {
        um = UIManager.Instance;
        changeThisLabel();
    }

    public void changeThisLabel() {
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

    public void OnPointerClick(PointerEventData pointerEventData) {
        string currentTag = this.tag;
        Image image = this.GetComponent<Image>();

        if (Input.GetKey("s")) {
            
        } else if (Input.GetKey("e")) {
            
        } else {
            if (currentTag == "Unselected") {
                gameObject.tag = "Selected";
                image.color = Color.black;
            } else {
                gameObject.tag = "Unselected";
                image.color = Color.white;
            }
        }

        changeThisLabel();
        um.changeLabel(label);
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        um.changeLabel(label);
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        um.changeLabel("None");
    }
}
