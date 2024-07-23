using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{   
    [SerializeField] private GameObject toolTipPrefab;
    private GameObject toolTip;
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

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (toolTip == null) {
            var buttonPos = this.transform.position;
            toolTip = Instantiate(toolTipPrefab, Vector2(this.transform.position))
        } else {
            toolTip.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
            toolTip.SetActive(false);
    }
}
