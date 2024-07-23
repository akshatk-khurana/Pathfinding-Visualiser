using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData) {
        this.tag = "Selected";
    }
    
    // public void TileButtonHandler() {
    //     GameObject buttonObject = EventSystem.current.currentSelectedGameObject;
    //     buttonObject.GetComponent<Image>().color = Color.black;
    //     buttonObject.tag = "Selected";
    // }
}
