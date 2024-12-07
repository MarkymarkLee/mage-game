using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
public class Beam : MonoBehaviour
{
    public float delayBeforeDamage = 1.0f;
    public float activeTime = 14.0f;
    public int damage = 10;
    
    
    public Transform startPoint;
    public Transform endPoint;

    public Material beamMaterial;
    public Material activeMaterial;

    public AudioClip activationSound;
    public AudioClip activatedSound;
    
    private float beamWidth = 1f;
    private LineRenderer beamRender;
    private bool isActive = false;
    private AudioSource audioSource;

    void Start()
    {
        beamRender = GetComponent<LineRenderer>();
        if (beamRender == null)
        {
            beamRender = gameObject.AddComponent<LineRenderer>();
        }
        audioSource = GetComponent<AudioSource>();
        CreateBeamMesh();
        StartCoroutine(ActivateBeam());
    }
    
    public void SetWidth(float width)
    {
        beamWidth = width;
    }

    public float GetWidth()
    {
        return beamWidth;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    void Update ()
    {
        if (startPoint != null)
        {
            beamRender.SetPosition(0, startPoint.position);
        }
        if (endPoint != null)
        {
            beamRender.SetPosition(1, endPoint.position);
        }
    }

    void CreateBeamMesh()
    {
        beamRender.startWidth = beamWidth;
        beamRender.endWidth = beamWidth;
        beamRender.material = beamMaterial;
    }

    // Coroutine to handle AoE activation timing
    IEnumerator ActivateBeam()
    {
        // Play activation sound
        if (activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }
        
        yield return new WaitForSeconds(delayBeforeDamage);
        isActive = true;

        beamRender.material = activeMaterial;

        // Play activated sound
        if (activatedSound != null)
        {
            StartCoroutine(FadeOutAndPlayActivatedSound());
        }
        yield return new WaitForSeconds(activeTime);

        // Stop activated sound
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        Destroy(gameObject); // Remove AoE after activation
    }

    IEnumerator FadeOutAndPlayActivatedSound()
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= Time.deltaTime / 0.1f;
            yield return null;
        }
        audioSource.Stop();

        audioSource.clip = activatedSound;
        audioSource.loop = true;
        audioSource.volume = 0.0f;
        audioSource.Play();

        while (audioSource.volume < 0.3f)
        {
            audioSource.volume += Time.deltaTime / 0.1f;
            yield return null;
        }
        audioSource.volume = 0.3f;
    }
}
