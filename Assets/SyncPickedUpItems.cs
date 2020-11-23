﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.FakeCompanyName.FakeGame
{
    public class SyncPickedUpItems : MonoBehaviourPunCallbacks, IPunObservable
    {
        Rigidbody r;

        Vector3 latestPos;
        Quaternion latestRot;
        Vector3 velocity;
        Vector3 angularVelocity;

        bool valuesReceived = false;

        // Start is called before the first frame update
        void Start()
        {
            r = GetComponent<Rigidbody>();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(r.velocity);
                stream.SendNext(r.angularVelocity);
            }
            else
            {
                //Network player, receive data
                latestPos = (Vector3)stream.ReceiveNext();
                latestRot = (Quaternion)stream.ReceiveNext();
                velocity = (Vector3)stream.ReceiveNext();
                angularVelocity = (Vector3)stream.ReceiveNext();

                valuesReceived = true; // wait till we get new values
            }
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                Debug.Log(PhotonNetwork.LocalPlayer.NickName);
                
                if (other.CompareTag("Hand"))
                {
                    Debug.Log("kek");
                    //Transfer PhotonView of Rigidbody to our local player
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

                }
            }
        }
        

        // Update is called once per frame
        void Update()
        {
            
            if (!photonView.IsMine && valuesReceived)
            {
                //Update Object position and Rigidbody parameters
                transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
                r.velocity = velocity;
                r.angularVelocity = angularVelocity;
            }
            
        }
    }
}

