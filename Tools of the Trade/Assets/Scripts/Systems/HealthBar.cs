using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] float updateTime = 0.2f;

    Slider slider;
    Coroutine healthVisual;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateValue(int oldHP, int currentHP, int maxHP)
    {
        if (healthVisual != null) StopCoroutine(healthVisual);
        healthVisual = StartCoroutine(UpdateHealthBar(oldHP, currentHP, maxHP));
    }

    /// <summary>
    /// Visually fills the HP bar over a short duration so changes aren't sudden and instant.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateHealthBar(int oldHP, int currentHP, int maxHP)
    {
        float oldBar = slider.value;
        float targetBar = (float)currentHP / maxHP;

        float elapsedTime = 0f;
        slider.value = oldBar;
        while (elapsedTime <= updateTime)
        {
            elapsedTime += Time.deltaTime;
            
            // (0 - 1) how much has the bar progress temporally between old and new value
            float updateTimeProgress = Mathf.Lerp(0f, 1f, elapsedTime / updateTime);
            // (oldBar - targetBar) where the bar should be
            float sliderVal = Mathf.Lerp(oldBar, targetBar, updateTimeProgress);
            slider.value = sliderVal;

            yield return null;
        }
        slider.value = targetBar;
    }
}
