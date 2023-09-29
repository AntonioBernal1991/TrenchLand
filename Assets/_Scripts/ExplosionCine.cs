using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ExplosionCine: Manages a cinematic explosion sequence.
public class ExplosionCine : MonoBehaviour
{
    // Lists of particle systems, game objects, and explosives to control the explosion sequence.
    public List<ParticleSystem> explosions;
    public List<GameObject> destructions;
    public List<GameObject> explosives;

    // Reference to the camera.
    public GameObject cam;

    // Event triggered when the explosion cinematic starts.
    public static event Action OnExplosion;

   
    void Start()
    {
        // Start the explosion cinematic after a delay of 2 seconds.
        StartCoroutine(ExploCinematic());
    }

   
    void Update()
    {
        // No update logic in this example.
    }

    // Coroutine for controlling the explosion cinematic.
    private IEnumerator ExploCinematic()
    {
        yield return new WaitForSeconds(2);

        
        foreach (ParticleSystem ex in explosions)
        {
            
            foreach (GameObject eps in explosives)
            {
                
                foreach (GameObject des in destructions)
                {
                    // Disable the explosive game object.
                    eps.SetActive(false);

                    // Play the explosion particle system.
                    ex.Play();

                    // Activate the destruction game object.
                    des.SetActive(true);

                    // Instantiate a grenade audio variant and destroy it after 6 seconds.
                    GameObject grenade = Instantiate(Resources.Load("GrenadeAudioVariant", typeof(GameObject))) as GameObject;
                    Destroy(grenade, 6);
                }
            }
        }

        // Trigger the OnExplosion event to notify other parts of the game.
        OnExplosion?.Invoke();

        // Activate the camera.
        cam.SetActive(true);
    }
}
