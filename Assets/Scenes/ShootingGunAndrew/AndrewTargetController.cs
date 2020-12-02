using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class AndrewTargetController : MonoBehaviour
    {
        //public GameObject target;
        public int scoreMultiplyer = 1;

        public GameObject explosion;
        private AndrewGameController gameController;

        private void Start()
        {
            gameController = GameObject.FindObjectOfType<AndrewGameController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Bullet")
            {
                //other.gameObject.GetComponent<bullet>().shooter;
                if (other.gameObject.GetComponent<bullet>().shooter.IsMine)
                {
                    Debug.Log(other.gameObject.GetComponent<bullet>().bannaGunOwner);
                    AndrewGameController.SetScore(other.gameObject.GetComponent<bullet>().bannaGunOwner);
                    PhotonNetwork.Destroy(other.gameObject);
                    gameController.SpawnTarget();

                    PhotonNetwork.Instantiate(this.explosion.name, this.transform.position, Quaternion.identity);
                    //Instantiate(explosion, this.transform.position, Quaternion.identity);
                    PhotonNetwork.Destroy(this.gameObject);
                }
                
            }
        }
    }
}

