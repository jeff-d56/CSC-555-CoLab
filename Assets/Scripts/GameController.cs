using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class GameController : MonoBehaviour
    {

        //List<int> highScore

        public static PhotonView ShGMPV;
        public bool gameOver;

        public int highScore;
        public GameObject highScoreParent;
        public Text highScoreText;
        public Text highScoreValue;


        //private ScoreManager scoreManager;

        // Start is called before the first frame update
        void Start()
        {
            ShGMPV = this.GetComponent<PhotonView>();
            //highScore = new List<int>(); // create new list
            highScore = 0;
            gameOver = false;
            StartGameHelper();
            //scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        }

        public void StartGameHelper()
        {
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer) // If water gun is not enabled and user is masterclient
            {
                ShGMPV.RPC("StartGame", RpcTarget.AllBuffered);
                
            }
        }

        [PunRPC]
        public void StartGame()
        {
            //scoreManager.score = 0; // set score
            //scoreManager.setScore(0); // set score text
            score = 0;
            scoreText.text = score.ToString();
            highScoreParent.SetActive(false); // set high score to be unactive
            gameOver = false;
        }

        public static void EndGameHelper(int targetWinner)
        {
            ShGMPV.RPC("EndGame", RpcTarget.AllBuffered, targetWinner);
        }

        [PunRPC]
        public void EndGame(int targetWinner)
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
            SetWinner(targetWinner);
            Debug.LogError("Game has ended");
        }


        public int score = 0;
        public Text scoreText;

        public void AddToScore(int mulitplyer)
        {
            score = score + mulitplyer;
            scoreText.text = score.ToString();
        }

        public void SetWinner(int targetNumber)
        {
            // Set targetNumber as winner
            targetNumberText[targetNumber - 1].text = "W";

            Debug.LogError("Winner is " + targetNumber);
        }

    }
}
