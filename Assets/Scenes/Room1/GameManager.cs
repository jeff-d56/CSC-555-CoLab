using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

namespace Com.FakeCompanyName.FakeGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        //public GameObject playerHead;
        //public GameObject playerLeftHand;
        //public GameObject playerRightHand;

        public GameObject playerPrefabNonVr;

        void Start()
        {
            /*
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            */
            
            if (PlayerManager.LocalPlayerInstance == null)
            {
                if (Launcher.playInVr)
                {
                    
                    
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    //Instantiate(this.playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity);
                    //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "HeadPrefab"), Vector3.zero, Quaternion.identity);
                    
                }
                else
                {
                    CreateNonVrPlayer();
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    //PhotonNetwork.Instantiate(this.playerPrefabNonVr.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    //Instantiate(this.playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity);
                    //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "HeadPrefab"), Vector3.zero, Quaternion.identity);
                }

            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
            
        }

        public void CreateVrPlayer()
        {
            /*
            GameObject temp = Instantiate(this.playerRig, new Vector3(0f, 5f, 0f), Quaternion.identity);
            temp.name = "PlayerRig";
            PhotonNetwork.Instantiate(this.playerHead.name, this.playerHead.transform.position, Quaternion.identity, 0);
            PhotonNetwork.Instantiate(this.playerLeftHand.name, this.playerLeftHand.transform.position, this.playerLeftHand.transform.rotation, 0);
            PhotonNetwork.Instantiate(this.playerRightHand.name, Vector3.zero, this.playerRightHand.transform.rotation, 0);
            */
        }

        public void CreateNonVrPlayer()
        {
            PhotonNetwork.Instantiate(this.playerPrefabNonVr.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        // This loads the game scene based on how many players can join
        private void LoadArena() 
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }
    }


}

