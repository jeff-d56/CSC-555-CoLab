using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Com.FakeCompanyName.FakeGame
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameManager LocalPlayersGameManager;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private Vector3 playerVelocity;
        [SerializeField] private bool groundedPlayer;
        [SerializeField] private float playerSpeed = 2.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private GameObject playerHead;
        [SerializeField] private GameObject ovrCameraRig;
        [SerializeField] private GameObject LaserPointer;
        [SerializeField] private Transform lHandAnchor;
        [SerializeField] private Text playerNameText;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        void Awake()
        {
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject; // If photonView is mine set localplayerinstance to this object
                buildPlayer();
                Destroy(playerNameText);
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            //DontDestroyOnLoad(this.gameObject);
        }

       

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            
        }
        public void buildPlayer()
        {
            // allow user to move
            if (Launcher.playInVr)
            {
                ovrCameraRig.gameObject.AddComponent<OVRCameraRig>();
                ovrCameraRig.gameObject.AddComponent<OVRManager>();
                ovrCameraRig.gameObject.AddComponent<OVRHeadsetEmulator>();
                playerController = this.gameObject.AddComponent<CharacterController>();
                this.gameObject.AddComponent<OVRPlayerController>();
                this.gameObject.GetComponent<OVRPlayerController>().HmdRotatesY = true;
                playerHead.GetComponent<Camera>().enabled = true;
                playerHead.GetComponent<AudioListener>().enabled = true;

                GameObject laser = Instantiate(LaserPointer, Vector3.zero, Quaternion.identity);
                laser.GetComponent<LaserPointerTest>().handAnchor = lHandAnchor;
                laser.GetComponent<LaserPointerTest>().playerTransform = LocalPlayerInstance.transform;
                laser.GetComponent<LaserPointerTest>().playerController = playerController;

            }
        }

        public void LeaveGame()
        {
            LocalPlayersGameManager.LeaveRoom();
        }
    }
}

