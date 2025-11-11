using UnityEngine;
using UnityEngine.InputSystem; // Para detectar teclas o clics

public class Thrower : MonoBehaviour
{
    [Header("Setup")]
    public GameObject stonePrefab;
    public Transform throwPoint;

    [Header("Tuning")]
    public float throwForce = 25f;
    public float upForceFactor = 0.35f;
    public float cooldown = 0.25f;

    float nextAllowed;

    // Se llama automáticamente al presionar "Fire"
    public void OnFire(InputValue value)
    {
        if (!value.isPressed) return;         // solo al presionar
        if (Time.time < nextAllowed) return;  // cooldown
        nextAllowed = Time.time + cooldown;

        if (stonePrefab == null || throwPoint == null) return;

        // Instanciar piedra
        var go = Instantiate(stonePrefab, throwPoint.position, throwPoint.rotation);
        var rb = go.GetComponent<Rigidbody>();

        // 🔒 Evitar que la piedra colisione contigo
        var stoneCol = go.GetComponent<Collider>();
        var playerCols = GetComponentsInChildren<Collider>();
        foreach (var pc in playerCols)
            Physics.IgnoreCollision(stoneCol, pc);

        // Lanzarla hacia adelante
        if (rb != null)
        {
            Vector3 dir = throwPoint.forward + Vector3.up * upForceFactor;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(dir.normalized * throwForce, ForceMode.Impulse);
        }
    }
}





