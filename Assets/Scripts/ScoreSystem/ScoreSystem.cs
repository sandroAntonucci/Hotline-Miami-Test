using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreSystem : MonoBehaviour
{
    // Vars
    public int baseMultiplier = 1;
    public int multiplier;
    public float baseMultiplierTime = 3;
    public float multiplierTime;
    public float multiplierDecrease = .1f;
    public uint score; // unsigned int can hold a bigger integer number =)

    public List<string> comboList = new List<string>();

    // Coroutines
    public Coroutine multiplierCoroutine;

    // Events
    public delegate void AwardPointsHandler(int amount, string weapon = "");

    public event AwardPointsHandler awardPointsEvent;

    private void Awake()
    {
        multiplier = baseMultiplier;
    }

    private void OnEnable()
    {
        awardPointsEvent += AwardPointsEventCallback;
        multiplierCoroutine = StartCoroutine(MultiplierCoroutine());
    }

    private void OnDisable()
    {
        awardPointsEvent -= AwardPointsEventCallback;
        StopCoroutine(multiplierCoroutine);
    }

    private void AwardPointsEventCallback(int amount, string weapon = "")
    {
        // TODO: Hay que calcular el multiplicador segun el combo.
        multiplierTime = baseMultiplierTime;
        score += (uint)(amount * multiplier);
        Debug.Log($"Awarded {amount} (x{multiplier}) of score, new score {score}");
        // if the weapon that was used to gather the points is different than the last one, then add it to the list.
        if (weapon != "")
        {
            if (comboList.Count == 0)
                comboList.Add(weapon);
            else if (comboList.Count > 0 && comboList[0] != weapon)
                comboList.Add(weapon);
        }
    }

    private IEnumerator MultiplierCoroutine()
    {
        while (true)
        {
            while (multiplierTime > 0)
            {
                yield return new WaitForSeconds(multiplierDecrease);
                multiplierTime -= multiplierDecrease;
                Debug.Log("Time until multiplier expires: " + multiplierTime);
            }
            yield return new WaitForEndOfFrame();
            multiplier = baseMultiplier;
        }
    }
}
