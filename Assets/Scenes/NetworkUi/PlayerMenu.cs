using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.FakeCompanyName.FakeGame
{
    public class PlayerMenu : MonoBehaviour
    {
        public Transform playerMenuHand;
        public GameObject menu;
        // Start is called before the first frame update
        void Start()
        {
            playerMenuHand = PlayerManager.LocalPlayerInstance.transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
            //y value of menu should be 0.5
        }

        private void Update()
        {
            if (playerMenuHand != null)
            {
                this.transform.position = playerMenuHand.transform.position + new Vector3(0, 0.5f, 0);
            }
            else
            {
                playerMenuHand = PlayerManager.LocalPlayerInstance.transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
            }
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                menu.SetActive(true);
            }
            else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                menu.SetActive(false);
            }
        }
    }
}

