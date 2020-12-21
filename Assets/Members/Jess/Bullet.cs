using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet:MonoBehaviour
{
    public float horizontal; 
    public float vertical; 
    const int SPEED = 3; 
    //public GameObject Player;    
    
    /*void Start()
    { 
        //for testing purposes
        horizontal = 1; 
        vertical = 1; 
    } */

    void Update()
    {
        //move the bullet forward
        float delta_x = horizontal * Time.deltaTime * SPEED; 
        float delta_y = vertical * Time.deltaTime * SPEED; 
        this.transform.position = new Vector2(transform.position.x + delta_x, transform.position.y + delta_y);
    }

    //todo: if it hits the player, decrease player health 
    void OnTriggerEnter2D()
    {
        //Debug.Log("collision!");
        Destroy(this.gameObject);
    }

    //Destroy the bullet if it flies off screen 
    //Kind of redundant because it also gets called when OnTriggerEnter2D gets called
    void OnBecameInvisible()
    {
        //Debug.Log("Destroyed off-screen bullet");
        Destroy(this.gameObject);
    }


}