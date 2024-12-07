using UnityEngine;

public class PentaVitalSideColor : MonoBehaviour
{
    // Start is called before the first frame update
    private PentagonBody enemyBody;
    private Renderer vitalSideRenderer; // Renderer for the vital side
    private BallApController ballApController; // Reference to the BallApController script
    public float curHealth;

    void Start()
    {
        enemyBody = GetComponentInParent<PentagonBody>();
        vitalSideRenderer = GetComponent<Renderer>();
        ballApController = GameObject.FindObjectOfType<BallApController>();

        UpdateVitalSideColor();
    }

    void Update()
    {
        UpdateVitalSideColor();
    }

    public void UpdatecurHealth(float health)
    {
        curHealth = health;
        UpdateVitalSideColor();
    }

    private void UpdateVitalSideColor()
    {
        float curHealth = enemyBody.currentHealth;
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
