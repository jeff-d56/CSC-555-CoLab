using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class ThrownFoodManager : MonoBehaviour
    {

        private bool playerIsAttached = false;
        private PhotonView foodsPV;

        private ThrowFoodGameController thrownFoodGameController;
        public void Start()
        {
            thrownFoodGameController = GameObject.FindObjectOfType<ThrowFoodGameController>();
            foodsPV = this.GetComponent<PhotonView>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Hand"))
            {
                if (GameObject.ReferenceEquals(PlayerManager.LocalPlayerInstance.transform, this.gameObject.transform.root) && !foodsPV.IsMine)
                {
                    Debug.Log(PhotonNetwork.LocalPlayer.NickName);
                    foodsPV.TransferOwnership(PhotonNetwork.LocalPlayer);
                }

            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Floor" && foodsPV.IsMine)
            {
                thrownFoodGameController.foodHitSomething(false, 0); ;
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Target" && foodsPV.IsMine)
            {
                thrownFoodGameController.foodHitSomething(true, other.gameObject.GetComponent<hoopController>().scoreMultiplyer);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }


        private void FixedUpdate()
        {
            if (GameObject.ReferenceEquals(PlayerManager.LocalPlayerInstance.transform, this.gameObject.transform.root)) // if player is holding food
            {
                playerIsAttached = true;
            }
            else
            {
                playerIsAttached = false;
            }
        }
        
    }
}

