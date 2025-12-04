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
    [SerializeField] GameObject mysteryObjectPrefab; // For Caveman
    [SerializeField] GameObject medievalMysteryObjectPrefab; // For Medieval
    [SerializeField] private GameObject futuristicMysteryObjectPrefab; // For Futuristic

    public Transform playerTransform;
    [SerializeField] float forwardDistance = 10f;
    [SerializeField] float initialDropHeight = 3f;
    [SerializeField] float inventoryDropOffset = 0.5f;

    private bool mysterySpawned = false;
    private int hitCount = 0;

    private GameObject collectedMysteryPrefab;

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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            Debug.Log("👤 GameManager: Player reference updated.");
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager: Player object not found in scene.");
        }

        // Scene-specific prefab override
        if (scene.name == "Medieval Era" && medievalMysteryObjectPrefab != null)
        {
            mysteryObjectPrefab = medievalMysteryObjectPrefab;
            Debug.Log("🏰 Switched to Medieval mystery object prefab.");
        }
        else if (scene.name == "Futuristic" && futuristicMysteryObjectPrefab != null)
        {
            mysteryObjectPrefab = futuristicMysteryObjectPrefab;
            Debug.Log("🌇 Switched to Futuristic mystery object prefab.");
        }

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

    // ✅ FIXED: Use correct prefab for scene, don’t overwrite it!
    void CheckMysteryObject()
    {
        if (mysterySpawned || hitCount < 3) return;

        if (playerTransform == null)
        {
            Debug.LogWarning("⚠️ Cannot spawn: Player reference is null.");
            mysterySpawned = true;
            return;
        }

        string scene = SceneManager.GetActiveScene().name;
        GameObject prefabToUse = null;

        if (scene == "Caveman Era")
            prefabToUse = mysteryObjectPrefab;
        else if (scene == "Medieval Era")
            prefabToUse = medievalMysteryObjectPrefab;
        else if (scene == "Futuristic")
            prefabToUse = futuristicMysteryObjectPrefab;

        if (prefabToUse == null)
        {
            Debug.LogWarning("⚠️ No mystery prefab assigned for this scene.");
            mysterySpawned = true;
            return;
        }

        Vector3 forwardPos = playerTransform.position + playerTransform.forward * forwardDistance;
        Vector3 spawnPos = forwardPos + Vector3.up * initialDropHeight;

        Instantiate(prefabToUse, spawnPos, Quaternion.identity);

        collectedMysteryPrefab = prefabToUse; // ✅ Store correct one for inventory
        mysterySpawned = true;

        Debug.Log($"🌟 Spawned: {prefabToUse.name} at {spawnPos}");
    }

    // ✅ FIXED: Inventory drops exact collected object at player
    public void SpawnMysteryObjectAtPlayer()
    {
        GameObject prefabToSpawn = collectedMysteryPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("❌ No collectedMysteryPrefab set!");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("❌ Player reference missing in SpawnMysteryObjectAtPlayer!");
            return;
        }

        Vector3 pos = playerTransform.position + playerTransform.forward * forwardDistance;
        pos += Vector3.up * 1.0f;

        Instantiate(prefabToSpawn, pos, Quaternion.identity);
        mysterySpawned = false;

        Debug.Log($"📦 Spawned object from inventory: {prefabToSpawn.name}");
    }

    public void RegisterCollectedMystery(GameObject pickedUpPrefab)
    {
        collectedMysteryPrefab = pickedUpPrefab;
        Debug.Log($"📦 Collected mystery object registered: {pickedUpPrefab.name}");
    }
}
