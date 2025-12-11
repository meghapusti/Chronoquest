// TeleportManager.cs (Using New Input System - Mouse)
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TeleportManager : MonoBehaviour
{
    public float maxDistance = 10f;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("TeleportManager requires a Camera tagged 'MainCamera' in the scene.");
        }
    }

    private void Update()
    {
        // Check for left mouse button click using New Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Camera is null!");
            return;
        }

        // Get mouse position using New Input System
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        Debug.Log($"Raycasting from mouse position: {mousePosition}");

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Debug.Log($"Hit object: {hit.collider.gameObject.name}");

            TeleportButton button = hit.collider.GetComponent<TeleportButton>();

            if (button != null)
            {
                Debug.Log($"Found TeleportButton! Teleporting to {button.targetSceneName}");
                ExecuteTeleport(button.targetSceneName, button.targetSpawnPointName);
            }
            else
            {
                Debug.Log("Hit object doesn't have TeleportButton component");
            }
        }
        else
        {
            Debug.Log("Raycast didn't hit anything within range");
        }
    }

    private void ExecuteTeleport(string sceneName, string spawnPointName)
    {
        PlayerPrefs.SetString("TargetSpawnPoint", spawnPointName);
        Debug.Log($"Teleporting to scene: {sceneName} and targeting spawn point: {spawnPointName}");
        SceneManager.LoadScene(sceneName);
    }
}