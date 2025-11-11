using UnityEngine;

public class Target : MonoBehaviour
{
    public int baseScore = 10;

    // Este método será llamado cuando la piedra impacte
    public void RegisterHit(Collision col)
    {
        Debug.Log("¡Impacto en Target!");

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(baseScore);
    }
}

