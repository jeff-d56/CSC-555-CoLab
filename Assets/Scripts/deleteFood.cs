using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FakeCompanyName.FakeGame
{
    public class deleteFood : MonoBehaviour
    {
        // Start is called before the first frame update

        private GameController gameController;
        private spawnFood SpawnFood;
        public GameObject effectPrefab;

        private void Start()
        {
            gameController = GameObject.FindObjectOfType<GameController>();
            SpawnFood = GameObject.FindObjectOfType<spawnFood>();
        }

        private void Update()
        {
            if (gameController.gameOver == true) // start game
            {
                if (OVRInput.GetDown(OVRInput.Button.One) && this.gameObject.transform.parent == SpawnFood.parent)
                {
                    SpawnFood.RestartGame();
                    gameController.StartGame();
                }
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Floor")
            {
                if (gameController.gameOver == false) // if game is running add to score and subtract food
                {
                    SpawnFood.SpawnFood();
                    Destroy(this.gameObject);
                }
                else
                {
                    SpawnFood.SpawnStartFood();
                    Destroy(this.gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Target")
            {
                if (gameController.gameOver == false) // if game is running add to score and subtract food
                {
                    gameController.AddToScore(other.GetComponent<hoopController>().scoreMultiplyer);
                    SpawnFood.SpawnFood();
                    Instantiate(effectPrefab, this.transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
                else
                {
                    SpawnFood.SpawnStartFood();
                    Destroy(this.gameObject);
                }

            }

        }


    }
}

