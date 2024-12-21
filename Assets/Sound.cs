using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip walkClip;    // Assign walk sound file
    public AudioClip runClip;     // Assign run sound file
    public AudioClip shootClip;   // Assign shoot sound file
    public AudioClip reloadClip;  // Assign reload sound file
    public AudioClip jumpClip;

    private AudioSource audioSource;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isShooting = false;
    private bool isReloading = false;
    private bool isJumping = false;

    private float shootInterval = 0.2f; // Minimum interval between shooting sounds
    private float lastShootTime = 0f;   // Timestamp of the last sound played

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Walking sound
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (!isWalking && !isRunning) // Ensure no overlap with running sound
            {
                PlayLoopingSound(walkClip);
                isWalking = true;
            }
        }
        else
        {
            if (isWalking)
            {
                StopSound();
                isWalking = false;
            }
        }

        // Running sound
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (!isRunning)
            {
                PlayLoopingSound(runClip);
                isRunning = true;
            }
        }
        else
        {
            if (isRunning)
            {
                StopSound();
                isRunning = false;
            }
        }

        // Jumping sound
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                PlaySound(jumpClip); // Play jump sound once
                isJumping = true;    // Mark that the character is jumping
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false; // Reset jump status when the key is released
        }

        // Shooting sound
        if (Input.GetMouseButton(0)) // Left mouse button is held down
        {
            PlayShootingSound();
        }

        // Reloading sound
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading)
            {
                PlaySound(reloadClip);
                isReloading = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            if (isReloading)
            {
                StopSound();
                isReloading = false;
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.loop = false; // Ensure no looping for single sounds
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayLoopingSound(AudioClip clip)
    {
        audioSource.loop = true; // Enable looping for walking or running sounds
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void PlayShootingSound()
    {
        float currentTime = Time.time;

        // Check if enough time has passed since the last shot
        if (currentTime - lastShootTime >= shootInterval)
        {
            audioSource.PlayOneShot(shootClip); // Play the shooting sound immediately
            lastShootTime = currentTime;       // Update the last shot time
        }
    }

    void StopSound()
    {
        audioSource.loop = false; // Disable looping when stopping the sound
        audioSource.Stop();
    }
}
