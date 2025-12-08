// TeleportManager.cs
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TeleportManager : MonoBehaviour
{
    // === Public Inspector Variables ===
    // Drag your Input Actions Asset's generated C# class here (e.g., MyControls.Player)
    public InputActionReference clickAction; 
    public float maxDistance = 10f; 

    // === Private References ===
    private Camera mainCamera;

    // --- Unity Lifecycle ---
    private void Awake()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main; 
        if (mainCamera == null)
        {
            Debug.LogError("TeleportManager requires a Camera tagged 'MainCamera' in the scene.");
        }
    }

    private void OnEnable()
    {
        // Subscribe to the Input Action's 'performed' phase
        clickAction.action.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        clickAction.action.performed -= OnClickPerformed;
    }

    // --- Input System Callback ---
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        // This method fires ONLY when the "Click" action is performed (e.g., mouse pressed)
        
        if (mainCamera == null) return;

        // Create a ray from the camera, at the screen center (for simplicity in 3D/VR testing)
        // For a desktop test, you might use Mouse.current.position.ReadValue() to get the mouse position.
        
        // For simple mouse click on desktop:
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        // Use Physics.Raycast to check for hits
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            // Check if the hit object has our TeleportButton component
            TeleportButton button = hit.collider.GetComponent<TeleportButton>();

            if (button != null)
            {
                // We hit a button! Call the teleport function
                ExecuteTeleport(button.targetSceneName, button.targetSpawnPointName);
            }
        }
    }
    public void ExecuteTeleportVR(string sceneName, string spawnPointName)
    {
        ExecuteTeleport(sceneName, spawnPointName);
    }
    // --- Teleportation Logic ---
    private void ExecuteTeleport(string sceneName, string spawnPointName)
    {
        // Save the target spawn point name so the next scene can read it
        PlayerPrefs.SetString("TargetSpawnPoint", spawnPointName);
        Debug.Log($"Teleporting to scene: {sceneName} and targeting spawn point: {spawnPointName}");

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}