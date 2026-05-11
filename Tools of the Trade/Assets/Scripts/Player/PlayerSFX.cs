using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip movementClip;
    [SerializeField] float movementSFXDelay = 0.4f; // Minimum delay between movement SFX in seconds
    private float elapsedMovementSFXTime = 0f; // Time elapsed since the last movement SFX was played
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip landClip;
    [SerializeField] AudioClip enterExitCrouchClip;
    [SerializeField] float CrouchSFXDelay = 0.3f; // Minimum delay between crouch SFX in seconds
    private float elapsedCrouchSFXTime = 0f; // Time elapsed since the last crouch SFX was played
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip attack1Clip;
    [SerializeField] AudioClip attack2Clip;
    [SerializeField] AudioClip attack3Clip;

    void Update()
    {
        elapsedMovementSFXTime += Time.deltaTime; // Update the elapsed time for movement SFX delay
        elapsedCrouchSFXTime += Time.deltaTime; // Update the elapsed time for crouch SFX delay
    }

    public void PlayMovementSFX()
    {
        if (elapsedMovementSFXTime >= movementSFXDelay)
        {
            audioSource.PlayOneShot(movementClip);
            elapsedMovementSFXTime = 0f;
        }
    }

    public void PlayJumpSFX()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayLandSFX()
    {
        audioSource.PlayOneShot(landClip);
    }

    public void PlayCrouchSFX()
    {
        if (elapsedCrouchSFXTime >= CrouchSFXDelay)
        {
            audioSource.PlayOneShot(enterExitCrouchClip);
            elapsedCrouchSFXTime = 0f;
        }
    }

    public void PlayDashSFX()
    {
        audioSource.PlayOneShot(dashClip);
    }

    public void PlayAttackSFX(int attackNumber)
    {
        switch (attackNumber)
        {
            case 1:
                audioSource.PlayOneShot(attack1Clip);
                break;
            case 2:
                audioSource.PlayOneShot(attack2Clip);
                break;
            case 3:
                audioSource.PlayOneShot(attack3Clip);
                break;
            default:
                Debug.LogWarning("Invalid attack number for SFX: " + attackNumber);
                break;
        }
    }

    public void StopAllSFX()
    {
        audioSource.Stop();
    }
}
