using UnityEngine;

public class VitalSideColor : MonoBehaviour
{
    // Start is called before the first frame update
    public bool autoUpdate = true;
    private EnemyBody enemyBody;
    private Renderer vitalSideRenderer; // Renderer for the vital side
    private BallApController ballApController; // Reference to the BallApController script

    void Start()
    {
        enemyBody = GetComponentInParent<EnemyBody>();
        vitalSideRenderer = GetComponent<Renderer>();
        ballApController = GameObject.FindObjectOfType<BallApController>();
        float curHealth = enemyBody.currentHealth;
        UpdateVitalSideColor(curHealth);
    }

    void Update()
    {
        if (autoUpdate)
        {
            float curHealth = enemyBody.currentHealth;
            UpdateVitalSideColor(curHealth);
        }
    }

    public void UpdateVitalSideColor(float curHealth)
    {
        if (curHealth <= ballApController.ballDamage_1)
        {
            vitalSideRenderer.material.color = ballApController.firstColor;
        }
        else if (curHealth <= ballApController.ballDamage_2)
        {
            vitalSideRenderer.material.color = ballApController.secondColor;
        }
        else
        {
            vitalSideRenderer.material.color = ballApController.thirdColor;
        }
    }
}
