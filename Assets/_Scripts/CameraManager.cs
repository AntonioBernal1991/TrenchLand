using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraObject;

    private void Start()
    {
        // Subscribe to events when any action is started or completed
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        // Initially, hide the action camera
        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        // Activate the action camera GameObject
        actionCameraObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        // Deactivate the action camera GameObject
        actionCameraObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        // Handle the event when any action is started
        switch (sender)
        {
            case ShootAction shootAction:
                // Get the shooter and target units from the ShootAction
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                // Define camera offsets for positioning
                Vector3 cameraCharacterHeight = Vector3.up * 2.3f;
                Vector3 cameraCharacterBack = Vector3.back * 4f;
                Vector3 cameraCharacterRight = Vector3.left * 1.5f;

                // Calculate the direction from shooter to target
                Vector3 shootDir = (targetUnit.GetWorldposition() - shooterUnit.GetWorldposition()).normalized;

                // Define a shoulder offset for camera
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                // Calculate the final camera position
                Vector3 actionCameraPosition =
                   shooterUnit.GetWorldposition() +
                   cameraCharacterHeight +
                   //cameraCharacterBack +
                   //cameraCharacterRight + 
                   shoulderOffset +
                   (shootDir * -1);

                // Set the position and look-at direction for the action camera
                actionCameraObject.transform.position = actionCameraPosition;
                actionCameraObject.transform.LookAt(targetUnit.GetWorldposition() + cameraCharacterHeight);

                // Show the action camera
                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        // Handle the event when any action is completed
        switch (sender)
        {
            case ShootAction shootAction:
                // Hide the action camera when the ShootAction is completed
                HideActionCamera();
                break;
        }
    }
}
