using UnityEngine;
using TMPro;

public class FPSButtonController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject fpsCounter;              // FPS text object
    public TextMeshProUGUI buttonText;         // Button's TMP text

    private bool isVisible = false;

    public void ToggleFPS()
    {
        isVisible = !isVisible;

        // Show / hide FPS counter
        fpsCounter.SetActive(isVisible);

        // Update button text
        if (isVisible)
            buttonText.text = "Show FPS: On";
        else
            buttonText.text = "Show FPS: Off";
    }

}


