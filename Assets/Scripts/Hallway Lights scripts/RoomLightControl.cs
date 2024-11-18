using System.Collections;
using UnityEngine;

public class RoomLightControl : MonoBehaviour
{
    public GameObject[] whiteLamps;    // Array of 5 white lamps
    public GameObject[] redLamps;      // Array of 6 red lamps
    public float lightChangeInterval = 1f; // Interval between light changes
    private bool lightsAreOn = true;   // Tracker for the lights' state
    private Color redColor = new Color(0.937f, 0.415f, 0.416f); // EF6A6A color in Unity's Color format

    private void Start()
    {
        InitializeLampColors(whiteLamps, Color.white); // Set initial color of white lamps to on (white)
        InitializeLampColors(redLamps, Color.black);   // Set initial color of red lamps to off (black)
    }

    // Method to set the initial emission color of lamps
    private void InitializeLampColors(GameObject[] lamps, Color emissionColor)
    {
        foreach (var lamp in lamps)
        {
            if (lamp != null)
            {
                Renderer renderer = lamp.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetColor("_EmissionColor", emissionColor);
                }

                // Find the light child object and set its intensity
                Transform lightChild = lamp.transform.Find("Light");
                if (lightChild != null)
                {
                    Light pointLight = lightChild.GetComponent<Light>();
                    if (pointLight != null)
                    {
                        pointLight.intensity = (emissionColor != Color.black) ? 0.7f : 0; // Set intensity based on color (on/off)
                    }
                }
            }
        }
    }

    // Method to turn off white lamps and turn on red lamps
    public void TurnOffWhiteAndTurnOnRed()
    {
        if (lightsAreOn)
        {
            StartCoroutine(TurnOffWhiteLampsAndActivateRed());
            lightsAreOn = false; // Prevent further activation
        }
    }

    private IEnumerator TurnOffWhiteLampsAndActivateRed()
    {
        // Turn off white lamps one by one
        for (int i = 0; i < whiteLamps.Length; i++)
        {
            // Turn off white lamp if it exists
            if (i < whiteLamps.Length && whiteLamps[i] != null)
            {
                Transform lightChild = whiteLamps[i].transform.Find("Light");
                if (lightChild != null)
                {
                    lightChild.gameObject.SetActive(false); // Turn off the light child object
                }

                Renderer renderer = whiteLamps[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetColor("_EmissionColor", Color.black); // Set emission color to black
                }

                AudioSource audioSource = whiteLamps[i].GetComponent<AudioSource>();
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play(); // Play sound for each white lamp as it turns off
                }
            }

            yield return new WaitForSeconds(lightChangeInterval); // Wait before next lamp turns off
        }

        // Play the sound from the 5th red lamp (index 4) after all white lamps are turned off
        if (redLamps.Length > 4 && redLamps[4] != null)
        {
            AudioSource redLampAudioSource = redLamps[4].GetComponent<AudioSource>(); // Get AudioSource of the 5th red lamp
            if (redLampAudioSource != null && !redLampAudioSource.isPlaying)
            {
                redLampAudioSource.Play(); // Play the sound of the 5th red lamp
            }
        }

        // Now turn on all red lamps immediately after all white lamps are turned off
        for (int i = 0; i < redLamps.Length; i++)
        {
            // Turn on red lamp if it exists
            if (i < redLamps.Length && redLamps[i] != null)
            {
                Transform lightChild = redLamps[i].transform.Find("Light");
                if (lightChild != null)
                {
                    Light pointLight = lightChild.GetComponent<Light>();
                    if (pointLight != null)
                    {
                        pointLight.intensity = 0.7f; // Turn on the light by setting intensity to 0.7
                        pointLight.color = redColor; // Set the light color to red (EF6A6A)
                    }
                }

                Renderer renderer = redLamps[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetColor("_EmissionColor", redColor); // Set emission color to red (EF6A6A)
                }
            }
        }
    }

    // New method to turn on the white lamps
    public void TurnOnWhiteLamps()
    {
        StartCoroutine(TurnOnWhiteLampsSequentially());
    }

    private IEnumerator TurnOnWhiteLampsSequentially()
    {
        for (int i = 0; i < whiteLamps.Length; i++)
        {
            if (whiteLamps[i] != null)
            {
                Transform lightChild = whiteLamps[i].transform.Find("Light");
                if (lightChild != null)
                {
                    lightChild.gameObject.SetActive(true); // Turn on the light child object
                }

                Renderer renderer = whiteLamps[i].GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetColor("_EmissionColor", Color.white); // Set emission color to white
                }

                AudioSource audioSource = whiteLamps[i].GetComponent<AudioSource>();
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play(); // Play sound for each white lamp as it turns on
                }
            }

            yield return new WaitForSeconds(lightChangeInterval); // Wait before the next lamp turns on
        }
    }

    public void TurnOffRedLamps()
    {
        // Turn off all red lamps at once
        foreach (var redLamp in redLamps)
        {
            if (redLamp != null)
            {
                Transform lightChild = redLamp.transform.Find("Light");
                if (lightChild != null)
                {
                    lightChild.gameObject.SetActive(false); // Turn off the light child object
                }

                Renderer renderer = redLamp.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.SetColor("_EmissionColor", Color.black); // Set emission color to black
                }
            }
        }

        // Play the sound from the 5th red lamp (index 4) after all red lamps are turned off
        if (redLamps.Length > 4 && redLamps[4] != null)
        {
            AudioSource redLampAudioSource = redLamps[4].GetComponent<AudioSource>(); // Get AudioSource of the 5th red lamp
            if (redLampAudioSource != null && !redLampAudioSource.isPlaying)
            {
                redLampAudioSource.Play(); // Play the sound of the 5th red lamp
            }
        }
    }

    // Detect when the player enters the first collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the collider (adjust the "Player" tag as needed)
        if (other.CompareTag("Player"))
        {
            TurnOffWhiteAndTurnOnRed(); // Call the method to turn off lights and change to red lamps
        }
    }
}
