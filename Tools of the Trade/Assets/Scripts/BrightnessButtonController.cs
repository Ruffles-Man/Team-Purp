using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BrightnessButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Image brightnessOverlay; // full screen black/white UI image

    private int step = 2; // start at 100% (index 2)

    // 1 = normal brightness, higher = brighter, lower = darker
    private float[] brightness = { 1.4f, 1.2f, 1.0f, 0.8f, 0.6f };

    void Start()
    {
        ApplyBrightness();
    }

    public void ChangeBrightness()
    {
        step++;

        if (step >= brightness.Length)
            step = 0;

        ApplyBrightness();
    }

    private void ApplyBrightness()
    {
        float value = brightness[step];

        // Simulated brightness using overlay alpha inversion
        // (darker = more black overlay)
        if (brightnessOverlay != null)
        {
            float alpha = Mathf.Clamp01(1f - value / 1.4f);
            Color c = brightnessOverlay.color;
            c.a = alpha;
            brightnessOverlay.color = c;
        }

        // Update button text
        if (buttonText != null)
        {
            int percent = Mathf.RoundToInt(value * 100);
            buttonText.text = "Brightness: " + percent + "%";
        }
    }
}