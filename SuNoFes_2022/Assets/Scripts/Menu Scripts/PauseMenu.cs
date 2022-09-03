using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused;
    public bool isPaused { get { return _isPaused; }}
    private void Update()
    {
        if (Input.GetKey("escape") && !_isPaused)
        {
            PauseGame();
        }
    }
    public void ResumeGame()
    {
        _isPaused = false;
    }

    public void PauseGame()
    {
        _isPaused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
