using UnityEngine;

public class MysteryObjectPickup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("💥 MysteryObject ha chocado con: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("✅ MysteryObject ha chocado con el PLAYER. Lo añadimos al inventario.");
            InventoryManager.Instance.AddMysteryObject();
            Destroy(gameObject);
        }
    }
}
