// PlayerSpawnOnLoad.cs
using UnityEngine;

public class PlayerSpawnOnLoad : MonoBehaviour
{
    private void Start()
    {
        // 1. Get the target spawn point name saved before the scene load
        string targetSpawnPointName = PlayerPrefs.GetString("TargetSpawnPoint", "");

        // Only proceed if a target name was saved
        if (!string.IsNullOrEmpty(targetSpawnPointName))
        {
            // 2. Find the GameObject with the matching name
            GameObject spawnPoint = GameObject.Find(targetSpawnPointName);

            if (spawnPoint != null)
            {
                // 3. Move the Player object to the spawn point's position and rotation
                transform.position = spawnPoint.transform.position;
                transform.rotation = spawnPoint.transform.rotation;
                Debug.Log($"Player spawned at {targetSpawnPointName}");
            }
            else
            {
                Debug.LogWarning($"Spawn point '{targetSpawnPointName}' not found in scene. Player staying at default position.");
            }
            
            // 4. Clear the key for the next time
            PlayerPrefs.DeleteKey("TargetSpawnPoint");
        }
    }
}