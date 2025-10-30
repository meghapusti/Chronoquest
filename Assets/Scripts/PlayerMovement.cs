using UnityEngine;
using UnityEngine.InputSystem; // Required for InputValue

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
    }

    void Update()
    {
        // --- Movement ---
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);
    }

    // 👇 THIS IS WHAT UNITY INPUT EVENTS LOOK FOR
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // 👇 THIS ONE HANDLES MOUSE OR RIGHT STICK LOOK
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();

        // Example camera rotation (optional)
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * Time.deltaTime);
    }
}
