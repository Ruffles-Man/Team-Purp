using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private PlayerHealth playerHealthJacob;
    [SerializeField] private PlayerHealth playerHealthNaomi;

    private bool isGameOver;

    private void Awake()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (playerHealthJacob == null && playerHealthNaomi == null)
        {
            Debug.LogWarning("[GameOverSystem] PlayerHealth not found in scene. Game over events will not be handled.");
        }

        if (playerHealthJacob != null)
        {
            playerHealthJacob.healthZero.AddListener(HandlePlayerDeath);
        }

        if (playerHealthNaomi != null)
        {
            playerHealthNaomi.healthZero.AddListener(HandlePlayerDeath);
        }
    }

    private void OnDisable()
    {
        if (playerHealthJacob != null)
        {
            playerHealthJacob.healthZero.RemoveListener(HandlePlayerDeath);
        }

        if (playerHealthNaomi != null)
        {
            playerHealthNaomi.healthZero.RemoveListener(HandlePlayerDeath);
        }
    }

    private void HandlePlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
        }
    }

    public void RestartTutorial()
    {
        SceneManager.LoadScene(3);
    }
}
