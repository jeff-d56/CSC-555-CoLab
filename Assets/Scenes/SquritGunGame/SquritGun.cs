using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class SquritGun : MonoBehaviour
    {
        public GameObject Target;

        public bool playerIsAttached = false;
        public bool isEnabled = false;
        public bool isFireing = false;
        
        public Transform playerHand;
        public Transform hoverOverHand;

        public PhotonView squritGunPV;
        public PhotonView targetPV;

        public ParticleSystem waterEffect;

        public int squritGunNumber;
        public LayerMask waterGunHitMask;
        public Material waterGunColor;

        private void Start()
        {
            squritGunPV = this.GetComponent<PhotonView>();
            squritGunPV.RPC("StopWaterEffect", RpcTarget.AllBuffered);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Hand") && !playerIsAttached) // If a hand hits the squrit gun
            {
                this.GetComponent<Renderer>().material.color = Color.green;
                hoverOverHand = other.gameObject.transform;
                // set that hand as movement of gun
                if (!squritGunPV.IsMine) // if squrit gun photon view is not mine make it mine
                {
                    squritGunPV.TransferOwnership(PhotonNetwork.LocalPlayer); // make the view on the box theirs.
                    targetPV.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Hand") && !playerIsAttached)
            {
                this.GetComponent<Renderer>().material.color = waterGunColor.color;
                // Set material of sqruitgun
            }
        }

        private void FixedUpdate()
        {
            if (playerHand != null)
            {
                this.transform.rotation = playerHand.rotation;
            }

            if (isEnabled && playerIsAttached)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, waterGunHitMask))
                {
                    if (hit.transform.gameObject == Target && isFireing)
                    {
                        SetWaterGauge(true);
                    }
                    else
                    {
                        SetWaterGauge(false);
                    }
                }
                else
                {
                    SetWaterGauge(false);
                }
            }
        }
        public void SetWaterGauge(bool setGauge)
        {
            if (Target.transform.TryGetComponent(out SquritGunTarget SGT))
            {
                SGT.waterGunIsHiting = setGauge;
            }
        }

        private void Update()
        {
            if (isEnabled && playerIsAttached) // If water gun is enabled
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) // If user pressed button to fire water gun
                {
                    isFireing = true;
                }
                else
                {
                    isFireing = false;
                }

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    squritGunPV.RPC("PlayWaterEffect", RpcTarget.AllBuffered);
                }
                else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
                {
                    squritGunPV.RPC("StopWaterEffect", RpcTarget.AllBuffered);
                }
            }
            else if (isFireing) // Turn water gun off after game has ended
            {
                squritGunPV.RPC("StopWaterEffect", RpcTarget.AllBuffered);
                isFireing = false; // Set is fireing back to false
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) // if user pressed button to attach hand
            {
                if (hoverOverHand != null) // if hand is not null 
                {
                    if (!playerIsAttached) // if player is not attached
                    {
                        LaserPointerTest.laserIsEnabled = false;
                        this.GetComponent<Renderer>().material.color = waterGunColor.color;
                        playerHand = hoverOverHand;
                        squritGunPV.RPC("SquritGunSetAttached", RpcTarget.AllBuffered);
                    }
                    else
                    {
                        LaserPointerTest.laserIsEnabled = true;
                        playerHand = null;
                        hoverOverHand = null;
                        this.transform.rotation = Quaternion.identity;
                        squritGunPV.RPC("SquritGunSetDetached", RpcTarget.AllBuffered);
                    }
                }
            }
        }


        [PunRPC]
        public void PlayWaterEffect()
        {
            waterEffect.Play();
        }

        [PunRPC]
        public void StopWaterEffect()
        {
            waterEffect.Stop();
        }

        [PunRPC]
        public void SquritGunSetAttached() // Set player has control over this squrit gun to stop other players from taking control
        {
            playerIsAttached = true;
        }

        [PunRPC]
        public void SquritGunSetDetached() // Set player has control over this squrit gun
        {
            playerIsAttached = false;
        }
    }

}
