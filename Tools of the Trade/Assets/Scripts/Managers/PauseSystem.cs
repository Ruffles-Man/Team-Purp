using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioSource backgroundMusic;
    bool isPaused;
    
    public void TogglePauseGame(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
        if (isPaused)
        {
            backgroundMusic.Pause();
        }
        else
        {
            backgroundMusic.UnPause();
        }
    }
    
    public bool GetIsPaused() { return isPaused; }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
