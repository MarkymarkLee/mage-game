using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip firstSong;      // The song to play first
    public AudioClip loopingSong;    // The song to loop afterward

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Ensure AudioSource is set up
        if (audioSource == null || firstSong == null || loopingSong == null)
        {
            Debug.LogError("AudioSource or AudioClips are not set up correctly!");
            return;
        }

        // Start the music sequence
        StartCoroutine(PlayMusicSequence());
    }

    private IEnumerator PlayMusicSequence()
    {
        // Play the first song
        audioSource.clip = firstSong;
        audioSource.loop = false;
        audioSource.Play();

        // Wait until the first song is almost done
        while (audioSource.isPlaying)
        {
            yield return null; // Wait for the frame and check again
        }

        // Transition to the looping song
        PlayLoopingSong();
    }

    private void PlayLoopingSong()
    {
        // Set up and play the looping song
        audioSource.clip = loopingSong;
        audioSource.loop = true;
        audioSource.Play();
    }
}

