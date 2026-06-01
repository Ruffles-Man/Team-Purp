using System;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    async Task Fade(float targetAlpha)
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
        canvasGroup.alpha = targetAlpha;
    }

    public async Task FadeIn()
    {
        await Fade(0f);
    }

    public async Task FadeOut()
    {
        await Fade(1f);
    }
}
