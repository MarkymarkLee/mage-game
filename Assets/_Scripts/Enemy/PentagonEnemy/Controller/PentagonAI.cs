using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonAI : MonoBehaviour
{
    public Transform player;
    public PentagonBody pentagonBody;
    public float enrageDuration = 10f;

    public List<Mechanic> mechanics;

    public bool isAttacking = false;
    private bool entryFinish = false;
    private bool isWeaken = false;
    private bool debut = false;
    private bool enrage = false;
    private int polygonSides = 6;
    private float polygonRadius = 2.0f;
    private int phase;
    private int enrageTimes = 0;
    private bool[] phaseExecuted;

    void Start()
    {
        // Initialize mechanics list if not already initialized
        if (mechanics == null)
        {
            mechanics = new List<Mechanic>();
        }

        phaseExecuted = new bool[mechanics.Count];
        
        phase = pentagonBody.phase;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        StartCoroutine(EnrageTimer(enrageDuration));
    }
    
    public void setisWeaken(bool value)
    {
        isWeaken = value;
    }

    public bool getisWeaken()
    {
        return isWeaken;
    }
    
    public void setisAttacking(bool value)
    {
        isAttacking = value;
    }

    public bool getisAttacking()
    {
        return isAttacking;
    }

    public void setEntryFinish(bool value)
    {
        entryFinish = value;
    }

    public int getEnrageTimes()
    {
        return enrageTimes;
    }

    public int getPolygonSides()
    {
        return polygonSides;
    }

    public float getPolygonRadius()
    {
        return polygonRadius;
    }

    void Update()
    {
        if (entryFinish && !debut)
        {
            debut = true;
            transform.localPosition = new Vector3(0, 0, 0);
        }
        
        phase = pentagonBody.phase;
        if (!isAttacking)
        {
            pentagonBody.switchLRSwitch();
            if (!entryFinish)
            {
                Begining();
            }
            else if (enrage || phase == 3)
            {
                EnragePhase();
            }
            else
            {
                StartPhase(phase);
            }
        }
    }

    public void Begining()
    {
        Debug.Log("Boss is in Begining: Debut.");
        isAttacking = true;
        StartCoroutine(mechanics[4].Execute());
    }

    public void StartPhase(int phase)
    {
        if (phase >= 0 && phase < mechanics.Count)
        {
            isAttacking = true;
            if (!phaseExecuted[phase])
            {
                phaseExecuted[phase] = true;
                StartCoroutine(mechanics[phase].Execute());
            }
            else
            {
                int randomPhase = Random.Range(0, phase + 1);
                StartCoroutine(mechanics[randomPhase].Execute());
            }
        }
        else
        {
            Debug.LogError("Invalid phase index: " + phase);
        }
    }

    public void EnragePhase()
    {
        Debug.Log("Boss has entered Phase 3: Enrage mode activated! Massive damage incoming.");
        // if (enrageTimes < 3)
        // {
        //     enrageTimes++;
        // }
        isAttacking = true;
        StartCoroutine(mechanics[3].Execute());
    }

    private IEnumerator EnrageTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        // enrage = true;
        Debug.Log("Boss is now enraged!");
    }

}