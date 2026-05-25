using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    public GameObject JacobCutscene;
    public GameObject NaomiCutscene;
    public GameObject ActiveCutscene;
    public float countdown = 19f;

    void Start()
    {
        if (DataManager.Instance.characters[0] == true)
        {
            JacobCutscene.SetActive(true);
            ActiveCutscene = JacobCutscene;
        }
        else if (DataManager.Instance.characters[1] == true)
        {
            NaomiCutscene.SetActive(true);
            ActiveCutscene = NaomiCutscene;
        }
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void SkipCutscene()
    {
        SceneManager.LoadScene(3);
    }

}