using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public string NextLevelScene;
    public string NextLevelText;
    public bool CanTransition = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CanTransition)
            {
                // Transition to next level
                SceneTransition.Instance.LoadScene(NextLevelScene, NextLevelText);
            }
        }
    }
}
