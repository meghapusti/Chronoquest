using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadHandler : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("📦 Scene Loaded: " + scene.name);

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ReinitializeUI();
        }
    }
}
