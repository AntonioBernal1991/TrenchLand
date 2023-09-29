using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    // Singleton pattern: a reference to the ScreenShake instance can be accessed from other scripts.
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        // Singleton pattern: ensure there's only one instance of ScreenShake in the scene.
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScreenShake! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Get the CinemachineImpulseSource component attached to this GameObject.
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Method for triggering a screen shake with an optional intensity parameter.
    public void Shake(float intensity = 1f)
    {
        // Generate a screen shake impulse with the specified intensity.
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}