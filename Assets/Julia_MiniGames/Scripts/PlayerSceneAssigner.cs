// PlayerSceneAssigner.cs
using UnityEngine;

public class PlayerSceneAssigner : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            // Assign THIS player's Transform to the persistent GameManager
            GameManager.Instance.playerTransform = this.transform;
        }
    }
}