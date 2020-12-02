using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


namespace Com.FakeCompanyName.FakeGame
{
    public class ThrowFoodGameController : MonoBehaviour
    {
        // Player Starts game
        // Spawn food food script only works if you own the food
        // Rpc call to update score

        public GameObject gameMenu;


        public Transform spawnPosition;

        public int highScore = 0;
        public int gameScore = 0;
        public static int foodRemaining = 10;

        public Text gameScoreText;
        public Text foodRemainingText;
        public Text menuText;

        public bool isGameRunning = false;

        public PhotonView TFPV;

        [SerializeField] private List<GameObject> foodList; // List that holds fold objects


        public void StartGame() // Called from button
        {
            if (!isGameRunning && PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer) 
            {
                TFPV.RPC("StartGameRPC", RpcTarget.AllBuffered);
            }
        }


        [PunRPC]
        public void StartGameRPC()
        {
            gameMenu.SetActive(false);
            gameScore = 0;
            foodRemaining = 10;
            isGameRunning = true;
            UpdateUI();
            SpawnFood();
        }



        public void EndGame()
        {
            if (isGameRunning && PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
            {
                TFPV.RPC("EndGameRPC", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        public void EndGameRPC()
        {
            isGameRunning = false;
            gameMenu.SetActive(true);
        }

        public void foodHitSomething(bool hitTarget, int multiplyer)
        {
            TFPV.RPC("foodHitSomethingRPC", RpcTarget.AllBuffered, hitTarget, multiplyer);
        }

        [PunRPC]
        public void foodHitSomethingRPC(bool hitTarget, int multiplyer)
        {
            foodRemaining--;

            if (hitTarget)
            {
                gameScore += 1 * multiplyer;
            }

            UpdateUI();

            if (foodRemaining <= 0)
            {
                isGameRunning = false;
                CalculateScore();
            }
            else
            {
                SpawnFood();
            }
 
        }

        public void CalculateScore()
        {

            if (highScore > gameScore)
            {
                menuText.text = "Game Over High Score " + highScore.ToString() ;
            }
            else
            {
                highScore = gameScore;
                menuText.text = "Game Over New High Score " + highScore.ToString();
            }

            gameMenu.SetActive(true);
        }


        public void SpawnFood()
        {
            if (PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer) // only the master client spawns food
            {
                PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity);
            }
        }

        public GameObject ChooseFood()
        {
            return foodList[Random.Range(0, foodList.Count)];
        }

        public void UpdateUI()
        {
            gameScoreText.text = gameScore.ToString();
            foodRemainingText.text = foodRemaining.ToString();
        }
    }
}

