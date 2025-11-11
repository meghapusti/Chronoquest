using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour
{
    [Header("Lifecycles")]
    public float life = 6f;                 // si NO se pega, se destruye a los 6s
    public float stickDestroyAfter = -1f;   // >0 para autodestruir la piedra pegada; -1 = para siempre

    [Header("Pegado")]
    public float stickOffset = 0.02f;       // separa un pelín para evitar z-fighting

    bool stuck = false;
    Rigidbody rb;
    Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        // antes destruías siempre aquí; ahora solo si no llega a pegarse:
        StartCoroutine(SelfDestructIfNotStuck());
    }

    IEnumerator SelfDestructIfNotStuck()
    {
        yield return new WaitForSeconds(life);
        if (!stuck) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision c)
    {
        Debug.Log($"Hit {c.collider.name} (tag {c.collider.tag})");

        if (c.collider.CompareTag("Target"))
        {
            // sumar puntos
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(20);

            // pegar la piedra a la superficie impactada
            StickTo(c);
        }
    }

    void StickTo(Collision c)
    {
        if (stuck) return;
        stuck = true;

        // parar física
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // ya no la afecta la física
        }

        // colocar en el punto de contacto y orientar la "punta" hacia dentro
        var contact = c.GetContact(0);
        transform.position = contact.point + contact.normal * stickOffset;
        transform.rotation = Quaternion.LookRotation(-contact.normal, Vector3.up);

        // opcional: desactivar el collider para que no siga chocando
        if (col != null) col.enabled = false;

        // hacerla hija del objeto golpeado para que se mueva con él si hace falta
        transform.SetParent(c.collider.transform, true);

        // si quieres que también desaparezca tras un rato aun pegada
        if (stickDestroyAfter > 0f) Destroy(gameObject, stickDestroyAfter);
    }
}
