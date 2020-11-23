using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Com.FakeCompanyName.FakeGame
{
    public class ThrowFoodStartGame : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private Collider startGameBox;
        [SerializeField] private int playerID;
        private bool turnOffBox = false;


        [SerializeField] private Transform spawnPosition;
        [SerializeField] private int amountOFFoodToSpawn = 10;

        [SerializeField] private List<GameObject> foodList;
        [SerializeField] private bool throwGameStarted = false;
        [SerializeField] private bool throwGameEnded = false;

        
        [SerializeField] private bool testThrowVar = false;

        // This is for score and food left
        [SerializeField] private static int throwGameScore = 0;
        [SerializeField] private static int throwGameFoodLeft = 10;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text foodLeftText;


        public int highScore; // highestScore obtained.
        public GameObject highScoreParentObject; // Canvas that holds highscore
        public Text highScoreText; // 
        public Text highScoreValue;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // If player hits box in front of throw food game
            {
                if (!photonView.IsMine) // If the view on the box is not theirs.
                {
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer); // make the view on the box theirs.
                }

                playerID = other.gameObject.GetComponent<PhotonView>().ViewID; // get player id for no reason
                Debug.Log(playerID + " Started New Game"); // log for fun
                turnOffBox = true; // Tell game to turn off box collider
                StartNewThrowGame(playerID);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // this will right to network
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(turnOffBox); // since the view is mine I am writing turnoffbox to everyone // so when someone enters the box they own the box and tell everyone else to
                stream.SendNext(throwGameStarted); // tell everyone on netwrok that game has started.
                stream.SendNext(throwGameScore); // tell everyone on network score of game.
                stream.SendNext(throwGameFoodLeft);
                stream.SendNext(throwGameEnded);
                stream.SendNext(testThrowVar);
            }
            else
            {
                //Network player, receive data
                turnOffBox = (bool)stream.ReceiveNext();
                throwGameStarted = (bool)stream.ReceiveNext();
                throwGameScore = (int)stream.ReceiveNext();
                throwGameFoodLeft = (int)stream.ReceiveNext();
                throwGameEnded = (bool)stream.ReceiveNext();
                testThrowVar = (bool)stream.ReceiveNext();
                //startGameBox.enabled = turnOffBox;

                updateGame(); // could call this and update game after all this is done

            }
        }

        public void SpawnNewFood()
        {
            PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity, 0); // this will spawn food for everyone on network
            /*
            if (throwGameFoodLeft == 0)
            {
                throwGameEnded = true;
                EndThrowGame();
            }
            else
            {
                PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity, 0); // this will spawn food for everyone on network
            }
            */
        }

        public void StartNewThrowGame(int playerID)
        {
            throwGameScore = 0;
            throwGameStarted = true;
            throwGameFoodLeft = 10;
            foodLeftText.text = throwGameFoodLeft.ToString();
            scoreText.text = throwGameScore.ToString();
            SpawnNewFood();
            
            testThrowVar = true;
        }

        public void EndThrowGame()
        {

            if (highScore > throwGameScore)
            {
                highScoreText.text = "Game Over High Score";
                highScoreValue.text = highScore.ToString();
            }
            else
            {
                highScore = throwGameScore;
                highScoreText.text = "Game Over New High Score";
                highScoreValue.text = highScore.ToString();
            }
            highScoreParentObject.SetActive(true); // need to tell everyone on network to do this.
            turnOffBox = false;
        }

        public GameObject ChooseFood()
        {
            return foodList[Random.Range(0, foodList.Count)];
        }

        public static void IncreaseScore(int multiplyer)
        {
            throwGameScore += 1 * multiplyer; // called by 
        }

        public static void DecreaseFoodLeft()
        {
            throwGameFoodLeft--; // called by
            
        }

        public void updateGame()
        {
            foodLeftText.text = throwGameFoodLeft.ToString();
        }

        public void Update()
        {
            foodLeftText.text = throwGameFoodLeft.ToString(); // probably dont put this here
            if (turnOffBox)
            {
                startGameBox.enabled = false;
            }
            else
            {
                startGameBox.enabled = true;
            }

            if (throwGameEnded)
            {
                EndThrowGame();
            }
            //Debug.Log(turnOffBox);
        }

    }

}
