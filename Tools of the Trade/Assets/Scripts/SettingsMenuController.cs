using UnityEngine;
using UnityEngine.InputSystem;


public class SettingsMenuController : MonoBehaviour
{

    public MonoBehaviour playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //settingsMenu = gameObject;
    }

    // Update is called once per frame
    public GameObject settingsMenu;
    private bool isOpen = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            //Debug.Log("ESC detected");
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        settingsMenu.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
        playerController.enabled = !isOpen;
    }
}
