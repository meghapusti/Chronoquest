using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour
{
    [Header("Lifecycles")]
    public float life = 6f;                 // si NO se pega, se destruye a los 6s
    public float stickDestroyAfter = -1f;   // >0 para autodestruir la piedra pegada; -1 = para siempre

    [Header("Pegado")]
    public float stickOffset = 0.02f;       // separa un poquito para evitar z-fighting

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
        if (life > 0f)
            Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision c)
    {
        if (stuck) return;

        // ¿Golpeó un Target?
        Target target = c.collider.GetComponent<Target>();

        if (target != null)
        {
            // Tu Target usa: RegisterHit(Stone, Collision)
            target.RegisterHit(this, c);
        }
    }

    public void StickTo(Collision c)
    {
        if (stuck) return;
        stuck = true;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (col != null) col.enabled = false;

        // Colocar exactamente en el punto de impacto
        var contact = c.GetContact(0);
        transform.position = contact.point + contact.normal * stickOffset;
        transform.rotation = Quaternion.LookRotation(-contact.normal, Vector3.up);

        // Hacerla hija del objeto golpeado (opcional)
        transform.SetParent(c.collider.transform, true);

        // Si quieres que desaparezca pegada tras un tiempo
        if (stickDestroyAfter > 0f)
            Destroy(gameObject, stickDestroyAfter);
    }
}


