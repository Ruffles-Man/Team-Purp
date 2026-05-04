using UnityEngine;
using UnityEngine.InputSystem;


public class SettingsMenuController : MonoBehaviour
{

    public MonoBehaviour playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    public GameObject settingsMenu;
    private bool isOpen = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (TitleCanvasIsHidden()){
                //Debug.Log("ESC detected");
                ToggleMenu();
            }
        }
    }

    bool TitleCanvasIsHidden(){
    GameObject titleCanvas = GameObject.Find("TitleCanvas");
        if (titleCanvas == null){
            return true;
        }
        return !titleCanvas.activeInHierarchy;
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        settingsMenu.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
        if(playerController != null){
            playerController.enabled = !isOpen;
        }
    }
}
