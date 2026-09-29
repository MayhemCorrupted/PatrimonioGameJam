using UnityEngine;
using UnityEngine.InputSystem;

public class LoreNote : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject loreCanvas;
    [SerializeField] private InputActionAsset inputs;

    private bool isReading = false;
    private InputAction interactAction;

    void Awake()
    {
        interactAction = inputs.FindAction("Interact");
    }
    public void Interact()
    {
        if (!isReading) OpenNote();
    }
    void Update()
    {
        if (isReading)
        {
            if (interactAction.WasPressedThisFrame() || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseNote();
            }
        }
    }
    private void OpenNote()
    {
        isReading = true;
        loreCanvas.SetActive(true);
        GameManager.Instance.SetPlayerControls(false); 
    }
    private void CloseNote()
    {
        isReading = false;
        loreCanvas.SetActive(false);
        GameManager.Instance.SetPlayerControls(true); 
    }
}