using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] TMP_Text levelNameText;

    void Awake()
    {
        transform.SetParent(null); // Detach from any parent to persist across scenes

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public async void LoadScene(string sceneName, string levelName)
    {
        levelNameText.text = levelName;
        await screenFader.FadeOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // Display text of new level for a few seconds, then fade in
        await Task.Delay(2000);
        await screenFader.FadeIn();
    }
}
