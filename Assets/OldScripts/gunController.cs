using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
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
            
            Instantiate(ChooseFood(), this.transform.position, this.transform.rotation * Quaternion.Euler(20,180,0));
            amountOfBullets--;
            updateMagazine(amountOfBullets);
        }
        
        if(amountOfBullets == 0)
        {
            updateMagazine(amountOfBullets);
            gameController.EndGame();
        }
        
    }


    public void updateMagazine(int bulletCount)
    {
        magazineCount.text = bulletCount.ToString(); // update magazine
    }
}
