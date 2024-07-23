using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{   
    private string label = "Path";
    public void OnPointerClick(PointerEventData pointerEventData) {
        string currentTag = this.tag;
        Image image = this.GetComponent<Image>();

        if (currentTag == "Unselected") {
            gameObject.tag = "Selected";
            image.color = Color.black;
        } else {
            gameObject.tag = "Unselected";
            image.color = Color.white;
        }

        label = label == "Unselected" ? "Path" : "Wall"; 
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        
    }
}
