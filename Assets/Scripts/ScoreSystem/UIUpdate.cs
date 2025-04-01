using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdate : MonoBehaviour
{
    void Update()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = $"Score: {ScoreSystem.Instance.score}";
        transform.GetChild(1).GetComponent<TMP_Text>().text = $"Multiplier: {ScoreSystem.Instance.multiplier}";
        transform.GetChild(2).GetComponent<TMP_Text>().text = $"Time: {ScoreSystem.Instance.multiplierTime}";
        transform.GetChild(3).GetComponent<TMP_Text>().text = $"MinScoreForA: {ScoreSystem.Instance.minimumScoreForA}";
        transform.GetChild(4).GetComponent<TMP_Text>().text = $"Letter: {ScoreSystem.Instance.GetScoreLetter()}";
    }
}
