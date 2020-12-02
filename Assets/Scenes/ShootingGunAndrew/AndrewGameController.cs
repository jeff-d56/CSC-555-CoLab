using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class AndrewGameController : MonoBehaviour
    {
        //List<int> highScore

        public static PhotonView ShGMPV;
        

        //public int highScore;
        
        public GameObject gameMenu;
        public GameObject target;
        public GameObject explosion;

        public AndrewGunController leftBannaGun;
        public AndrewGunController rightBannaGun;

        public Text leftPlayerScoreText;
        public Text rightPlayerScoreText;
        public Text winnerText;

        //public Text highScoreText;
        //public Text highScoreValue;

        public static bool gameOver;
        public static bool bannaGunOneDone = false;
        public static bool bannaGunTwoDone = false;

        public static int leftPlayerScore = 0;
        public static int rightPlayerScore = 0;

        //private ScoreManager scoreManager;

        // Start is called before the first frame update
        void Start()
        {
            ShGMPV = this.GetComponent<PhotonView>();
            //highScore = new List<int>(); // create new list
            //highScore = 0;
            gameOver = true;
            //StartGameHelper();
            //scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        }

        public void StartGame()
        {
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
            {
                ShGMPV.RPC("StartGameRPC", RpcTarget.AllBuffered);
            }
            ShGMPV.RPC("StartGameRPC", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void StartGameRPC()
        {

            gameMenu.SetActive(false); // set high score to be unactive
            leftPlayerScore = 0;
            rightPlayerScore = 0;
            UpdateUi();
            //scoreManager.score = 0; // set score
            //scoreManager.setScore(0); // set score text
            //score = 0;
            //scoreText.text = score.ToString();

            gameOver = false;
        }

        public static void SetbannaGunDone(int bannaGunNumber)
        {
            ShGMPV.RPC("SetBannaGunDoneRPC", RpcTarget.AllBuffered, bannaGunNumber);
        }

        [PunRPC]
        public  void SetBannaGunDoneRPC(int bananaGunNumber)
        {
            switch(bananaGunNumber){
                case 1:
                    bannaGunOneDone = true;
                    break;
                case 2:
                    bannaGunTwoDone = true;
                    break;
            }

            if (bannaGunOneDone == bannaGunTwoDone)
            {
                string tempWinner = "";
                if (rightPlayerScore > leftPlayerScore)
                {
                    tempWinner = "Player 2 Wins!";
                }
                else
                {
                    tempWinner = "Player 1 Wins!";
                }

                if (rightPlayerScore == leftPlayerScore)
                {
                    tempWinner = "Its A Tie!";
                }
                gameOver = true;
                EndGame(tempWinner);
            }
        }

        
        public void EndGame(string winner)
        {
            //gameOver = true; // set game over to true

            /*
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
            */
            bannaGunOneDone = false;
            bannaGunTwoDone = false;

            

            leftBannaGun.amountOfBullets = 10;
            rightBannaGun.amountOfBullets = 10;

            //UpdateUi();
            gameMenu.SetActive(true);
            winnerText.text = winner;
            //SetWinner(targetWinner);
            Debug.LogError("Game has ended");
        }


        public static void SetScore(int bannaGunOwner)
        {
            ShGMPV.RPC("SetScoreRPC", RpcTarget.AllBuffered, bannaGunOwner);
        }

        [PunRPC]
        public void SetScoreRPC(int bannaGunOwner)
        {
            switch (bannaGunOwner)
            {
                case 1:
                    leftPlayerScore += 1;
                    break;
                case 2:
                    rightPlayerScore += 1;
                    break;
            }

            UpdateUi();
        }

        public void UpdateUi()
        {
            leftPlayerScoreText.text = leftPlayerScore.ToString();
            rightPlayerScoreText.text = rightPlayerScore.ToString();
        }


        public void SpawnTarget(Vector3 targetLocation, int bannaGunOwner)
        {
            ShGMPV.RPC("SpawnTargetRPC", RpcTarget.AllBuffered, targetLocation, bannaGunOwner);
        }

        [PunRPC]
        public void SpawnTargetRPC(Vector3 targetLocation, int bannaGunOwner)
        {
            Debug.Log("Spawned");
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
            {
                
                PhotonNetwork.Instantiate(this.target.name, new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(1, 4), Random.Range(4, 20)), Quaternion.identity);
                //PhotonNetwork.Instantiate(this.explosion.name, targetLocation, Quaternion.identity);
                
                SetScore(bannaGunOwner);
                Debug.LogError("Spawned Target");
            }
            Instantiate(explosion, targetLocation, Quaternion.identity);

        }



        /*

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
            //targetNumberText[targetNumber - 1].text = "W";

            Debug.LogError("Winner is " + targetNumber);
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
            gameMenu.SetActive(true);
            SetWinner(targetWinner);
            Debug.LogError("Game has ended");
        }
        */
    }
        
}
  
    

