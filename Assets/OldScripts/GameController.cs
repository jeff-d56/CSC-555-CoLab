using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //List<int> highScore;
   
    public bool gameOver;

    public int highScore;
    public GameObject highScoreParent;
    public Text highScoreText;
    public Text highScoreValue;

    //private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        //highScore = new List<int>(); // create new list
        highScore = 0;
        gameOver = false;
        //scoreManager = GameObject.FindObjectOfType<ScoreManager>();

    }
    public void StartGame()
    {
        //scoreManager.score = 0; // set score
        //scoreManager.setScore(0); // set score text
        score = 0;
        scoreText.text = score.ToString();
        highScoreParent.SetActive(false); // set high score to be unactive
        gameOver = false;
    }

    public void EndGame()
    {
        gameOver = true; // set game over to true

        if (highScore > score)
        {
            highScoreText.text = "High Score";
            highScoreValue.text = highScore.ToString();
        }
        else
        {
            highScore = score;
            highScoreText.text = "New High Score";
            highScoreValue.text = highScore.ToString();
        }
        highScoreParent.SetActive(true);
    }


    public int score = 0;
    public Text scoreText;

    public void AddToScore(int mulitplyer)
    {
        score = score + mulitplyer;
        scoreText.text = score.ToString();
    }

}
