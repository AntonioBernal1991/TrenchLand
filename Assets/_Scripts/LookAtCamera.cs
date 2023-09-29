using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;  // Flag to determine if the object should invert its look direction

    private Transform cameraTransform;  // Reference to the main camera's transform

    private void Awake()
    {
        cameraTransform = Camera.main.transform;  // Get a reference to the main camera's transform
    }

    // LateUpdate is called once per frame after Update, typically used for camera-related operations
    private void LateUpdate()
    {
        cameraTransform = Camera.main.transform;  // Update the camera transform reference (in case it changes)

        if (invert)
        {
            // Calculate a direction from the object to the camera and invert it,
            // then make the object look in that direction (effectively facing away from the camera)
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1);
        }
        else
        {
            // Make the object look directly at the camera
            transform.LookAt(cameraTransform);
        }
    }
}