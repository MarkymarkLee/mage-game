using UnityEngine;

public class VitalSideColor : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemyBody enemyBody;
    private Renderer vitalSideRenderer; // Renderer for the vital side
    private BallApController ballApController; // Reference to the BallApController script

    void Start()
    {
        enemyBody = GetComponentInParent<EnemyBody>();
        vitalSideRenderer = GetComponent<Renderer>();
        ballApController = GameObject.FindObjectOfType<BallApController>();

        UpdateVitalSideColor();
    }

    void Update()
    {
        UpdateVitalSideColor();
    }

    private void UpdateVitalSideColor()
    {
        float curHealth = enemyBody.currentHealth;
        if(curHealth <= ballApController.ballDamage_1)
        {
            vitalSideRenderer.material.color = ballApController.firstColor;
        }
        else if(curHealth <= ballApController.ballDamage_2)
        {
            vitalSideRenderer.material.color = ballApController.secondColor;
        }
        else
        {
            vitalSideRenderer.material.color = ballApController.thirdColor;
        }
    }
}
