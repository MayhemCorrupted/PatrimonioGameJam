using UnityEngine;
using UnityEngine.InputSystem;
public interface IInteractable
{
    void Interact();
}
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float interactRange = 1;
    [SerializeField] LayerMask interactLayer;
    [SerializeField] Transform interactPosition;
    [SerializeField] private InputActionAsset inputs;

    [Header("Debug")]
    [SerializeField] bool showGizmo = true;
    [SerializeField] Color undetectedColor = Color.green;
    [SerializeField] Color detectedColor = Color.blue;
    IInteractable currentInteract;
    void Update()
    {
        DetectInteractable();
        InteractInput();
    }
    void DetectInteractable()
    {
        if (interactPosition == null)
        {
            Debug.LogWarning($"Poner {interactPosition} en el inspector");
            return;
        }

        if (Physics.Raycast(interactPosition.position, interactPosition.forward, out RaycastHit hit,interactRange, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null) currentInteract = interactable;
            else currentInteract = null;
        }
        currentInteract = null;
    
    }
    void InteractInput()
    {
        if (inputs.FindAction("Interact").WasPressedThisFrame() && currentInteract != null)
        {
            currentInteract.Interact();
        }
    }
    void OnDrawGizmos()
    {
        if (interactPosition == null && !showGizmo) return;
        Vector3 origin = interactPosition.position;
        Vector3 direction = interactPosition.forward;

        bool hitInteract = Physics.Raycast(origin, direction, interactRange, interactLayer);
        Gizmos.color = hitInteract ? detectedColor : undetectedColor;

        Gizmos.DrawRay(origin, direction * interactRange);
    }
}
