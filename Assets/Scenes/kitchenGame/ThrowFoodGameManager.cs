﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame {
    public class ThrowFoodGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private int amountOFFoodToSpawn = 10;

        [SerializeField] private List<GameObject> foodList;
        private bool throwGameStarted = false;
        private bool throwGameEnded = false;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartNewThrowGame(int playerID)
        {
            GameObject food = PhotonNetwork.Instantiate(this.ChooseFood().name, spawnPosition.position, Quaternion.identity, 0);
        }

        public GameObject ChooseFood()
        {
            return foodList[Random.Range(0, foodList.Count)];
        }
    }

}


