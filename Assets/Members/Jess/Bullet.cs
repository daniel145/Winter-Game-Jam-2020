using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet:MonoBehaviour
{
    public float horizontal; 
    public float vertical; 
    const int SPEED = 2; 
    
    /*
    void Start()
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

    //todo: if it hits the player, destroy the bullet and remove some health? 

    //todo: destroy the bullet if it flies off screen 

}