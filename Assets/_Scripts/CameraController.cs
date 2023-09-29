using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        // Get the CinemachineTransposer component from the CinemachineVirtualCamera
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        // Store the initial follow offset
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        // Handle camera movement, rotation, and zoom
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        // Get the camera movement input vector
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        // Set the camera movement speed
        float moveSpeed = 10f;

        // Calculate the movement vector based on input and camera orientation
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;

        // Update the camera position
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        // Get the camera rotation input
        Vector3 rotationVector = new Vector3(0, 0, 0);
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        // Set the camera rotation speed
        float rotationSpeed = 100f;

        // Update the camera's Euler angles to rotate it
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        // Define how much the zoom increases with input
        float zoomIncreaseAmount = 1.5f;

        // Adjust the target follow offset based on input
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount();

        // Clamp the follow offset to stay within specified bounds
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        // Set the zoom speed
        float zoomSpeed = 5f;

        // Interpolate between the current follow offset and the target offset for a smooth zoom effect
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
