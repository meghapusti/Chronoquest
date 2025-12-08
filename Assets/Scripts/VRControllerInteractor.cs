using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Attach this to each XR Controller (left and right)
/// Handles grabbing objects and pressing buttons with visual feedback
/// </summary>
public class VRControllerInteractor : MonoBehaviour
{
    [Header("Input Actions")]
    [Tooltip("Trigger button for grabbing objects")]
    public InputActionReference grabAction;

    [Tooltip("Primary button for pressing UI buttons")]
    public InputActionReference selectAction;

    [Header("Ray Settings")]
    [Tooltip("Line renderer for visual feedback")]
    public LineRenderer pointerRay;

    [Tooltip("Maximum distance for ray interaction")]
    public float maxRayDistance = 10f;

    [Tooltip("Color when pointing at nothing")]
    public Color idleColor = Color.white;

    [Tooltip("Color when pointing at interactable")]
    public Color highlightColor = Color.yellow;

    [Tooltip("Color when pointing at button")]
    public Color buttonColor = Color.green;

    [Header("Grab Settings")]
    [Tooltip("Transform where grabbed objects will attach")]
    public Transform grabPoint;

    [Tooltip("Distance to check for grabbable objects")]
    public float grabRadius = 0.3f;

    // Private variables
    private GameObject currentGrabbedObject;
    private Rigidbody grabbedRigidbody;
    private bool wasKinematic;
    private RaycastHit currentHit;
    private bool isPointingAtInteractable;

    void OnEnable()
    {
        if (grabAction != null)
        {
            grabAction.action.performed += OnGrabPerformed;
            grabAction.action.canceled += OnGrabCanceled;
        }

        if (selectAction != null)
        {
            selectAction.action.performed += OnSelectPerformed;
        }
    }

    void OnDisable()
    {
        if (grabAction != null)
        {
            grabAction.action.performed -= OnGrabPerformed;
            grabAction.action.canceled -= OnGrabCanceled;
        }

        if (selectAction != null)
        {
            selectAction.action.performed -= OnSelectPerformed;
        }
    }

    void Update()
    {
        UpdatePointerRay();

        // If holding an object, keep it at grab point
        if (currentGrabbedObject != null && grabPoint != null)
        {
            currentGrabbedObject.transform.position = grabPoint.position;
            currentGrabbedObject.transform.rotation = grabPoint.rotation;
        }
    }

    /// <summary>
    /// Updates the visual ray and checks what it's pointing at
    /// </summary>
    void UpdatePointerRay()
    {
        if (pointerRay == null) return;

        // Set ray start position
        pointerRay.SetPosition(0, transform.position);

        // Cast ray forward
        Vector3 rayDirection = transform.forward;
        isPointingAtInteractable = false;

        if (Physics.Raycast(transform.position, rayDirection, out currentHit, maxRayDistance))
        {
            // Hit something
            pointerRay.SetPosition(1, currentHit.point);

            // Check what we hit
            if (currentHit.collider.GetComponent<TeleportButton>() != null)
            {
                pointerRay.startColor = buttonColor;
                pointerRay.endColor = buttonColor;
                isPointingAtInteractable = true;
            }
            else if (currentHit.collider.GetComponent<MysteryObjectPickup>() != null)
            {
                pointerRay.startColor = highlightColor;
                pointerRay.endColor = highlightColor;
                isPointingAtInteractable = true;
            }
            else
            {
                pointerRay.startColor = idleColor;
                pointerRay.endColor = idleColor;
            }
        }
        else
        {
            // No hit, extend ray to max distance
            pointerRay.SetPosition(1, transform.position + rayDirection * maxRayDistance);
            pointerRay.startColor = idleColor;
            pointerRay.endColor = idleColor;
        }
    }

    /// <summary>
    /// Called when trigger is pressed
    /// </summary>
    void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (currentGrabbedObject == null)
        {
            TryGrabObject();
        }
    }

    /// <summary>
    /// Called when trigger is released
    /// </summary>
    void OnGrabCanceled(InputAction.CallbackContext context)
    {
        if (currentGrabbedObject != null)
        {
            ReleaseObject();
        }
    }

    /// <summary>
    /// Called when select button is pressed (for UI interactions)
    /// </summary>
    void OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (isPointingAtInteractable)
        {
            // Check if pointing at button
            TeleportButton teleportBtn = currentHit.collider.GetComponent<TeleportButton>();
            if (teleportBtn != null)
            {
                PressButton(teleportBtn);
                return;
            }

            // Check if pointing at mystery object
            MysteryObjectPickup mysteryObj = currentHit.collider.GetComponent<MysteryObjectPickup>();
            if (mysteryObj != null)
            {
                mysteryObj.VRSelect();
            }
        }
    }

    /// <summary>
    /// Attempts to grab the nearest object
    /// </summary>
    void TryGrabObject()
    {
        // Check sphere around grab point for objects
        Collider[] nearbyObjects = Physics.OverlapSphere(grabPoint.position, grabRadius);

        foreach (Collider col in nearbyObjects)
        {
            // Check if it's a grabbable object (has Rigidbody and MysteryObjectPickup)
            MysteryObjectPickup mysteryObj = col.GetComponent<MysteryObjectPickup>();
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (mysteryObj != null && rb != null)
            {
                currentGrabbedObject = col.gameObject;
                grabbedRigidbody = rb;

                // Store original kinematic state
                wasKinematic = rb.isKinematic;

                // Make kinematic while holding
                rb.isKinematic = true;
                rb.useGravity = false;

                Debug.Log($"✋ Grabbed: {currentGrabbedObject.name}");
                break;
            }
        }
    }

    /// <summary>
    /// Releases the currently held object
    /// </summary>
    void ReleaseObject()
    {
        if (currentGrabbedObject != null && grabbedRigidbody != null)
        {
            // Restore physics
            grabbedRigidbody.isKinematic = wasKinematic;
            grabbedRigidbody.useGravity = true;

            Debug.Log($"🖐️ Released: {currentGrabbedObject.name}");

            currentGrabbedObject = null;
            grabbedRigidbody = null;
        }
    }

    /// <summary>
    /// Presses a button (teleport or UI)
    /// </summary>
    void PressButton(TeleportButton button)
    {
        if (button != null)
        {
            // Visual feedback
            StartCoroutine(ButtonPressEffect(currentHit.collider.gameObject));

            // Execute teleport through TeleportManager
            TeleportManager tm = FindObjectOfType<TeleportManager>();
            if (tm != null)
            {
                tm.ExecuteTeleportVR(button.targetSceneName, button.targetSpawnPointName);
            }

            Debug.Log($"🔘 Pressed button: {button.gameObject.name}");
        }
    }

    /// <summary>
    /// Visual effect when pressing a button
    /// </summary>
    System.Collections.IEnumerator ButtonPressEffect(GameObject button)
    {
        Vector3 originalScale = button.transform.localScale;
        button.transform.localScale = originalScale * 0.9f;
        yield return new WaitForSeconds(0.1f);
        button.transform.localScale = originalScale;
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        if (grabPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(grabPoint.position, grabRadius);
        }
    }
}