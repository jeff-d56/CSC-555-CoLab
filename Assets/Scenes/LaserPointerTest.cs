using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class LaserPointerTest : MonoBehaviour
    {
        [SerializeField] public Transform handAnchor;
        private LineRenderer laser;
        private int layerMask = 1 << 8;
        private testRPC testStart;
        private PhotonView PV;


        // Start is called before the first frame update
        void Start()
        {
            testStart = GameObject.FindObjectOfType<testRPC>();
            laser = this.gameObject.AddComponent<LineRenderer>();
            laser.startWidth = 0.2f;
            laser.endWidth = 0.2f;
        }

        

        // Update is called once per frame
        void Update()
        {
            //showLaser();
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Debug.Log("Laser go");
                showLaser();
            }
            else
            {
                laser.enabled = false;
            }
        }

        private void showLaser()
        {
            RaycastHit hit;
            if (Physics.Raycast(handAnchor.position, handAnchor.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {

                //PV = testStart.gameObject.GetComponent<PhotonView>();

                /*
                if (!PV.IsMine) // If the view on the box is not theirs.
                {
                    PV.TransferOwnership(PhotonNetwork.LocalPlayer); // make the view on the box theirs.
                    testStart.StartNewThrowGame(10);
                }
                else
                {
                    testStart.StartNewThrowGame(10);
                }
                */
                //Debug.DrawRay(handAnchor.position, handAnchor.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                testStart.playerStartedGame();
            }
           
        }
    }
}

