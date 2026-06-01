using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject pauseMenu;
    bool isPaused;
    public void TogglePauseGame(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }
    public bool GetIsPaused() { return isPaused; }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
