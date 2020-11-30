using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject target;
    public int scoreMultiplyer = 1;

    public GameObject explosion;
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject); // destroy bullet
            gameController.AddToScore(scoreMultiplyer); // set score
            Instantiate(target, new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(1, 4), Random.Range(4, 20)), Quaternion.identity); // spawn new target
            Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject); // destroy this target
        }
    }
}
