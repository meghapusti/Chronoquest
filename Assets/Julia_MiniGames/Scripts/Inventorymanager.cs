using UnityEngine;
using UnityEngine.UI; // Required for the Button component

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // We keep the original fields, relying on code to populate them based on names.
    [Header("UI (Auto-Populated at Runtime)")]
    [SerializeField] private GameObject inventoryPanel; // This will hold the Canvas_Inventory object
    [SerializeField] private GameObject mysteryIcon;    // This will hold the artifact icon slot inside the panel
    
    [Header("Compass movement")]
    [SerializeField] private RectTransform compassButton; // This will hold the RectTransform of the visible compass icon
    [SerializeField] private float compassMoveUpAmount = 120f; 

    public bool hasMysteryObject = false; // Persistent state
    private bool inventoryOpen = false;
    private Vector2 compassClosedPos;
    private Sprite currentMysterySprite; // ✅ NEW: store icon for current object


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // --- STEP 1: Find the UI objects by name (based on your working structure) ---
        
        // 1a. Find the root inventory canvas object: "Canvas_Inventory"
        if (inventoryPanel == null)
        {
            inventoryPanel = GameObject.Find("Canvas_Inventory"); 
            
            if (inventoryPanel != null)
            {
                // Ensure the entire Canvas root object persists
                DontDestroyOnLoad(inventoryPanel.transform.root.gameObject); 
                inventoryPanel.SetActive(false); // Hide the panel immediately
                Debug.Log("Inventory Canvas found and set to DontDestroyOnLoad.");
            }
            else
            {
                Debug.LogError("FATAL ERROR: Could not find 'Canvas_Inventory' in the scene.");
            }
        }
        // 1b. CRITICAL FIX: Find the persistent visible compass button's RectTransform: "MysteryIcon"
        if (compassButton == null)
        {
            // Find the object named "MysteryIcon" globally. We search for the GameObject first, then check components.
            GameObject compassObject = GameObject.Find("MysteryIcon");
            
            if (compassObject != null)
            {
                compassButton = compassObject.GetComponent<RectTransform>();
            }
            // If it's still null, try finding it again in case it's a child of the DontDestroyOnLoad root we can access. (This is often unnecessary but adds safety)
            if (compassButton == null && compassObject == null) 
            {
                Debug.LogError("FATAL ERROR: Could not find GameObject named 'MysteryIcon' globally.");
            }
            else if (compassButton != null)
            {
                 Debug.Log("Visible Compass Button RectTransform found.");
            }
            else
            {
                Debug.LogError("FATAL ERROR: Found 'MysteryIcon' object but it is missing the RectTransform component.");
            }
        }

        // 1c. Find the artifact icon *inside* the inventory panel: "Panel/MysteryIcon"
        if (mysteryIcon == null && inventoryPanel != null)
        {
            // Look for the icon inside the persistent Canvas_Inventory object.
            // NOTE: The path is often Canvas_Inventory/Panel/ArtifactIconSlot. We'll use your original attempt: Panel/MysteryIcon.
            Transform artifactIconTransform = inventoryPanel.transform.Find("Panel/MysteryIcon");
            
            if (artifactIconTransform != null)
            {
                mysteryIcon = artifactIconTransform.gameObject;
            }
            else
            {
                // Log a warning if the icon slot inside the panel is missing, but don't halt execution
                Debug.LogWarning("Could not find artifact icon slot at path 'Panel/MysteryIcon' inside Canvas_Inventory.");
            }
        }
        // -------------------------------------------------------------

        // --- STEP 2: Initialize UI State ---
        if (compassButton != null)
        {
            compassClosedPos = compassButton.anchoredPosition;
        }
        
        // --- STEP 3: CRITICAL FIX: Programmatically re-link the main button's OnClick event ---
        // This is the most crucial step that fixes the clickability after persistence/scene loading.
        if (compassButton != null)
        {
            Button compassBtnComponent = compassButton.GetComponent<Button>();
            if (compassBtnComponent != null)
            {
                compassBtnComponent.onClick.RemoveAllListeners(); 
                compassBtnComponent.onClick.AddListener(ToggleInventory);
                Debug.Log("Inventory Compass Button Listener re-linked successfully!");
            }
            else
            {
                Debug.LogError("The visible 'MysteryIcon' (Compass) object is missing the Button component!");
            }
        }
        
        // --- STEP 4: Retain Artifact State ---
        UpdateInventoryUI();
    }
    
    // Call this after collecting or using the item to refresh the UI
    private void UpdateInventoryUI()
    {
        // The mysteryIcon is the visual slot for the artifact INSIDE the inventory panel.
        if (mysteryIcon != null)
        {
           mysteryIcon.SetActive(hasMysteryObject);

            if (hasMysteryObject && currentMysterySprite != null)
            {
                Image image = mysteryIcon.GetComponent<Image>();
                if (image != null)
                    image.sprite = currentMysterySprite; // ✅ Swap icon in UI
            }

            
            // Additionally, ensure the artifact icon's button calls UseMysteryObject()
            Button artifactButton = mysteryIcon.GetComponent<Button>();
            if (artifactButton != null)
            {
                artifactButton.onClick.RemoveAllListeners();
                artifactButton.onClick.AddListener(UseMysteryObject);
            }
        }
    }
    // Public function to toggle the inventory visibility
    public void ToggleInventory()
    {
        if (inventoryPanel == null || compassButton == null)
        {
            Debug.LogError("Cannot toggle inventory: UI references were not found in Awake.");
            return;
        }
        
        // IMPORTANT: The panel that opens is the Canvas_Inventory object itself, based on your original logic.
        inventoryOpen = !inventoryOpen;
        inventoryPanel.SetActive(inventoryOpen);

        // Move the compass button
        Vector2 targetPos = inventoryOpen 
            ? compassClosedPos + new Vector2(0, compassMoveUpAmount) 
            : compassClosedPos;

        compassButton.anchoredPosition = targetPos;
        
        Debug.Log($"Inventory Toggled: {inventoryOpen}");
    }

    // This function is called when the artifact is clicked INSIDE the inventory
    public void UseMysteryObject()
    {
        if (!hasMysteryObject) 
        {
            Debug.LogWarning("Tried to use object, but inventory is empty.");
            return;
        }
        
        if (GameManager.Instance == null)
        {
             Debug.LogError("GameManager instance not available to spawn object!");
             return;
        }

        // 1. Call the GameManager to spawn the item in the world
        GameManager.Instance.SpawnMysteryObjectAtPlayer();

        // 2. Update the state
        hasMysteryObject = false;
        
        // 3. Update the UI (hides the icon)
        UpdateInventoryUI();
        
        // 4. Close the inventory after use (optional, but good UX)
        if (inventoryOpen)
        {
            ToggleInventory();
        }
        
        Debug.Log("Mystery Object used and spawned.");
    }
    
    // Function to be called when the player picks up the artifact
    public void CollectMysteryObject(Sprite newSprite)
    {
        hasMysteryObject = true;
        currentMysterySprite = newSprite; // ✅ Save the right icon to display
        UpdateInventoryUI();
    }


    public void ReinitializeUI()
{
    Debug.Log("🔁 Reinitializing Inventory UI after scene load...");

    // Re-find the inventory panel if needed
    if (inventoryPanel == null)
        inventoryPanel = GameObject.Find("Canvas_Inventory");

    // Re-find the compass button
    if (compassButton == null)
    {
        GameObject compassObject = GameObject.Find("MysteryIcon");
        if (compassObject != null)
        {
            compassButton = compassObject.GetComponent<RectTransform>();
            if (compassButton != null)
            {
                Button compassBtnComponent = compassButton.GetComponent<Button>();
                if (compassBtnComponent != null)
                {
                    compassBtnComponent.onClick.RemoveAllListeners();
                    compassBtnComponent.onClick.AddListener(ToggleInventory);
                    Debug.Log("✅ Compass button re-linked successfully.");
                }
                compassClosedPos = compassButton.anchoredPosition;
            }
        }
    }

    // Re-find the icon inside inventory panel
    if (mysteryIcon == null && inventoryPanel != null)
    {
        Transform artifactIconTransform = inventoryPanel.transform.Find("Panel/MysteryIcon");
        if (artifactIconTransform != null)
        {
            mysteryIcon = artifactIconTransform.gameObject;
        }
    }

    UpdateInventoryUI(); // Refresh state
    }

}