using UnityEngine;
using UnityEngine.UI;
public class AnimationManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Image handPoint, pagePoint;

    [Header("Animators")]
    [SerializeField] Animator mainAnimator;
    [SerializeField] Animator pageAnimator;

    [Header("Player References")]
    [SerializeField] CharacterController playerController;

    [Header("Tilt Effect References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float tiltMultiplier = 2.5f;
    [SerializeField] float maxTiltOffset = 150f;
    [SerializeField] float lookDownAngle = 45f;

    private RectTransform handRect;
    private RectTransform pageRect;
    private Vector2 initialHandPosition;
    private Vector2 initialPagePosition;

    private readonly int moveHash = Animator.StringToHash("move");
    private readonly int[] itemHashes = new int[]
    {
        Animator.StringToHash("hasAzulejo"),
        Animator.StringToHash("hasMoneda"),
        Animator.StringToHash("hasPala"),
        Animator.StringToHash("hasPartitura")
    };

    void Start()
    {
        if (handPoint != null)
        {
            handRect = handPoint.GetComponent<RectTransform>();
            initialHandPosition = handRect.anchoredPosition;
        }
        if (pagePoint != null)
        {
            pageRect = pagePoint.GetComponent<RectTransform>();
            initialPagePosition = pageRect.anchoredPosition;
        }
    }

    void Update()
    {
        HandleEquipmentAndUI();
        HandleMovementState();
        HandleUITiltEffect();
    }

    private void HandleEquipmentAndUI()
    {
        if (EquipmentManager.Instance == null) return;

        ItemData equippedItem = EquipmentManager.Instance.CurrentEquippedItem;
        bool hasItem = equippedItem != null;

        if (handPoint != null) handPoint.enabled = hasItem;
        if (pagePoint != null) pagePoint.enabled = hasItem;

        for (int i = 0; i < itemHashes.Length; i++)
        {
            bool isThisItemEquipped = false;

            if (hasItem && equippedItem.id == (i + 1))
            {
                isThisItemEquipped = true;
            }

            if (pageAnimator != null)
            {
                pageAnimator.SetBool(itemHashes[i], isThisItemEquipped);
            }
        }
    }

    private void HandleMovementState()
    {
        if (playerController == null || mainAnimator == null) return;

        Vector3 horizontalVelocity = new Vector3(playerController.velocity.x, 0, playerController.velocity.z);
        int moveState = horizontalVelocity.magnitude > 0.1f ? 1 : 0;

        if (mainAnimator != null) mainAnimator.SetInteger(moveHash, moveState);
        if (pageAnimator != null) pageAnimator.SetInteger(moveHash, moveState);
    }

    private void HandleUITiltEffect()
    {   
        if (cameraTransform == null) return;

        float pitch = cameraTransform.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        float dropAmount = (lookDownAngle - Mathf.Clamp(pitch, -90f, lookDownAngle)) * tiltMultiplier;
        float yOffset = Mathf.Clamp(dropAmount, 0f, maxTiltOffset);

        if (handRect != null) handRect.anchoredPosition = new Vector2(initialHandPosition.x, initialHandPosition.y - yOffset);
        if (pageRect != null) pageRect.anchoredPosition = new Vector2(initialPagePosition.x, initialPagePosition.y - yOffset);
    }
}
