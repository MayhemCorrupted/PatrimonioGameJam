using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PingIndicator : MonoBehaviour
{
    [Header("Validación de Item")]
    [Tooltip("El ItemData exacto que debe estar equipado para que ESTE efecto se active")]
    [SerializeField] private ItemData associatedItem;

    [Header("Configuración de Entradas")]
    [SerializeField] private InputActionAsset inputs;
    private InputAction pingAction;

    [Header("Mecánica de Desgaste")]
    [Tooltip("Límite de usos antes de Game Over")]
    [SerializeField] private int maxUses = 4;
    private int currentUses = 0;

    private bool isPingActive = false;

    [Header("Visuales del Destello 3D (Ping)")]
    [Tooltip("El Transform del objeto principal que escalará en el mundo")]
    [SerializeField] private Transform pingObject;

    [Tooltip("El Transform del objeto separado que ROTARÁ apuntando a la tumba")]
    [SerializeField] private Transform pointerTransform;

    [Tooltip("El Renderer del objeto 3D para cambiar su color y transparencia")]
    [SerializeField] private Renderer pingRenderer;
    [SerializeField] private float expandDuration = 0.5f;
    [SerializeField] private Vector3 maxScale = new Vector3(2f, 2f, 2f);

    [Header("UI Adicional")]
    [Tooltip("La imagen en el Canvas que también cambiará de color")]
    [SerializeField] private Image uiPingImage;

    [Header("Colores por Desgaste")]
    [Tooltip("Colores que tomará el ping según el daño. Índice 0 = 1 uso.")]
    [SerializeField] private Color[] damageColors;

    [Header("Referencias de Entorno")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTombPlaceholder;

    private Material pingMaterial;
    private Transform mainCameraTransform;

    private float originalUIAlpha = 1f;
    void Awake()
    {
        if (inputs != null)
        {
            pingAction = inputs.FindAction("Ping");
        }

        if (pingRenderer != null)
        {
            pingMaterial = pingRenderer.material;
        }

        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        if (uiPingImage != null)
        {
            originalUIAlpha = uiPingImage.color.a;
        }
    }
    void OnEnable()
    {
        pingAction?.Enable();
    }
    void OnDisable()
    {
        pingAction?.Disable();
    }
    void Update()
    {
        if (pingAction != null && pingAction.WasPressedThisFrame())
        {
            TryActivatePing();
        }
    }
    private void TryActivatePing()
    {
        if (EquipmentManager.Instance == null || EquipmentManager.Instance.CurrentEquippedItem == null) return;
        if (EquipmentManager.Instance.CurrentEquippedItem != associatedItem) return;

        if (isPingActive) return;

        currentUses++;

        if (currentUses > maxUses)
        {
            TriggerGameOver();
            return;
        }

        if (damageColors.Length >= currentUses)
        {
            Color newColor = damageColors[currentUses - 1];

            if (pingMaterial != null)
            {
                newColor.a = 1f;
                pingMaterial.color = newColor;
            }

            if (uiPingImage != null)
            {
                Color uiColor = newColor;
                uiColor.a = originalUIAlpha;
                uiPingImage.color = uiColor;
            }
        }

        StartCoroutine(PingAnimationCoroutine());

        ItemData item = EquipmentManager.Instance.CurrentEquippedItem;
        if (AudioManager.Instance != null && item != null)
        {
            AudioManager.Instance.PlayPingSound(item.pingSound, currentUses);
        }
    }
    private IEnumerator PingAnimationCoroutine()
    {
        isPingActive = true;

        pingObject.gameObject.SetActive(true);
        pingObject.localScale = Vector3.zero;

        float elapsedTime = 0f;
        Color currentColor = pingMaterial != null ? pingMaterial.color : Color.white;

        while (elapsedTime < expandDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / expandDuration;

            if (pingObject != null && mainCameraTransform != null)
            {
                Vector3 faceDirection = pingObject.position - mainCameraTransform.position;
                if (faceDirection.sqrMagnitude > 0.001f)
                {
                    pingObject.rotation = Quaternion.LookRotation(faceDirection);
                }
            }

            if (pointerTransform != null && targetTombPlaceholder != null)
            {
                Vector3 dirToTarget = targetTombPlaceholder.position - pointerTransform.position;
                dirToTarget.y = 0f;

                if (dirToTarget.sqrMagnitude > 0.001f)
                {
                    pointerTransform.rotation = Quaternion.LookRotation(dirToTarget);
                }
            }

            pingObject.localScale = Vector3.Lerp(Vector3.zero, maxScale, t);

            if (pingMaterial != null)
            {
                currentColor.a = Mathf.Lerp(1f, 0f, t);
                pingMaterial.color = currentColor;
            }

            yield return null;
        }

        pingObject.gameObject.SetActive(false);
        isPingActive = false;
    }
    private void TriggerGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
    public void ResetPing()
    {
        currentUses = 0;

        if (damageColors.Length > 0 && pingMaterial != null)
        {
            Color matColor = damageColors[0];
            matColor.a = 0f;
            pingMaterial.color = matColor;
        }

        if (uiPingImage != null)
        {
            Color uiColor = Color.white;
            uiColor.a = originalUIAlpha;
            uiPingImage.color = uiColor;
        }
    }
}