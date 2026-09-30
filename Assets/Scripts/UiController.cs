using UnityEngine;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        PauseGame();
        ShowPanel(mainMenuPanel);
    }

    public void StartGame()
    {
        HideAllPanels();
        ResumeGame();
    }

    public void ShowVictory()
    {
        PauseGame();
        ShowPanel(victoryPanel);
    }

    public void ShowGameOver()
    {
        PauseGame();
        ShowPanel(gameOverPanel);
    }

    public void QuickRestart()
    {
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
    private void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
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
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
