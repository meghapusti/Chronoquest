using UnityEngine;
using UnityEngine.InputSystem; // Required for InputValue and Keyboard

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotateSpeed = 120f; // velocidad de giro con flechas

    private Vector2 moveInput; // viene de la acción "Move" (WASD)
    private float lookX;       // viene de la acción "Look" (flechas izq/der)

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller == null) return;

        // -------- MOVIMIENTO ADELANTE/ATRÁS Y STRAFE (WASD + FLECHAS ↑↓) --------
        // moveInput.x = A/D   |   moveInput.y = W/S   (del Input System)
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Añadimos también flechas ↑ ↓ a mano para que muevan al player
        var kb = Keyboard.current;
        if (kb != null)
        {
            float extraForward = 0f;
            if (kb.upArrowKey.isPressed)   extraForward += 1f;
            if (kb.downArrowKey.isPressed) extraForward -= 1f;

            move += transform.forward * extraForward;
        }

        // Normalizar para no correr más en diagonal
        if (move.sqrMagnitude > 1f) move.Normalize();

        controller.Move(move * speed * Time.deltaTime);

        // -------- GIRO SOLO CON FLECHAS IZQ / DER (acción Look) --------
        // lookX será -1 con LeftArrow, +1 con RightArrow según tu Input Action
        transform.Rotate(0f, lookX * rotateSpeed * Time.deltaTime, 0f);
    }

    // Llamado por PlayerInput (acción "Move")
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Llamado por PlayerInput (acción "Look")
    public void OnLook(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lookX = v.x; // solo usamos el eje horizontal (izq/der)
    }
}


