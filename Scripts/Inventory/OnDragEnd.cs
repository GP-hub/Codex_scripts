using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnDragEnd : MonoBehaviour, IDropHandler
{
    private SlotScript nextSlot;

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop : " + transform.position);
    }
}
