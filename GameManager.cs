using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate { };
    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;
    [SerializeField]
    private CameraScript camera;
    [SerializeField]
    private ScoreText score;
    [SerializeField]
    private HighscoreText highscore;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float speedIncrement = 0.2f;
    [SerializeField]
    private GameObject playAgain;
    [SerializeField]
    public GameObject playButton;
    private bool usedAd = false;
    private int playAgainKids = 4;//mereu e 4

    private void Awake() 
    {
        spawners = FindObjectsOfType<CubeSpawner>();
        InitializeButtons();
    }

    //la fiecare click
    public void Click()
    {
        //misca camera
        camera.MoveCamera();


        if(MovingCube.CurrentCube != null)
            if(!MovingCube.CurrentCube.Stop())
                {
                    if(!usedAd)//deschide fereastra ptr ad
                        {
                            for(int i = 0 ; i < playAgainKids ; i++)
                                playAgain.transform.GetChild(i).gameObject.SetActive(true);
                            usedAd = true;
                            playButton.GetComponent<Image>().enabled = false;
                        }
                    else//a fost folosit ad.ul
                        Reset();
                }
        //schimbare spawner
        spawnerIndex = spawnerIndex == 0 ? 1 : 0;
        currentSpawner = spawners[spawnerIndex];
        //creste viteza
        if(speed < 3f)
            speed += speedIncrement;
        //spawn cub
        currentSpawner.SpawnCube(speed);
        //actualizeaza score
        OnCubeSpawned();
        //actualizare highscore
        highscore.OnCubeSpawnedd();
    }
    public void Continue()
    {
      //  Debug.Log("continue functioneaza");
       // Debug.Log(playAgain + "  playAgain");
      //  Debug.Log(playButton + "  playButton");
        for(int i = 0 ; i < playAgainKids ; i++)
            playAgain.transform.GetChild(i).gameObject.SetActive(false);
      //  Debug.Log("playAgain set active false");
        playButton.GetComponent<Image>().enabled = true;
      //  Debug.Log("playButton set active true");
    }
    public void Reset()
    {
        MovingCube.ResetCubes();
        SceneManager.LoadScene(0);
        usedAd = false;
    }
    public void InitializeButtons()
    {
        playAgain = GameObject.FindWithTag("PlayAgain");
        playButton = GameObject.FindWithTag("PlayButton");
    }
}
