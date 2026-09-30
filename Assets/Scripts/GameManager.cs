using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configuración de UI (Paneles)")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Referencias del Jugador")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Progreso del Juego")]
    private int objectsDelivered = 0;
    private const int OBJECTS_TO_WIN = 4;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PauseGame();
        ShowPanel(mainMenuPanel);
    }
    public void StartGame()
    {
        HideAllPanels();
        ResumeGame();
        SetPlayerControls(true);
    }
    public void AddDeliveredObject()
    {
        objectsDelivered++;

        if (objectsDelivered >= OBJECTS_TO_WIN)
        {
            TriggerVictory();
        }
    }
    public void TriggerGameOver()
    {
        Debug.Log("¡Se ejecutó TriggerGameOver!"); 

        SetPlayerControls(false);
        PauseGame();
        ShowPanel(gameOverPanel);
        if (EquipmentManager.Instance != null) EquipmentManager.Instance.Unequip();
        if (AudioManager.Instance != null) AudioManager.Instance.StopMusic();
    }
    private void TriggerVictory()
    {
        SetPlayerControls(false);
        ShowPanel(victoryPanel);
        PauseGame();
    }
    public void QuickRestart()
    {
        objectsDelivered = 0;
        if (EquipmentManager.Instance != null) EquipmentManager.Instance.Unequip();
        if (InventoryManager.Instance != null) InventoryManager.Instance.ClearInventory();

        if (playerTransform != null && spawnPoint != null)
        {
            playerTransform.position = spawnPoint.position;
        }
        StartGame();
    }   

    public void ReturnToMainMenu()
    {
        ResumeGame(); 
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetPlayerControls(bool state)
    {
        if (playerMovement != null) playerMovement.enabled = state;
        if (playerInteract != null) playerInteract.enabled = state;
    }
    private void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void ShowPanel(GameObject panelToShow)
    {
        HideAllPanels();
        if (panelToShow != null) panelToShow.SetActive(true);
    }
    private void HideAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }
}