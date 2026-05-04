using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftDustVFX;
    [SerializeField] private ParticleSystem rightDustVFX;

    public void PlayLeftDustVFX()
    {
        leftDustVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        leftDustVFX.Play();
    }

    public void PlayRightDustVFX()
    {
        rightDustVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        rightDustVFX.Play();
    }
}
