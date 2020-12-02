using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Com.FakeCompanyName.FakeGame
{
    public class AndrewGunController : MonoBehaviour
    {
        public Transform parent;

        private AndrewGameController gameController; // find gamecontroller script
        public int bannaGunNumber;

        //public Text magazineCount;
        public int amountOfBullets = 10;

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

        public bool playerIsAttached = false;

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
            //gameController = GameObject.FindObjectOfType<AndrewGameController>();
        }

       

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Hand"))
            {
                Debug.Log(GameObject.ReferenceEquals(PlayerManager.LocalPlayerInstance.transform, this.gameObject.transform.root) && !bananaGunPV.IsMine);
                if (GameObject.ReferenceEquals(PlayerManager.LocalPlayerInstance.transform, this.gameObject.transform.root) && !bananaGunPV.IsMine)
                {
                    Debug.Log(PhotonNetwork.LocalPlayer.NickName);
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
            if (!AndrewGameController.gameOver) // if game is running
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && playerIsAttached && bananaGunPV.IsMine) // player has gun and hit trigger
                {
                    shootGun();
                }
            }

           
            if (GameObject.ReferenceEquals(PlayerManager.LocalPlayerInstance.transform, this.gameObject.transform.root)) // if player is holding banana gun
            {
                playerIsAttached = true;
            }
            else
            {
                playerIsAttached = false;
            }

        }

        public void shootGun()
        {

            //updateMagazine(amountOfBullets);

            if (amountOfBullets > 0)
            {
                Debug.Log(this.transform.localPosition);

                //Instantiate(ChooseFood(), this.transform.position, this.transform.rotation * Quaternion.Euler(20, 180, 0));
                GameObject tempBullet = PhotonNetwork.Instantiate(this.ChooseFood().name, this.transform.position, this.transform.rotation * Quaternion.Euler(20, 180, 0), 0);
                tempBullet.GetComponent<bullet>().bannaGunOwner = bannaGunNumber;
                tempBullet.GetComponent<bullet>().shooter = bananaGunPV;
                amountOfBullets--;
                //updateMagazine(amountOfBullets);
            }

            if (amountOfBullets == 0)
            {
                //updateMagazine(amountOfBullets);
                AndrewGameController.SetbannaGunDone(bannaGunNumber);
            }

        }

        /*
        public void updateMagazine(int bulletCount)
        {
            magazineCount.text = bulletCount.ToString(); // update magazine
        }
        */
    }
}
