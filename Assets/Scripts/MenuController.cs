using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;


    public void PlayGame()
    {
        Debug.Log("A");
        SceneManager.LoadScene("Scenes/MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
//        Time.timeScale = 1;
        scoreText.text = "Score: " + GameManager.Instance.GetScore();
        // Reset the score for the new game
        GameManager.Instance.ResetScore();
    }
}