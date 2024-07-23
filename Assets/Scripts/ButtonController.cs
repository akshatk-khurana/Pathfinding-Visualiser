using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
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
    }
    
    // public void TileButtonHandler() {
    //     GameObject buttonObject = EventSystem.current.currentSelectedGameObject;
    //     buttonObject.GetComponent<Image>().color = Color.black;
    //     buttonObject.tag = "Selected";
    // }
}
