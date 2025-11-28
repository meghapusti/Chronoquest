using UnityEngine;
using UnityEngine.InputSystem;

public class MysteryObjectPickup : MonoBehaviour
{
    [Header("Highlight")]
    public Color highlightColor = Color.yellow;

    private Renderer rend;
    private Material[] materials;
    private Color[] originalColors;
    private bool isSelected = false;

    void Start()
    {
        // Guardamos materiales y colores originales
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            materials = rend.materials;
            originalColors = new Color[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                originalColors[i] = materials[i].color;
            }
        }
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // Solo reaccionamos cuando se hace click IZQUIERDO este frame
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    // Hemos hecho click en ESTE mystery object
                    HandleClickOnThis();
                }
            }
        }
    }

    void HandleClickOnThis()
    {
        if (!isSelected)
        {
            // 1er click → seleccionamos y ponemos amarillo
            isSelected = true;
            SetHighlight(true);
            Debug.Log("✨ MysteryObject seleccionado (primer click).");
        }
        else
        {
            // 2º click → añadir a mochila y destruir
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddMysteryObject();
                Debug.Log("🎒 MysteryObject añadido a la mochila (segundo click).");
            }
            else
            {
                Debug.LogWarning("No hay InventoryManager en la escena.");
            }

            SetHighlight(false);
            Destroy(gameObject);
        }
    }

    void SetHighlight(bool on)
    {
        if (materials == null) return;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = on ? highlightColor : originalColors[i];
        }
    }

    // ⚠️ Ya NO recogemos por colisión.
    // Este método se puede borrar, pero si Unity lo tiene referenciado,
    // lo dejamos vacío para que no haga nada.
    void OnCollisionEnter(Collision collision)
    {
        // Antes se recogía al chocar con el Player.
        // Ahora todo va por clicks, así que lo dejamos vacío.
    }
}
