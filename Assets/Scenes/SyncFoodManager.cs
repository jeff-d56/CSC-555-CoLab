using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class SyncFoodManager : MonoBehaviourPunCallbacks, IPunObservable
    {

        private ThrowFoodStartGame TFSG;
        private Vector3 latestPos;
        bool valuesReceived = false;

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

        private void Update()
        {
            if (!photonView.IsMine && valuesReceived)
            {
                //Update Object position and Rigidbody parameters
                transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
                
            }
        }

        private void Start()
        {
            TFSG = GameObject.FindObjectOfType<ThrowFoodStartGame>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Floor")
            {
                ThrowFoodStartGame.DecreaseFoodLeft();
                TFSG.SpawnNewFood();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Target")
            {
                ThrowFoodStartGame.IncreaseScore(other.gameObject.GetComponent<hoopController>().scoreMultiplyer);
                ThrowFoodStartGame.DecreaseFoodLeft();
                TFSG.SpawnNewFood();
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

