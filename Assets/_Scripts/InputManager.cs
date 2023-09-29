#define USE_NEW_INPUT_SYSTEM  // Define this to use the new Input System

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Include the Input System namespace

/* input handler that seamlessly supports both the old and new Input Systems based on a preprocessor directive.
 * It ensures a singleton pattern for managing input actions, provides methods to retrieve mouse positions, check mouse button presses, 
 * and capture camera movement, rotation, and zoom inputs, all while accommodating the selected Input System.
 */
public class InputManager : MonoBehaviour
{
    // Singleton pattern: Ensure only one instance of InputManager exists
    public static InputManager Instance { get; private set; }
    private PlayerInputActions playerInputActions;  // Input actions for the player

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's already an instance of InputManager. Destroying this one.");
            Destroy(gameObject);  // Destroy this duplicate instance
            return;
        }

        Instance = this;  // Set the instance to this object
        playerInputActions = new PlayerInputActions();  // Initialize the player input actions
        playerInputActions.Player.Enable();  // Enable the player input actions
    }

    // Get the mouse position on the screen
    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();  // Use the new Input System
#else
        return Input.mousePosition;  // Use the old Input System
#endif
    }

    // Check if the mouse button was pressed this frame
    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();  // Use the new Input System
#else
        return Input.GetMouseButtonDown(0);  // Use the old Input System
#endif
    }

    // Get a 2D vector representing camera movement input
    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CamerMovement.ReadValue<Vector2>();  // Use the new Input System
#else
        Vector2 inputMoveDir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;  // Use the old Input System
#endif
    }

    // Get a float representing camera rotation input
    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CamerRotate.ReadValue<float>();  // Use the new Input System
#else
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount = -1f;
        }

        return rotateAmount;  // Use the old Input System
#endif
    }

    // Get a float representing camera zoom input
    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CamerZoom.ReadValue<float>();  // Use the new Input System
#else
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;  // Use the old Input System
#endif
    }
}
