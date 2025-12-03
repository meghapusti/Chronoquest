using UnityEngine;

public class PersistEventSystem : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
