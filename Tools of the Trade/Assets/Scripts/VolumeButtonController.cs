using UnityEngine;
using TMPro;

public class VolumeButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    private int step = 0;

    // Hardcoded volume levels
    private float[] volumes = { 1f, 0.8f, 0.6f, 0.4f, 0.2f, 0f };

    void Start()
    {
        ApplyVolume();
    }

    public void ChangeVolume()
    {
        step++;

        if (step >= volumes.Length){
            step = 0;
        }

        ApplyVolume();
    }

    private void ApplyVolume()
    {
        float volume = volumes[step];

        // Apply to whole game audio
        AudioListener.volume = volume;

        if (buttonText != null){
            int percent = Mathf.RoundToInt(volume * 100);
            buttonText.text = "Volume: " + percent + "%";
        }
    }
}