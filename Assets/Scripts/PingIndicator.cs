using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PingIndicator : MonoBehaviour
{
    [Header("Configuración de Entradas")]
    [SerializeField] private InputActionAsset inputs;
    private InputAction pingAction;

    [Header("Mecánica de Desgaste")]
    [Tooltip("Límite de usos antes de Game Over")]
    [SerializeField] private int maxUses = 4;
    private int currentUses = 0;

    [Header("Visuales del Destello (Ping)")]
    [SerializeField] private RectTransform pingVisual;
    [SerializeField] private CanvasGroup pingCanvasGroup;
    [Tooltip("El componente de imagen que cambiará de color")]
    [SerializeField] private Image pingImage;
    [SerializeField] private float expandDuration = 0.5f;
    [SerializeField] private Vector3 maxScale = new Vector3(2f, 2f, 2f);

    [Header("Colores por Desgaste")]
    [Tooltip("Colores que tomará el ping según el daño. Índice 0 = 1 uso.")]
    [SerializeField] private Color[] damageColors;

    [Header("Referencias de Entorno")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTombPlaceholder;
    void Awake()
    {
        if (inputs != null)
        {
            pingAction = inputs.FindAction("Ping");
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
        if (EquipmentManager.Instance == null || EquipmentManager.Instance.CurrentEquippedItem == null)
        {
            return;
        }

        currentUses++;

        if (currentUses > maxUses)
        {
            TriggerGameOver();
            return;
        }

        if (pingImage != null && damageColors.Length >= currentUses)
        {
            pingImage.color = damageColors[currentUses - 1];
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
        pingVisual.gameObject.SetActive(true);
        pingVisual.localScale = Vector3.zero;
        if (pingCanvasGroup != null) pingCanvasGroup.alpha = 1f;

        if (targetTombPlaceholder != null && playerTransform != null)
        {
            Vector3 direction = (targetTombPlaceholder.position - playerTransform.position).normalized;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            pingVisual.localRotation = Quaternion.Euler(0, 0, angle);
        }

        float elapsedTime = 0f;

        while (elapsedTime < expandDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / expandDuration;

            pingVisual.localScale = Vector3.Lerp(Vector3.zero, maxScale, t);

            if (pingCanvasGroup != null)
                pingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        pingVisual.gameObject.SetActive(false);
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
    }
}