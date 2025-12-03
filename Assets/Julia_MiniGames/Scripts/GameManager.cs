using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }

    public TextMeshProUGUI scoreText;

    [Header("Objeto misterioso")]
    [SerializeField] int mysteryScoreThreshold = 30;
    [SerializeField] GameObject mysteryObjectPrefab; // Default for Caveman
    [SerializeField] GameObject medievalMysteryObjectPrefab; // For Medieval
    public Transform playerTransform; 
    [SerializeField] float forwardDistance = 10f;
    [SerializeField] float initialDropHeight = 3f; 
    [SerializeField] float inventoryDropOffset = 0.5f;

    private bool mysterySpawned = false;
    private int hitCount = 0;

    private GameObject collectedMysteryPrefab; // ✅ NEW: Track actual collected object

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scoreText == null)
        {
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
            {
                scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
                Debug.Log("✅ GameManager: ScoreText re-linked after scene load.");
            }
        }

        // 👇 Scene-specific prefab override
        if (scene.name == "Medieval Era" && medievalMysteryObjectPrefab != null)
        {
            mysteryObjectPrefab = medievalMysteryObjectPrefab;
            Debug.Log("🏰 Switched to Medieval mystery object prefab.");
        }

        // ✅ Reset hit count after teleport
        hitCount = 0;
        mysterySpawned = false;

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        hitCount++;

        Debug.Log($"Score: {Score} | Hits: {hitCount}");
        UpdateUI();
        CheckMysteryObject();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Score}";
        }
    }

    void CheckMysteryObject()
    {
        // ✅ Only spawn after exactly 3 hits
        if (mysterySpawned || hitCount < 3) return;

        if (mysteryObjectPrefab == null || playerTransform == null)
        {
            Debug.LogWarning("⚠️ Missing prefab or player reference.");
            mysterySpawned = true;
            return;
        }

        Vector3 forwardPos = playerTransform.position + playerTransform.forward * forwardDistance;
        Vector3 spawnPos = forwardPos + Vector3.up * initialDropHeight;

        GameObject spawned = Instantiate(mysteryObjectPrefab, spawnPos, Quaternion.identity);

        // ✅ Store actual prefab so inventory spawn matches this one
        collectedMysteryPrefab = mysteryObjectPrefab;

        mysterySpawned = true;
        Debug.Log("🌟 Mystery object spawned.");
    }

    public void SpawnMysteryObjectAtPlayer()
    {
        // ✅ Use collected object (not current prefab)
        GameObject prefabToSpawn = collectedMysteryPrefab != null ? collectedMysteryPrefab : mysteryObjectPrefab;

        if (prefabToSpawn == null || playerTransform == null)
        {
            Debug.LogError("⚠️ Cannot spawn: missing prefab or player reference.");
            return;
        }

        float objectHalfHeight = 1f;
        MeshRenderer meshRenderer = prefabToSpawn.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            objectHalfHeight = meshRenderer.bounds.extents.y;
        }

        Vector3 forwardPos = playerTransform.position + playerTransform.forward * forwardDistance;
        Vector3 spawnPos = forwardPos + Vector3.up * objectHalfHeight;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        mysterySpawned = false;
        Debug.Log($"🎯 Spawned collected mystery object in world.");
    }

    // ✅ Called by InventoryManager when player picks up object
    public void RegisterCollectedMystery(GameObject pickedUpPrefab)
    {
        collectedMysteryPrefab = pickedUpPrefab;
        Debug.Log($"📦 Collected mystery object registered: {pickedUpPrefab.name}");
    }
}
