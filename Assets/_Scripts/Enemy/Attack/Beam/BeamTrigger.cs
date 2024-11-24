using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamTriggerGen : MonoBehaviour
{
    public float duration = 1f;
    public Material beamMaterial;


    private float beamWidth;
    private Beam beam;
    private LineRenderer beamRender;
    private float timer = 0;

    void Start()
    {
        beam = GetComponentInParent<Beam>();
        beamWidth = beam.GetWidth();
        duration = beam.delayBeforeDamage;
        CreateBeamMesh();
    }

    void CreateBeamMesh()
    {
        beamRender = GetComponent<LineRenderer>();
        if (beamRender == null)
        {
            beamRender = gameObject.AddComponent<LineRenderer>();
        }

        beamRender.startWidth = 0f;
        beamRender.endWidth = 0f;
        beamRender.material = beamMaterial;
    }

    void Update()
    {
        beamRender.SetPosition (0, beam.startPoint.position);
		beamRender.SetPosition (1, beam.endPoint.position);
        if (timer <= duration)
        {
            timer += Time.deltaTime;
            float scaleFactor = timer / duration;
            float currentWidth = Mathf.Lerp(0, beamWidth, scaleFactor);
            beamRender.startWidth = currentWidth;
            beamRender.endWidth = currentWidth;
        }
        else
        {
            beamRender.startWidth = beamWidth;
            beamRender.endWidth = beamWidth;
            Destroy(gameObject);
        }
    }
}
