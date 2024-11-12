using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackLaughingClownScript : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    // Drag the second AudioClip here in the Inspector
    public AudioSource secondSoundClip;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component missing. Please attach an AudioSource to play sound.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the trigger
        if (other.CompareTag("Player") && !hasPlayed && audioSource != null)
        {
            // Play the first audio clip and set hasPlayed to true
            audioSource.Play();
            hasPlayed = true;

            // Start the coroutine to deactivate the GameObject and play the second sound after 8 seconds
            StartCoroutine(DeactivateAfterDelay(8f));
        }
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Play the second sound clip if assigned
        if (secondSoundClip != null)
        {
            audioSource = secondSoundClip;
            audioSource.Play();
        }

        // Deactivate or destroy the GameObject
        Destroy(gameObject);
    }
}
