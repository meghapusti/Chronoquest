// TeleportButton.cs
using UnityEngine;

// This script only holds data for the TeleportManager to read.
public class TeleportButton : MonoBehaviour
{
    [Tooltip("The exact name of the scene to load (e.g., MedievalEra)")]
    public string targetSceneName;

    [Tooltip("The exact name of the spawn point in the target scene (e.g., MedievalSpawn)")]
    public string targetSpawnPointName;
}