using UnityEngine;
using UnityEngine.InputSystem; // Required for InputValue and InputSystem

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float lookSpeed = 2f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private CharacterController controller;
    private Transform playerCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main.transform;

        // ✅ Forzar que el Input System use Dynamic Update (reduce lag del ratón)
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }

    void Update()
    {
        // --- Movement ---
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);
    }

    // 👇 Unity llama automáticamente a esto desde Player Input (Send Messages)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // 👇 Maneja la rotación con el ratón o joystick derecho
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        // Rotación horizontal del jugador
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);
    }
}


