using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

// The CheckIfAlive class is responsible for monitoring the health status of multiple entities
// and triggering a game over scenario when all of them have died. It subscribes to the OnDead
// event of each HealthSystem component associated with the entities and counts the number of deaths.
// When the specified number of deaths is reached, it initiates a scene transition to signify
// the player's loss in the game.

public class CheckIfAlive : MonoBehaviour
{
    private HealthSystem healthSystem;
    private int deathCounts; // Counter to keep track of how many health systems have reported death
    public List<HealthSystem> systems; // List of HealthSystem components to monitor
    [SerializeField] private Image transitionPanel;

    private void Awake()
    {
        // Keep this GameObject and script alive when transitioning scenes
        DontDestroyOnLoad(this.gameObject);

        // Subscribe to the OnDead event of each HealthSystem in the list
        for (int i = 0; i <= 3; i++)
        {
            systems[i].OnDead += HealthSystem_onDead;
        }
    }

    void Start()
    {
        // Initialization code can go here if needed
    }

    private void HealthSystem_onDead(object sender, EventArgs e)
    {
        // Increment the death count when a HealthSystem reports death
        deathCounts++;

        // Check if all monitored HealthSystems have reported death
        if (deathCounts >= 4)
        {
            // Start the "YouLose" coroutine when all are dead
            StartCoroutine(YouLose());
        }

        // Debug print the death count (optional)
        print(deathCounts);
    }

    IEnumerator YouLose()
    {
        // Wait for a certain amount of time (3 seconds in this case)
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(5); // Load a specific scene (scene index 5) when all are dead
    }
}