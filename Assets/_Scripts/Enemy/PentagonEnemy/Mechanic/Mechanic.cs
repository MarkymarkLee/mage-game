using System.Collections;
using UnityEngine;

public abstract class Mechanic : MonoBehaviour
{
    public abstract IEnumerator Execute();
}