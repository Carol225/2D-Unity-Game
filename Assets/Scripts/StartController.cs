﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    // Start is called before the first frame update
    public void NextPage()
    {
        SceneManager.LoadScene("StartPage");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
