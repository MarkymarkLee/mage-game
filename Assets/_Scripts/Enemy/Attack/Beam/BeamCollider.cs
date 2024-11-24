using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BeamCollider : MonoBehaviour
{
    public BoxCollider2D beamCollider;
    private float beamWidth;
    private Beam beam;

    void Start()
    {
        beamCollider = GetComponent<BoxCollider2D>();
        if (beamCollider == null)
        {
            beamCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        if (beamCollider == null)
        {
            beamCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        beam = GetComponentInParent<Beam>();
        beamWidth = beam.GetWidth();
    }

    void Update()
    {
        if (beam.startPoint != null && beam.endPoint != null)
        {
            Vector3 startPos = beam.startPoint.position;
            Vector3 endPos = beam.endPoint.position;

            Vector3 center = (startPos + endPos) / 2f;
            Vector3 direction = endPos - startPos;

            transform.position = center;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            beamCollider.size = new Vector2(direction.magnitude, beamWidth);
        }

        if (beam.GetIsActive())
        {
            Vibrating();
        }
    }


    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && beam.GetIsActive())
        {
            other.GetComponent<PlayerSpirit>().TakeDamage();
        }
    }

    void Vibrating()
    {
        transform.position = new Vector3(transform.position.x + Random.Range(-0.001f, 0.001f), transform.position.y + Random.Range(-0.001f, 0.001f), transform.position.z);
    }
}