using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class FontEffects : MonoBehaviour
{

    private TextMeshProUGUI text;

    public Color startColor, endColor;

    public int hueSpeed = 10; 

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(ChangeHueOverTime());    
    }

    private IEnumerator ChangeHueOverTime()
    {
        while (true) // Infinite loop to keep changing hue
        {

            for (int i = 0; i < 360; i += hueSpeed)
            {
                text.color = ShiftHue(startColor, i);
                yield return new WaitForSeconds(0.1f);
            }

            for (int i = 360; i > 0; i -= hueSpeed)
            {
                text.color = ShiftHue(startColor, i);
                yield return new WaitForSeconds(0.1f);
            }


        }
    }

    private Color ShiftHue(Color color, float hueShift)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        h += hueShift / 360f; // Convert degrees to [0,1] range
        if (h > 1) h -= 1;
        if (h < 0) h += 1;
        return Color.HSVToRGB(h, s, v);
    }

}
