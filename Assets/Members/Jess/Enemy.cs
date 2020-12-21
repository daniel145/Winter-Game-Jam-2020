using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy:MonoBehaviour
{
    private GameObject EnemyObject; 
    public GameObject BulletObject; //a prefab 
    private Vector2 EnemyPosition; 
    public GameObject Player; //follow the Player using tags? 
    const float WAIT_TIME = 1.0f; //fires bullets at 1 second intervals 
    float time_elapsed = 0;

    void Start()
    {
        //get the Player 
        Player = GameObject.FindWithTag("Player"); 

        //instantiate enemy at random position 
        EnemyPosition = new Vector2(Random.Range(-100F, 100F), Random.Range(-100F, 100F));
        if(EnemyPosition != (Vector2)Player.transform.position) //don't spawn an enemy on top of a player
        {
            Instantiate(EnemyObject, EnemyPosition, Quaternion.identity); 
        }
        
        
    }

    void Update()
    {
        //move in a random direction, somewhat following the player? 
        //just stationary for now 

        time_elapsed += Time.deltaTime; 
        if(time_elapsed >= WAIT_TIME)
        {
            Shoot(); 
            time_elapsed = 0; 
        }

    }

    void Shoot()
    {
        //calculate the path the bullet should go on 
        Vector2 player_position = Player.transform.position; 
        float h = this.transform.position.x - player_position.x; 
        float v = this.transform.position.y - player_position.y;
        float magnitude = Mathf.Sqrt((h*h) + (v*v));
        h /= magnitude; 
        v /= magnitude; 


        GameObject temp = Instantiate(BulletObject, (Vector2)transform.position, Quaternion.identity); 
        Bullet b = temp.GetComponent<Bullet> (); 
        b.transform.parent = this.transform; //make the bullet a child of the enemy
        b.horizontal = h; 
        b.vertical = v; 
    }
}