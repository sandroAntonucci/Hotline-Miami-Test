using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;
    public float hueSpeed = 10f; // Speed of hue change (degrees per second)

    void Start()
    {

        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            StartCoroutine(ChangeHueOverTime());
        }
    }

    private IEnumerator ChangeHueOverTime()
    {
        while (true) // Infinite loop to keep changing hue
        {
            if (vignette != null)
            {
                Color currentColor = vignette.color.value;
                Color newColor = ShiftHue(currentColor, hueSpeed * Time.deltaTime);
                vignette.color.value = newColor;
            }
            yield return null; // Wait for the next frame
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


