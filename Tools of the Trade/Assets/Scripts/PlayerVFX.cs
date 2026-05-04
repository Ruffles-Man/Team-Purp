using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustVFX;
    private readonly float runDustEmissionRate = 10f;

    void Awake()
    {
        SetDustVFXEmissionRate(runDustEmissionRate);
    }

    public void PlayDustVFX()
    {
        if (!dustVFX.isPlaying)
        {
            dustVFX.Play();
        }
    }

    /// <summary>
    /// Provide a value between 0 and 1 to adjust the emission rate of the dust VFX. 0 means no emission, 1 means run emission rate, and values greater than 1 will increase the emission rate proportionally.
    /// </summary>
    /// <param name="rate"></param>
    public void SetDustVFXEmissionRate(float rate)
    {
        float adjustedRate = rate * runDustEmissionRate;
        float clampedRate = Mathf.Clamp(adjustedRate, 0f, 100f);

        var emission = dustVFX.emission;
        emission.rateOverTime = clampedRate;
    }

    public void StopDustVFX()
    {
        if (dustVFX.isPlaying)
        {
            dustVFX.Stop();
        }
    }
}
