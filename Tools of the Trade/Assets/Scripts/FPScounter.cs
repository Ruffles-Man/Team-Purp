using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    //private float smoothedFPS;
    float deltaTime = 0.0f;

    void Update(){
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        //float fps = 1f / Time.unscaledDeltaTime;
        //smoothedFPS = Mathf.Lerp(smoothedFPS, fps, 0.1f);
        if(fps > 60){
            fpsText.color = Color.green;
        }
        else if(fps > 30){
            fpsText.color = Color.yellow;
        }else{
            fpsText.color = Color.red;
        }
        fpsText.text = "FPS: " + Mathf.RoundToInt(fps);        
    }
}