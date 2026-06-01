using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySelectedCharacter : MonoBehaviour
{
    public GameObject JacobPlayer;
    public GameObject NaomiPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DataManager.Instance.characters[0] == true)
        {
            JacobPlayer.SetActive(true);
            NaomiPlayer.SetActive(false);
        }
        else if (DataManager.Instance.characters[1] == true)
        {
            JacobPlayer.SetActive(false);
            NaomiPlayer.SetActive(true);
        }
    }
}
