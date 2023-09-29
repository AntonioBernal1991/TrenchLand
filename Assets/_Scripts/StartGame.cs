
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartGame : MonoBehaviour
{
    // Reference to a button GameObject that will be shown after a delay.
    public GameObject button;

    // Reference to the transition panel (likely a UI panel for scene transitions).
    [SerializeField] private Image transitionPanel;

    private void Start()
    {
        // Start the ShowButton coroutine when the script starts.
        StartCoroutine(ShowButton());
    }

    // Method called when a "Start" button is clicked.
    public void StartPlay()
    {
        // Start the SceneOne coroutine to transition to the next scene.
        StartCoroutine(SceneOne());
    }

    // Coroutine that shows the button after a delay.
    IEnumerator ShowButton()
    {
        // Wait for 3 seconds.
        yield return new WaitForSeconds(3);
        // Activate the referenced button GameObject.
        button.SetActive(true);
    }

    // Coroutine that handles scene transitions.
    IEnumerator SceneOne()
    {
        // Wait for 1 second.
        yield return new WaitForSeconds(1);

        // Activate the transition panel GameObject.
        transitionPanel.gameObject.SetActive(true);

        // Use DOTween to fade the transition panel to black over 1 second.
        transitionPanel.DOFade(1.0f, 1.0f).WaitForCompletion();

        // Wait for 1 second.
        yield return new WaitForSeconds(1);

        // Load the scene with index 1 (presumably the next scene).
        SceneManager.LoadScene(1);

        // Wait for 1 second.
        yield return new WaitForSeconds(1);
    }
}
