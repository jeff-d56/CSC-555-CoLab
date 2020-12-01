using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Com.FakeCompanyName.FakeGame
{
    // when first player joins the gamemanagers photonview will default to them
    // so only send RPC if photonview is yours.


    public class shootingGameRPC : MonoBehaviour
    {
        private PhotonView PV;
        [SerializeField] private GameObject StartGameMenuCube; // Shoot this to start game
        [SerializeField] private Transform spawnPosition; // This is where food is spawned
        [SerializeField] private List<GameObject> foodList; // List that holds fold objects

        [SerializeField] private GameObject EndGameScreenObject; // Displays when Game is over
        [SerializeField] private Text EndGameScreenScoreText;
        [SerializeField] private Text EndGameScreenText;

        private int score;
        private int highScore;
        private bool gameStarted = false;


        [SerializeField] private Text scoreText;

        void Start()
        {
            Debug.Log("what");
            PV = this.GetComponent<PhotonView>();
            score = 69;
            UpdateUI();
        }

        public void playerStartedGame()
        {
            Debug.LogError("from laser " + PV.IsMine);
            if (gameStarted == false)
            {
                if (PV.IsMine)
                {
                    PV.RPC("StartNewGame", RpcTarget.AllBuffered);
                }
                else if (!PV.IsMine) // If the view on the box is not theirs.
                {
                    Debug.LogError("view not mine " + PV.IsMine);
                    PV.TransferOwnership(PhotonNetwork.LocalPlayer); // make the view on the box theirs.
                    Debug.LogError("view should be mine  " + PV.IsMine);
                    PV.RPC("StartNewGame", RpcTarget.AllBuffered);
                }
            }
        }



        [PunRPC]
        public void testDecreasFood()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(0, 0.5f, 0);
        }

        [PunRPC]
        public void StartNewGame()
        {
            Debug.LogError(PV.IsMine);
            StartGameMenuCube.SetActive(false);
            EndGameScreenObject.SetActive(false);
            Debug.LogError(gameStarted);
            gameStarted = true;
            Debug.LogError(gameStarted);

            score = 0;
            UpdateUI();
        }

        [PunRPC]
        public void EndGame()
        {

            if (highScore > score)
            {
                EndGameScreenText.text = "Game Over High Score";
                EndGameScreenScoreText.text = highScore.ToString();
            }
            else
            {
                highScore = score;
                EndGameScreenText.text = "Game Over New High Score";
                EndGameScreenScoreText.text = highScore.ToString();
            }

            gameStarted = false;
            EndGameScreenObject.SetActive(true);
            StartGameMenuCube.SetActive(true);
        }

        public void FoodHitTarget(int multi)
        {
            PV.RPC("FoodHitTargetUpdate", RpcTarget.AllBuffered, multi);
        }

        [PunRPC]
        public void FoodHitTargetUpdate(int multiplyer)
        {
            score += 1 * multiplyer;
            UpdateUI();
        }
        /*
        public void SpawnNewFood()
        {
            Debug.LogError("Called This");
            Debug.LogError(PV.IsMine);
            if (PV.IsMine) // Only spawn food if you own the game
            {
                Debug.LogError("What is this " + foodRemaining);
                if (foodRemaining <= 0)
                {
                    PV.RPC("EndGame", RpcTarget.AllBuffered);
                }
                else
                {
                    Debug.LogError("What the fuck " + foodRemaining);
                    PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity, 0);
                }
            }


        }
        */

        private void UpdateUI()
        {
            scoreText.text = score.ToString();
        }


    }
}
