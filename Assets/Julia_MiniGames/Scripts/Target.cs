using UnityEngine;

public class Target : MonoBehaviour
{
    public int baseScore = 10;
    bool alreadyHit = false;   // esta diana ya ha sido acertada o no

    // Llamado por la piedra cuando impacta
    public void RegisterHit(Stone stone, Collision col)
    {
        // Si ya se había acertado antes, no hacemos nada
        if (alreadyHit)
            return;

        alreadyHit = true;
        Debug.Log("¡Impacto en Target por primera vez!");

        // 1) Sumar puntos (10 por diana)
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(baseScore);

        // 2) Pegar la piedra a la superficie
        if (stone != null)
            stone.StickTo(col);

        // 3) Destruir la diana (y la piedra se va con ella porque es hija)
        Destroy(gameObject, 0.1f);
    }
}
