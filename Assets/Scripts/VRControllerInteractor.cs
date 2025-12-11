using UnityEngine;
using UnityEngine.InputSystem;

public class VRControllerInteractor : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference grabAction;
    public InputActionReference selectAction;

    [Header("Ray Settings")]
    public LineRenderer pointerRay;
    public float maxRayDistance = 10f;
    public Color idleColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Color buttonColor = Color.green;

    [Header("Grab Settings")]
    public Transform grabPoint;
    public float grabRadius = 0.3f;

    private GameObject currentGrabbedObject;
    private Rigidbody grabbedRigidbody;
    private bool wasKinematic;
    private RaycastHit currentHit;
    private bool isPointingAtInteractable;

    void Start()
    {
        if (pointerRay != null)
        {
            pointerRay.positionCount = 2;
            pointerRay.startWidth = 0.02f;
            pointerRay.endWidth = 0.02f;
        }
    }

    void OnEnable()
    {
        if (grabAction != null && grabAction.action != null)
        {
            grabAction.action.performed += OnGrabPerformed;
            grabAction.action.canceled += OnGrabCanceled;
            grabAction.action.Enable();
        }

        if (selectAction != null && selectAction.action != null)
        {
            selectAction.action.performed += OnSelectPerformed;
            selectAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (grabAction != null && grabAction.action != null)
        {
            grabAction.action.performed -= OnGrabPerformed;
            grabAction.action.canceled -= OnGrabCanceled;
        }

        if (selectAction != null && selectAction.action != null)
        {
            selectAction.action.performed -= OnSelectPerformed;
        }
    }

    void Update()
    {
        UpdatePointerRay();

        if (currentGrabbedObject != null && grabPoint != null)
        {
            currentGrabbedObject.transform.position = grabPoint.position;
            currentGrabbedObject.transform.rotation = grabPoint.rotation;
        }
    }

    void UpdatePointerRay()
    {
        if (pointerRay == null) return;

        pointerRay.SetPosition(0, transform.position);

        Vector3 rayDirection = transform.forward;
        isPointingAtInteractable = false;

        if (Physics.Raycast(transform.position, rayDirection, out currentHit, maxRayDistance))
        {
            pointerRay.SetPosition(1, currentHit.point);

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
            pointerRay.SetPosition(1, transform.position + rayDirection * maxRayDistance);
            pointerRay.startColor = idleColor;
            pointerRay.endColor = idleColor;
        }
    }

    void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (currentGrabbedObject == null)
        {
            TryGrabObject();
        }
    }

    void OnGrabCanceled(InputAction.CallbackContext context)
    {
        if (currentGrabbedObject != null)
        {
            ReleaseObject();
        }
    }

    void OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (isPointingAtInteractable)
        {
            TeleportButton teleportBtn = currentHit.collider.GetComponent<TeleportButton>();
            if (teleportBtn != null)
            {
                PressButton(teleportBtn);
                return;
            }

            MysteryObjectPickup mysteryObj = currentHit.collider.GetComponent<MysteryObjectPickup>();
            if (mysteryObj != null)
            {
                mysteryObj.VRSelect();
            }
        }
    }

    void TryGrabObject()
    {
        if (grabPoint == null) return;

        Collider[] nearbyObjects = Physics.OverlapSphere(grabPoint.position, grabRadius);

        foreach (Collider col in nearbyObjects)
        {
            MysteryObjectPickup mysteryObj = col.GetComponent<MysteryObjectPickup>();
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (mysteryObj != null && rb != null)
            {
                currentGrabbedObject = col.gameObject;
                grabbedRigidbody = rb;

                wasKinematic = rb.isKinematic;
                rb.isKinematic = true;
                rb.useGravity = false;

                Debug.Log("Grabbed: " + currentGrabbedObject.name);
                break;
            }
        }
    }

    void ReleaseObject()
    {
        if (currentGrabbedObject != null && grabbedRigidbody != null)
        {
            grabbedRigidbody.isKinematic = wasKinematic;
            grabbedRigidbody.useGravity = true;

            Debug.Log("Released: " + currentGrabbedObject.name);

            currentGrabbedObject = null;
            grabbedRigidbody = null;
        }
    }

    void PressButton(TeleportButton button)
    {
        if (button != null)
        {
            TeleportManager tm = FindObjectOfType<TeleportManager>();
            if (tm != null)
            {
                tm.ExecuteTeleportVR(button.targetSceneName, button.targetSpawnPointName);
            }

            Debug.Log("Pressed button: " + button.gameObject.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (grabPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(grabPoint.position, grabRadius);
        }
    }
}