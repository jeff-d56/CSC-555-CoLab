using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class SyncFoodManager : MonoBehaviourPunCallbacks, IPunObservable
    {

        private testRPC TFSG;
        private Vector3 latestPos;
        bool valuesReceived = false;

        private void Start()
        {
            TFSG = GameObject.FindObjectOfType<testRPC>();
        }

        // Track objects location
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // this will right to network
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                latestPos = (Vector3)stream.ReceiveNext();
                valuesReceived = true; // wait till we get new values
            }
        }

        // Apply objects location
        private void Update()
        {
            if (!photonView.IsMine && valuesReceived)
            {
                //Update Object position and Rigidbody parameters
                transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
                
            }
        }

        // If food hits ground and is yours
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Floor" && photonView.IsMine)
            {
                //ThrowFoodStartGame.DecreaseFoodLeft();
                //TFSG.SpawnNewFood();
                TFSG.FoodHitGround();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Target" && photonView.IsMine)
            {
                //ThrowFoodStartGame.IncreaseScore(other.gameObject.GetComponent<hoopController>().scoreMultiplyer);
                //ThrowFoodStartGame.DecreaseFoodLeft();
                TFSG.FoodHitTarget(other.gameObject.GetComponent<hoopController>().scoreMultiplyer);
                PhotonNetwork.Destroy(this.gameObject);
            }
            else if (other.CompareTag("Hand"))
            {
                if (!photonView.IsMine)
                {
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
        }
    }
}

