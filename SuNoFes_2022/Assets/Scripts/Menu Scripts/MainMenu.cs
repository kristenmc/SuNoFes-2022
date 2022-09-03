using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void StartGame()
    {

    }

    public void OpenCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }

    public void CloseCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }

    void QuitGame()
    {
        
    }
}
