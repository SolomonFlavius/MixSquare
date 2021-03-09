using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreText : MonoBehaviour
{
    private TextMeshProUGUI highscore;
    [SerializeField]
    private ScoreText scoreText;
    void Start()
    {
        highscore = GetComponent<TMPro.TextMeshProUGUI>();
        highscore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
    }
    private void Update() 
    {
        
    }
    public void OnCubeSpawnedd()
    {
        if(scoreText.score >=  PlayerPrefs.GetInt("Highscore", 0))
        {
            
           PlayerPrefs.SetInt("Highscore", scoreText.score );
        }
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0);

    }
}
