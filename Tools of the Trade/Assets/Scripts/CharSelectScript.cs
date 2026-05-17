using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectScript : MonoBehaviour
{
    public GameObject[] characters;

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SelectJacob()
    {
        characters[0].SetActive(true);
        characters[1].SetActive(false);
        SceneManager.LoadScene(2);
    }

    public void SelectNaomi()
    {
        characters[0].SetActive(false);
        characters[1].SetActive(true);
        SceneManager.LoadScene(2);
    }
}