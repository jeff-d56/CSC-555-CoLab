using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.FakeCompanyName.FakeGame
{
    public class SquritGunTarget : MonoBehaviour
    {
        public GameObject fillGauge;

        public float fillGaugePosition = 2; // 2 to 14


        public int targetNumber;

        public bool waterGunIsHiting = false;

        

        private void FixedUpdate()
        {

            if (waterGunIsHiting)
            {
                if (fillGauge.transform.localPosition.y <= 14)
                {
                    fillGauge.transform.localPosition += new Vector3(0, Time.deltaTime, 0);
                }
                else
                {
                    SquritGameManager.EndGameHelper(targetNumber);
                }
            }
            else
            {
                if (fillGauge.transform.localPosition.y > 2)
                {
                    fillGauge.transform.localPosition -= new Vector3(0, Time.deltaTime, 0);
                }
                else
                {
                    fillGauge.transform.localPosition = new Vector3(0, 2, 1.27f);
                }
            }
            
        }



        public void ResetFillGauge()
        {
            fillGauge.transform.localPosition = new Vector3(0, 2, 1.27f);
        }
    }
}

