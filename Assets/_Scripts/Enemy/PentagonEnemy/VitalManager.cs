using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalManager : MonoBehaviour
{
    public void UpdatecurHealth(int currentHealth) {
        foreach(var vital in GetComponentsInChildren<PentaVitalSideColor>()) {
            vital.UpdatecurHealth(currentHealth);
        }
    }
}
