using UnityEngine;

public class InventoryButtonHelper : MonoBehaviour
{
    // A public function that the Button component can easily find.
    public void OnMysteryIconClicked()
    {
        if (InventoryManager.Instance != null)
        {
            // *** FIX: Use the correct method name defined in InventoryManager ***
            InventoryManager.Instance.UseMysteryObject();
        }
        else
        {
            Debug.LogError("Cannot find InventoryManager instance to use the item!");
        }
    }
}