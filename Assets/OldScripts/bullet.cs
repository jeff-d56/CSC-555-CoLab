using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class bullet : MonoBehaviour
{
    public int bannaGunOwner;
    public PhotonView shooter;
    public float speed = 0.2f;
    void Start()
    {
        //Destroy(this.gameObject, 5); // destroy bullet after 5 seconds
        //PhotonNetwork.Destroy(this.gameObject);
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(this.gameObject);
    }
    
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 2));
      
        transform.Translate(Vector3.forward * Time.deltaTime * speed); // move bullet foward at set speed
    }
}
