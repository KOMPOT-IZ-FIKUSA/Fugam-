using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on UI element: " + gameObject.name);

        // Perform actions for this UI element
    }
}