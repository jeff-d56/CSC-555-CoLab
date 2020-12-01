using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class spawnFood : MonoBehaviourPunCallbacks
    {
        public GameObject banana;
        public GameObject cheese;
        public GameObject cherry;
        public GameObject hamburger;
        public GameObject hotdog;
        public GameObject olive;
        public GameObject watermelon;

        private List<GameObject> foodList;

        public Transform spawnPosition;
        public int amountOFFoodToSpawn = 10;

        public int score = 0;
        public Text scoreText;
        public Text foodCountText;

        public Transform parent;


        private GameController gameController; // find gamecontroller script



        private void Start()
        {
            foodList = new List<GameObject>();

            foodList.Add(banana);
            foodList.Add(cheese);
            foodList.Add(cherry);
            foodList.Add(hamburger);
            foodList.Add(hotdog);
            foodList.Add(olive);
            foodList.Add(watermelon);


            gameController = GameObject.FindObjectOfType<GameController>();
            SpawnFood();
        }

        public GameObject ChooseFood()
        {
            return foodList[Random.Range(0, foodList.Count)];
        }

        public void SpawnStartFood()
        {
            Instantiate(ChooseFood(), spawnPosition.position, Quaternion.identity);
        }

        public void SpawnFood()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (gameController.gameOver == false)
                {
                    if (amountOFFoodToSpawn > 0)
                    {
                        PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity, 0);
                        //Instantiate(ChooseFood(), spawnPosition.position, Quaternion.identity);
                        amountOFFoodToSpawn--;
                        UpdateFoodCount(amountOFFoodToSpawn);
                    }

                    if (amountOFFoodToSpawn == 0)
                    {
                        UpdateFoodCount(amountOFFoodToSpawn);
                        gameController.EndGame();
                    }
                }
                else
                {
                    PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity, 0);
                    //Instantiate(ChooseFood(), spawnPosition.position, Quaternion.identity);
                }
            }
        }

        public void RestartGame()
        {
            amountOFFoodToSpawn = 10;
            UpdateFoodCount(amountOFFoodToSpawn);
        }

        public void UpdateFoodCount(int bulletCount)
        {
            foodCountText.text = bulletCount.ToString(); // update magazine
        }
    }
}

