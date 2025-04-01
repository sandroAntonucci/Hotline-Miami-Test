using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    ScoreSystem scoreSystem;
    private void Start()
    {
        scoreSystem = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<ScoreSystem>();
    }
    void Update()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = $"Score: {scoreSystem.score}";
        transform.GetChild(1).GetComponent<TMP_Text>().text = $"Multiplier: {scoreSystem.multiplier}";
        transform.GetChild(2).GetComponent<TMP_Text>().text = $"Time: {scoreSystem.multiplierTime}";
    }
}
