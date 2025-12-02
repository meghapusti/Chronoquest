using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI")]
    public GameObject inventoryPanel;   // Canvas_inventory
    public GameObject mysteryIcon;      // MysteryIcon

    [Header("Compass movement")]
    public RectTransform compassButton;      // Compass_Button (de la UI)
    public float compassMoveUpAmount = 120f; // cuánto sube cuando abre la mochila

    bool hasMysteryObject = false;
    bool inventoryOpen = false;
    Vector2 compassClosedPos;

    private void Awake()
    {
        Instance = this;

        // Guardamos la posición original de la brújula
        if (compassButton != null)
        {
            compassClosedPos = compassButton.anchoredPosition;
        }
        // FIX: Explicitly set the inventory state to closed at startup
    inventoryOpen = false; 
    if (inventoryPanel != null)
    {
        inventoryPanel.SetActive(false); // Make sure the UI object is hidden initially
    }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        // Cambiar estado abierto/cerrado
        inventoryOpen = !inventoryOpen;
        inventoryPanel.SetActive(inventoryOpen);

        // Mover la brújula si la hemos asignado
        if (compassButton != null)
        {
            if (inventoryOpen)
            {
                // Subir la brújula
                compassButton.anchoredPosition = compassClosedPos + new Vector2(0f, compassMoveUpAmount);
            }
            else
            {
                // Volver a la posición original
                compassButton.anchoredPosition = compassClosedPos;
            }
        }
    }

    public void AddMysteryObject()
    {
        if (hasMysteryObject) return;

        hasMysteryObject = true;

        if (mysteryIcon != null)
            mysteryIcon.SetActive(true);
    }
}
