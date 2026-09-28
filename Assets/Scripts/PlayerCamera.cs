using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    CinemachineInputAxisController axisController;
    [SerializeField] private float mouseSensitivity = 1f;
    void Awake()
    {
        axisController = GetComponentInChildren<CinemachineInputAxisController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        SetSensibility();
    }
    private void SetSensibility()
    {
        foreach (var controller in axisController.Controllers)
        {
            if (controller.Name == "Look X (Pan)")
            {
                controller.Input.Gain = mouseSensitivity;
            }
            else if (controller.Name == "Look Y (Tilt)")
            {
                controller.Input.Gain = -mouseSensitivity;
            }
        }
    }
}
