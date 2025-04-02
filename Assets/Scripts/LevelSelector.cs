using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class LevelSelector : MonoBehaviour
{

    [SerializeField] private int currentLevel = 1;

    [SerializeField] GameObject currentLevelButton;

    [SerializeField] private float buttonColorChangeTime = 0.5f;

    private Coroutine buttonEffectsCoroutine;

    private Image currentLevelImage;


    private void Awake()
    {
        currentLevelImage = currentLevelButton.GetComponent<Image>();
    }

    private void OnEnable()
    {
        buttonEffectsCoroutine = StartCoroutine(ButtonEffects());
    }

    private void OnDisable()
    {
        StopCoroutine(buttonEffectsCoroutine);
    }


    private IEnumerator ButtonEffects()
    {
        float alpha = 0f;
        float targetAlpha = 0.15f;

        while (true)
        {
            // Fade in
            while (alpha < targetAlpha)
            {
                alpha = Mathf.MoveTowards(alpha, targetAlpha, buttonColorChangeTime * Time.deltaTime);
                currentLevelImage.color = new Color(currentLevelImage.color.r, currentLevelImage.color.g, currentLevelImage.color.b, alpha);
                yield return null;
            }

            targetAlpha = 0f; // Switch to fade out

            // Fade out
            while (alpha > targetAlpha)
            {
                alpha = Mathf.MoveTowards(alpha, targetAlpha, buttonColorChangeTime * Time.deltaTime);
                currentLevelImage.color = new Color(currentLevelImage.color.r, currentLevelImage.color.g, currentLevelImage.color.b, alpha);
                yield return null;
            }

            targetAlpha = 0.15f; // Switch to fade in again
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level" + currentLevel);
    }

}
