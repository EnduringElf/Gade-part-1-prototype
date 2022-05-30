using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public GameObject difficultyUI;

    public void DifficulityUi()
    {
        difficultyUI.SetActive(true);
    }

    public void DifficultyUiDisable()
    {
        difficultyUI.SetActive(false);
    }

    public void LoadMultiplayer()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadHardMode()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadNormalMode()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadEasyMode()
    {
        SceneManager.LoadScene(3);
    }




}
