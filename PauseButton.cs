using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public bool isPaused = false;
    [SerializeField]
    private GameObject playButton;
    public void pauseGame()
    {
        if(isPaused)
        {
            playButton.SetActive(true);
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            playButton.SetActive(false);
            Time.timeScale = 0;
            isPaused = true;
        }
    }
}
