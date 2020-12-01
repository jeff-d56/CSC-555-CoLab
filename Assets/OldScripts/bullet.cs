using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 0.2f;
    void Start()
    {
        Destroy(this.gameObject, 5); // destroy bullet after 5 seconds
    }
    
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 2));
      
        transform.Translate(Vector3.forward * Time.deltaTime * speed); // move bullet foward at set speed
    }
}
