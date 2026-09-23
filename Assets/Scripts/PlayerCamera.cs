using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    CinemachineInputAxisController axisController;
    [SerializeField] private float mouseSensitivity = 1f;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        axisController = GetComponentInChildren<CinemachineInputAxisController>();
    }
    private void Update()
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
