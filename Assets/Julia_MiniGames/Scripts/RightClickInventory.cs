using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickInventory : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventoryManager.Instance.ToggleInventory();
        }
    }
}
