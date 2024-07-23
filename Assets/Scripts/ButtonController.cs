using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{   
    [SerializeField] private GameObject toolTipPrefab;
    private GameObject toolTip;
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
        Debug.Log(this.name);
        var buttonPos = this.transform.position;
        Debug.Log($"x: {buttonPos.x} y: {buttonPos.y}");
        toolTip = Instantiate(toolTipPrefab, new Vector2(buttonPos.x, buttonPos.y), Quaternion.identity);
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        Destroy(toolTip);
    }
}
