using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Transform cameraTransform;
    private CharacterController controller;
    [SerializeField] private Transform orientation;
    [SerializeField] private InputActionAsset inputs;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float moveSpeed = 5;
    private float verticalVelocity;
    void Start()
    {
        cameraTransform = GetComponentInChildren<CinemachineCamera>().transform;
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        Move();
        Orientation();
    }
    private void Orientation()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Se te olvidó ingresar el componente cameraTransform");
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        if (cameraForward != Vector3.zero) orientation.rotation = Quaternion.LookRotation(cameraForward, cameraTransform.up);
    }
    private void Move()
    {
        if (controller.isGrounded) verticalVelocity = -2f;
        else verticalVelocity += gravity * Time.deltaTime;

        var moveInput = inputs.FindAction("Move").ReadValue<Vector2>();
        Vector3 forwardOrientate = new Vector3(orientation.forward.x, 0f, orientation.forward.z).normalized;
        Vector3 rightOrientate = new Vector3(orientation.right.x, 0f, orientation.right.z).normalized;
        Vector3 direction = forwardOrientate * moveInput.y + rightOrientate * moveInput.x;
        controller.Move((direction.normalized * moveSpeed + verticalVelocity * Vector3.up) * Time.deltaTime);
    }
}
