using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
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

        //[SerializeField] private GameObject controller;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        

        void Awake()
        {
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                Debug.Log("What is called first awak");
                PlayerManager.LocalPlayerInstance = this.gameObject;
                
                buildPlayer();
                //Camera camrea = new Camera();
                //camrea.gameObject.transform.SetParent(this.gameObject.transform);
            }
            else
            {

            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("or start");

            if (photonView.IsMine)
            {
                playerController = this.GetComponent<CharacterController>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            ProcessInputs();
        }

        void ProcessInputs()
        {
            if (Launcher.playInVr)
            {
                //l_Hand.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LHand);
                //r_Hand.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RHand);
                //l_Hand.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
                //r_Hand.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand);
            }
            else
            {
                CharacterController controller = GetComponent<CharacterController>();

                // Rotate around y - axis
                transform.Rotate(0, Input.GetAxis("Horizontal") * .5f, 0);

                // Move forward / backward
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                float curSpeed = 4 * Input.GetAxis("Vertical");
                controller.SimpleMove(forward * curSpeed);
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
                this.gameObject.AddComponent<CharacterController>();
                this.gameObject.AddComponent<OVRPlayerController>();
                this.gameObject.GetComponent<OVRPlayerController>().HmdRotatesY = true;
                playerHead.GetComponent<Camera>().enabled = true;
                playerHead.GetComponent<AudioListener>().enabled = true;

                GameObject laser = Instantiate(LaserPointer, Vector3.zero, Quaternion.identity, this.transform);
                laser.GetComponent<LaserPointerTest>().handAnchor = lHandAnchor;

            }
            else
            {
                ovrCameraRig.SetActive(true);
            }

            

        }
    }
}

