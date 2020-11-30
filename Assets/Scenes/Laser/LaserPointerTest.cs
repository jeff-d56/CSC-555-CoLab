using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class LaserPointerTest : MonoBehaviour
    {
        [SerializeField] public Transform handAnchor;

        public CharacterController playerController;
       

        public LayerMask UILayer;

        public Transform playerTransform;

        public static bool laserIsEnabled = true;

        public LineRenderer laserLineRenderer;
        private float laserWidth = 0.01f;

        public Vector3 teleportLocation;
        public bool canTeleport = false;

        // Start is called before the first frame update
        void Start()
        {
            Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            laserLineRenderer.SetPositions(initLaserPositions);
            laserLineRenderer.SetWidth(laserWidth, laserWidth);
        }

        

        // Update is called once per frame
        void Update()
        {
            if (laserIsEnabled) // If Laser is enabled players hand is not attached to something
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)) // if player hits index trigger display the laser
                {
                    showLaser(handAnchor.position, handAnchor.TransformDirection(Vector3.forward), 500);
                    laserLineRenderer.enabled = true;
                }else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch) && canTeleport)
                {
                    playerTransform.position = teleportLocation + new Vector3(0, 1, 0) ;
                    canTeleport = false;
                    if (playerController != null)
                    {
                        playerController.enabled = true;
                    }
                    
                }
                else
                {
                    laserLineRenderer.enabled = false;
                }


            }
            else
            {
                if (laserLineRenderer.enabled)
                {
                    laserLineRenderer.enabled = false;
                }
            }
        }

        private void showLaser(Vector3 targetPosition, Vector3 direction, float length)
        {
            Vector3 endPosition; // if hit nothing display full length

            RaycastHit hit;
            if (Physics.Raycast(handAnchor.position, handAnchor.TransformDirection(Vector3.forward), out hit, length, UILayer)) // if player hit a ui element
            {
                laserLineRenderer.material.color = Color.green;
                endPosition = hit.point;

                Debug.Log(hit.transform.gameObject.layer == 8);
                if (hit.transform.gameObject.layer == 8) // Set player can teleport
                {
                    teleportLocation = hit.point;
                    canTeleport = true;
                    if (playerController != null)
                    {
                        playerController.enabled = false;
                    }

                }
                else if(hit.transform.gameObject.layer == 5)
                {
                    if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch)) // if player hits A button on left controller
                    {
                        if (hit.transform.TryGetComponent(out UiButtonPress button)) // if ui element is interactable
                        {
                            Debug.Log("Called Button");
                            button.Interact(); // call the buttons function
                        }
                    }
                }
            }
            else
            {
                canTeleport = false;
                if (playerController != null)
                {
                    playerController.enabled = true;
                }
                laserLineRenderer.material.color = Color.red;
                endPosition = targetPosition + (length * direction);
            }
            
            laserLineRenderer.SetPosition(0, targetPosition);
            laserLineRenderer.SetPosition(1, endPosition);
        }
    }
}

