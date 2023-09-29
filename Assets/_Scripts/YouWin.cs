using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The "YouWin" class is responsible for displaying a "You Win" message and transitioning to another scene.
// It activates the "youWin" GameObject after a delay and, after another delay, loads a specified scene.
// The class also checks for the "Escape" key press but does not perform any action.
public class YouWin : MonoBehaviour
{
    public GameObject youWin; // Reference to the "youWin" GameObject to be activated.

    // Start is called before the first frame update.
    void Start()
    {
        StartCoroutine(ShowText());
    }

    private void Update()
    {
        // Check if the "Escape" key is pressed, but no specific action is performed in this implementation.
        if (Input.GetKey("escape"))
        {
            // Handle the "Escape" key press (no action implemented here).
        }
    }

    // Coroutine to display the "You Win" text and transition to another scene.
    public IEnumerator ShowText()
    {
        yield return new WaitForSeconds(2.8f); // Wait for a specified duration before showing the text.
        youWin.SetActive(true); // Activate the "youWin" GameObject.
        yield return new WaitForSeconds(3); // Wait for a specified duration before transitioning to another scene.
        SceneManager.LoadScene(4); // Load the scene with the index 4 (you can change this index as needed).

        // The following lines are commented out and can be used to quit the application in the editor:
        // UnityEditor.EditorApplication.isPlaying = false;
        // Application.Quit();
    }
}

