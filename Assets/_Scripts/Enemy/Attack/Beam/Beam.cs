using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Beam : MonoBehaviour
{
    public float delayBeforeDamage = 1.0f;
    public float activeTime = 14.0f;
    public int damage = 10;
    
    
    public Transform startPoint;
    public Transform endPoint;

    public Material beamMaterial;
    public Material activeMaterial;
    
    private float beamWidth = 1f;
    private LineRenderer beamRender;
    private bool isActive = false;

    void Start()
    {
        beamRender = GetComponent<LineRenderer>();
        if (beamRender == null)
        {
            beamRender = gameObject.AddComponent<LineRenderer>();
        }
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
        // Display warning for 1 second
        yield return new WaitForSeconds(delayBeforeDamage);
        isActive = true;

        beamRender.material = activeMaterial;
        yield return new WaitForSeconds(activeTime);
        Destroy(gameObject); // Remove AoE after activation
    }
}
