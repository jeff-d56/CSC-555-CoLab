using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class gunController : MonoBehaviour
    {
        public Transform parent;

        private GameController gameController; // find gamecontroller script

        public Text magazineCount;
        public int amountOfBullets = 50;

        public GameObject bananaBullet;
        public GameObject cheeseBullet;
        public GameObject cherryBullet;
        public GameObject hamburgerBullet;
        public GameObject hotdogBullet;
        public GameObject oliveBullet;
        public GameObject watermelonBullet;
        private List<GameObject> foodList;

        public PhotonView bananaGunPV;
        public PhotonView targetPV;

        public Transform playerHand;
        public Transform hoverOverHand;

        private void Start()
        {
            bananaGunPV = this.GetComponent<PhotonView>();

            foodList = new List<GameObject>();

            foodList.Add(bananaBullet);
            foodList.Add(cheeseBullet);
            foodList.Add(cherryBullet);
            foodList.Add(hamburgerBullet);
            foodList.Add(hotdogBullet);
            foodList.Add(oliveBullet);
            foodList.Add(watermelonBullet);

            //magazineCount.text = amountOfBullets.ToString();
            gameController = GameObject.FindObjectOfType<GameController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Hand") && !playerIsAttached)
            {
                hoverOverHand = other.gameObject.transform;
                if (!bananaGunPV.IsMine)
                {
                    bananaGunPV.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
        }

        public GameObject ChooseFood()
        {
            return foodList[Random.Range(0, foodList.Count)];
        }

        void Update()
        {
            // if user presses trigger and parent is right hand shoot bullet
            if (gameController.gameOver == false) // shoot gun if game is still running
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && this.gameObject.transform.parent == parent)
                {
                    shootGun();
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) // if user pressed button to attach hand
            {
                if (hoverOverHand != null) // if hand is not null 
                {
                    if (!playerIsAttached) // if player is not attached
                    {
                        LaserPointerTest.laserIsEnabled = false;
                        playerHand = hoverOverHand;
                        squritGunPV.RPC("bananaGunSetAttached", RpcTarget.AllBuffered);
                    }
                    else
                    {
                        LaserPointerTest.laserIsEnabled = true;
                        playerHand = null;
                        hoverOverHand = null;
                        this.transform.rotation = Quaternion.identity;
                        bananaGunPV.RPC("bananaGunSetDetached", RpcTarget.AllBuffered);
                    }
                }
            }

            if (gameController.gameOver == true) // start game
            {
                if (OVRInput.GetDown(OVRInput.Button.One) && this.gameObject.transform.parent == parent)
                {
                    amountOfBullets = 50; // reset bullet count
                    updateMagazine(amountOfBullets); // update magazine
                    gameController.StartGame();
                }
            }

        }

        public void shootGun()
        {

            //updateMagazine(amountOfBullets);

            if (amountOfBullets > 0)
            {
                Debug.Log(this.transform.localPosition);

                Instantiate(ChooseFood(), this.transform.position, this.transform.rotation * Quaternion.Euler(20, 180, 0));
                amountOfBullets--;
                updateMagazine(amountOfBullets);
            }

            if (amountOfBullets == 0)
            {
                updateMagazine(amountOfBullets);
                gameController.EndGameHelper();
            }

        }


        public void updateMagazine(int bulletCount)
        {
            magazineCount.text = bulletCount.ToString(); // update magazine
        }

        [PunRPC]
        public void bananaGunSetAttached() // Set player has control over this squrit gun to stop other players from taking control
        {
            playerIsAttached = true;
        }

        [PunRPC]
        public void bananaGunSetDetached() // Set player has control over this squrit gun
        {
            playerIsAttached = false;
        }

    }
}
