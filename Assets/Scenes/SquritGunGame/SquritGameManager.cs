using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace Com.FakeCompanyName.FakeGame
{
    public class SquritGameManager : MonoBehaviour
    {
        public List<SquritGunTarget> Targets;
        public List<SquritGun> squritGuns;
        public List<Text> targetNumberText;
        public static PhotonView SGMPV;

        public GameObject gameMenu;

        public GameObject targetHolder;
        public Transform targetTextHolder;
        public Transform squritGunHolder;

        public bool isGameRunning;

        private void Start()
        {
            SGMPV = this.GetComponent<PhotonView>();
            foreach (Transform child in targetHolder.transform)
            {
                Targets.Add(child.GetComponent<SquritGunTarget>());
            }

            foreach (Transform child in squritGunHolder)
            {
                squritGuns.Add(child.GetComponent<SquritGun>());
            }

            foreach (Transform child in targetTextHolder.transform)
            {
                targetNumberText.Add(child.GetComponent<Text>());
            }
        }

        public void StartGameHelper()
        {
            if(!isGameRunning && PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer) // If water gun is not enabled and user is masterclient
            {
                SGMPV.RPC("StartGame", RpcTarget.AllBuffered);
                //SquritGameManager.StartGameHelper(); // Start game
            }
            //SGMPV.RPC("StartGame", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void StartGame()
        {
            if (!isGameRunning)
            {
                gameMenu.SetActive(false);
                foreach (SquritGunTarget target in Targets)
                {
                    target.ResetFillGauge();
                }

                // go through all squritguns and enable them
                foreach (SquritGun gun in squritGuns)
                {
                    gun.isEnabled = true;
                }

                for (int i = 0; i < 9; i++)
                {
                    targetNumberText[i].text = (i + 1).ToString();
                }
                isGameRunning = true;
                Debug.LogError("Game has Started");

            }
        }

        public static void EndGameHelper(int targetWinner)
        {
            SGMPV.RPC("EndGame", RpcTarget.AllBuffered, targetWinner);
        }

        [PunRPC]
        public void EndGame(int targetWinner)
        {
            if (isGameRunning)
            {
                foreach (SquritGun gun in squritGuns)
                {
                    gun.isEnabled = false;
                }

                
                foreach (SquritGunTarget target in Targets)
                {
                    target.waterGunIsHiting = false;
                }
                

                isGameRunning = false;
                SetWinner(targetWinner);
                Debug.LogError("Game has ended");
                gameMenu.SetActive(true);
            }
        }


        public void SetWinner(int targetNumber)
        {
            // Set targetNumber as winner
            targetNumberText[targetNumber - 1].text = "W";

            Debug.LogError("Winner is " + targetNumber);
        }

    }
}

