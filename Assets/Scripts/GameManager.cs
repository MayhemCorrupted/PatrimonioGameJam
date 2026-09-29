using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Fin de Partida")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Referencias del Jugador")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteract playerInteract;

    private int objectsDelivered = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void AddDeliveredObject()
    {
        objectsDelivered++;
        if (objectsDelivered >= 4)
        {
            TriggerVictory();
        }
    }

    public void TriggerGameOver()
    {
        SetPlayerControls(false);
        EquipmentManager.Instance.Unequip();

        AudioManager.Instance.StopMusic();

        gameOverScreen.SetActive(true);
    }

    private void TriggerVictory()
    {
        SetPlayerControls(false);
        victoryScreen.SetActive(true);
    }

    public void SetPlayerControls(bool state)
    {
        if (playerMovement != null) playerMovement.enabled = state;
        if (playerInteract != null) playerInteract.enabled = state;
    }
}