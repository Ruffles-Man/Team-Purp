using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectScript : MonoBehaviour
{
    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SelectJacob()
    {
        PlayerManager.Instance.characters[0] = true;
        PlayerManager.Instance.characters[1] = false;
        SceneManager.LoadScene(2);
    }

    public void SelectNaomi()
    {
        PlayerManager.Instance.characters[0] = false;
        PlayerManager.Instance.characters[1] = true;
        SceneManager.LoadScene(2);
    }
}