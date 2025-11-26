using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }

    public TextMeshProUGUI scoreText;

    // ----------- OBJETO MISTERIOSO -----------
    [Header("Objeto misterioso")]
    [SerializeField] int mysteryScoreThreshold = 30;
    [SerializeField] GameObject mysteryObjectPrefab;
    [SerializeField] Transform playerTransform;

    // Distancia hacia delante del jugador
    [SerializeField] float forwardDistance = 10f;

    // Altura desde la que cae
    [SerializeField] float dropHeight = 3f;

    bool mysterySpawned = false;
    // ----------------------------------------

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"Score: {Score}");
        UpdateUI();

        // Comprobamos si hay que spawnear el objeto misterioso
        CheckMysteryObject();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {Score}";
    }

    // --------- SPAWN DEL OBJETO MISTERIOSO ---------
    void CheckMysteryObject()
    {
        if (mysterySpawned)
            return;

        if (Score < mysteryScoreThreshold)
            return;

        if (mysteryObjectPrefab == null || playerTransform == null)
        {
            Debug.LogWarning("Falta mysteryObjectPrefab o playerTransform en el GameManager.");
            mysterySpawned = true;
            return;
        }

        // Posición delante del jugador, a una distancia configurable
        Vector3 forwardPos = playerTransform.position + playerTransform.forward * forwardDistance;

        // Lo elevamos para que caiga desde arriba
        Vector3 spawnPos = forwardPos + Vector3.up * dropHeight;

        // Instanciamos el objeto misterioso
        Instantiate(mysteryObjectPrefab, spawnPos, Quaternion.identity);

        mysterySpawned = true;
        Debug.Log("🌟 Objeto misterioso generado delante del jugador.");
    }
    // ------------------------------------------------
}
