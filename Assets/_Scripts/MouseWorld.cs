using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*obtain the world position of the mouse pointer on the screen. 
 * It utilizes a static method GetPosition that casts a ray from the main camera to the current mouse screen position.  
 * This ray is used to determine the intersection point with objects in the specified mousePlaneLayerMask. 
 * The class effectively converts the 2D screen coordinates of the mouse cursor into 3D world coordinates and returns that point,
 * allowing other scripts to interact with the world based on the mouse's position.*/
public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;  // Reference to the single instance of the MouseWorld script

    [SerializeField] private LayerMask mousePlaneLayerMask;  // Layer mask to control what the mouse can interact with

    private void Awake()
    {
        instance = this;  // Set the static instance to this object
    }

    // Get the world position of the mouse pointer
    public static Vector3 GetPosition()
    {
        // Create a ray from the main camera to the mouse screen position and check for collisions with the specified layer mask
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Debug.Log(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask));  // Log whether a collision occurred
        return raycastHit.point;  // Return the point of collision (mouse world position)
    }
}
