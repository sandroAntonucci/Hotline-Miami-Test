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
    public float baseMultiplier = 1;
    public float multiplier;
    public float baseMultiplierTime = 3;
    public float multiplierTime;
    public float multiplierDecrease = .1f;
    public uint score; // unsigned int can hold a bigger integer number =)

    public List<string> comboList = new List<string>();
    private uint scoreToAdd;

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
    }

    private void OnDisable()
    {
        awardPointsEvent -= AwardPointsEventCallback;
    }

    public void TriggerAwardPointsEvent(int amount, string weapon = "")
    {
        awardPointsEvent?.Invoke(amount, weapon);
    }

    private void AwardPointsEventCallback(int amount, string weapon = "")
    {
        multiplier = baseMultiplier + (comboList.Count * 0.5f);
        multiplierTime = baseMultiplierTime;
        scoreToAdd = (uint)(amount * multiplier);
        score += scoreToAdd;
        Debug.Log($"Awarded {scoreToAdd} (x{multiplier}) of score, new score {score}");
        // if the weapon that was used to gather the points is different than the last one, then add it to the list.
        if (weapon != "")
        {
            if (comboList.Count == 0)
                comboList.Add(weapon);
            else if (comboList.Count > 0 && comboList[0] != weapon)
                comboList.Add(weapon);
        }

        if (multiplierCoroutine != null)
        {
            StopCoroutine(multiplierCoroutine);
        }
        multiplierCoroutine = StartCoroutine(MultiplierCoroutine());
    }

    private IEnumerator MultiplierCoroutine()
    {
        while (multiplierTime > 0)
        {
            yield return new WaitForSeconds(multiplierDecrease);
            multiplierTime -= multiplierDecrease;
            Debug.Log("Time until multiplier expires: " + multiplierTime);
        }
        yield return new WaitForEndOfFrame();
        multiplier = baseMultiplier;
        multiplierTime = 0;
        comboList.Clear();
    }
}
