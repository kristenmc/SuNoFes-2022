using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Not my code. It is being used by the game though.
public class MainMenu : MonoBehaviour
{
    void Start()
    {
        //AkSoundEngine.PostEvent("Play_shopMusic", this.gameObject);
    }
    public void StartGame()
    {
        //AkSoundEngine.PostEvent("Stop_shopMusic", this.gameObject);
        SceneManager.LoadScene(1);
    }

    public void OpenCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }

    public void CloseCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
