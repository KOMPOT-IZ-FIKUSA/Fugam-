using UnityEngine;

public class GeneratorControl : MonoBehaviour
{
    public RoomLightControl lightController;   // Reference to the RoomLightControl script that controls the lamps
    public Animator generatorAnimator;          // Reference to the generator's animator to play the animation
    private bool generatorOn = false;           // Track if the generator is turned on

    private AudioSource generatorAudioSource;   // The audio source to play the generator sound
    private AudioSource spotlightAudioSource;   // The audio source to play the spotlight sound when lights turn on

    private bool playerInRange = false;         // Whether the player is within the generator's collider

    void Start()
    {
        // Get the AudioSource components attached to the generator and spotlight (if available)
        generatorAudioSource = GetComponent<AudioSource>();
        if (lightController != null && lightController.redLamps.Length > 4)
        {
            spotlightAudioSource = lightController.redLamps[4].GetComponent<AudioSource>(); // Assuming the spotlight sound is on the 5th red lamp
        }
    }

    void Update()
    {
        // Detect the player pressing the 'E' key while near the generator, but only if the generator is off
        if (Input.GetKeyDown(KeyCode.E) && !generatorOn && playerInRange)
        {
            TurnOnGenerator();
        }
    }

    // Detect when the player enters the generator's trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            playerInRange = true; // Set playerInRange to true when the player enters the collider

            // Only show the message if the generator is off
            if (!generatorOn)
            {
                Debug.Log("PRESS E to turn on the lights");
            }
        }
    }

    // Detect when the player exits the generator's trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            playerInRange = false; // Set playerInRange to false when the player leaves the collider
        }
    }

    // Method to turn on the generator and activate lights
    private void TurnOnGenerator()
    {
        // Set generator as on
        generatorOn = true;

        // Play the generator sound (if it has an AudioSource)
        if (generatorAudioSource != null && !generatorAudioSource.isPlaying)
        {
            generatorAudioSource.Play();  // Play the sound of the generator turning on
        }

        // Play the generator animation
        if (generatorAnimator != null)
        {
            generatorAnimator.SetTrigger("Activate"); // Trigger the generator turning on animation
        }

        // Turn on the lights and play the spotlight sound from the 5th red lamp
        if (lightController != null)
        {
            lightController.TurnOnWhiteLamps();
            lightController.TurnOffRedLamps();// This method should now control only the white lamps
            if (spotlightAudioSource != null && !spotlightAudioSource.isPlaying)
            {
                spotlightAudioSource.Play();  // Play the spotlight sound once
            }
        }
    }
}
