using UnityEngine;
using System.Collections.Generic;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftDustVFX;
    [SerializeField] private ParticleSystem rightDustVFX;

    private int trailRequestCount = 0;
    private readonly List<TrailRenderer> trails = new();

    void Awake()
    {
        trails.AddRange(GetComponentsInChildren<TrailRenderer>());
        ReleaseAllTrailRequests();
    }

    public void ReleaseAllTrailRequests()
    {
        trailRequestCount = 0;
        EnableTrails(false);
    }

    public void RequestTrails()
    {
        trailRequestCount++;
        EnableTrails(true);
    }

    public void ReleaseTrails()
    {
        trailRequestCount = Mathf.Max(0, trailRequestCount - 1);
        if (trailRequestCount == 0)
        {
            EnableTrails(false);
        }
    }

    private void EnableTrails(bool enabled)
    {
        foreach (TrailRenderer trail in trails)
        {
            trail.emitting = enabled;
        }
    }

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
