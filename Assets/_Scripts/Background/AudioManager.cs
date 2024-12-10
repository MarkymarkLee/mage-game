using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip PlayerDamage;
    public AudioClip PlayerDeath;
    public AudioClip PlayerTeleport;
    public AudioClip PlayerDash;
    public AudioClip PlayerShoot;
    public AudioClip EnemyDamage;
    public AudioClip EnemyDeath;
    public AudioClip TriangleShoot;


    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySfx(AudioClip sfx)
    {
        if(sfx != null){
            audioSource.PlayOneShot(sfx);
        }
    }
}
