using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy:MonoBehaviour
{
    const float WAIT_TIME = 1.0f; //fires bullets at 1 second intervals 
    //bounds for spawning enemies in; maybe use camera instead?
    const float LEFT_BOUND = 0f; 
    const float RIGHT_BOUND = 5f; 
    const float LOWER_BOUND = -5f; 
    const float UPPER_BOUND = 5f; 
    const float MOVE_SPEED = 1.0f; 
    public GameObject BulletObject; //a prefab 
    private Vector2 EnemyPosition; 
    public GameObject Player; 
    private float time_elapsed = 0;
    private bool CR_running = false; 

    void Start()
    {
        //get the Player 
        Player = GameObject.FindWithTag("Player"); 

        //Spawn enemy at random position 
        Vector2 temp = new Vector2(Random.Range(LEFT_BOUND, RIGHT_BOUND), Random.Range(LOWER_BOUND, UPPER_BOUND));
        if(temp != (Vector2)Player.transform.position) //don't spawn an enemy on top of a player
        {
            transform.position = temp; 
            StartCoroutine(Move());
        }
    }

    void Update()
    {
        //move in a random direction, somewhat following the player? 
        if(CR_running == false)
        {
            StartCoroutine(Move());
        }

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
        float h = 1000; //really big placeholder values for debugging
        float v = 1000; 
        getPlayerVector(ref h, ref v, Player.transform.position);

        GameObject temp = Instantiate(BulletObject, (Vector2)transform.position, Quaternion.identity); 
        Bullet b = temp.GetComponent<Bullet> (); 
        //b.transform.parent = this.transform; //make the bullet a child of the enemy
        b.horizontal = h; 
        b.vertical = v; 
    }

    IEnumerator Move() //unfinished
    {
        CR_running = true; 
        
        //move in a random direction, but don't get too close to the player
        //have enemies move within a predetermined box rn for convenience 
        //todo: prevent enemies from running into each other

        //const float dist_between_enemies = 1.0f; 
        const float min_distance_from_player = 3.0f; 
        Vector2 player_pos = Player.transform.position; 
        float upper_bound = player_pos.y + min_distance_from_player; //should rename later 
        float lower_bound = player_pos.y - min_distance_from_player; 
        float right_bound = player_pos.x + min_distance_from_player;
        float left_bound = player_pos.x - min_distance_from_player; 

        float delta_x = 0; 
        float delta_y = 0; 
        int dir = Random.Range(0, 4); 

        
        if(dir == 0 || dir == 1)
            delta_x = 1 * Time.deltaTime * MOVE_SPEED; 
        else if (dir == 2 || dir == 3)
            delta_y = 1 * Time.deltaTime * MOVE_SPEED; 

        //don't move if you go inside the player's "bubble"
        if(transform.position.x + delta_x <= right_bound && transform.position.x - delta_x >= left_bound
        && transform.position.y + delta_y <= upper_bound && transform.position.y - delta_y >= lower_bound)
        {
            Debug.Log("Can't move into player bubble");
            CR_running = false;
            yield break; 
        }
        
        //also prevent the enemy from moving outside the bounds of the game 
        if(transform.position.x + delta_x > RIGHT_BOUND || transform.position.x - delta_x < LEFT_BOUND
        || transform.position.y + delta_y > UPPER_BOUND || transform.position.y - delta_y < LOWER_BOUND)
        {
            Debug.Log("Can't move outside of enemy bubble");
            CR_running = false;
            yield break; 
        }

        //Moves the enemy in a random direction
        switch (dir)
        {
            case 0: //right 
                Debug.Log("Moving right");
                for(int i = 0; i < 150; i++)
                {
                    if(transform.position.x + delta_x < RIGHT_BOUND)
                        transform.position = new Vector2(transform.position.x + delta_x, transform.position.y);

                    yield return null; 
                }
                    
                break; 

            case 1: //left
                Debug.Log("Moving left");
                for(int i = 0; i < 150; i++){
                    if(transform.position.x + delta_x > LEFT_BOUND)
                        transform.position = new Vector2(transform.position.x - delta_x, transform.position.y);

                    yield return null; 
                }
                    
                break; 
            
            case 2: //up
                Debug.Log("Moving up");
                for(int i = 0; i < 150; i++){
                    if(transform.position.y + delta_y < UPPER_BOUND)
                        transform.position = new Vector2(transform.position.x, transform.position.y + delta_y);

                    yield return null; 
                }
                    
                break; 

            case 3: //down
                Debug.Log("Moving down");
                for(int i = 0; i < 150; i++){
                    if(transform.position.y + delta_y > LOWER_BOUND)
                        transform.position = new Vector2(transform.position.x, transform.position.y - delta_y);

                    yield return null; 
                }

                break; 
            default: 
                CR_running = false;
                yield break; 
        }
        CR_running = false;

    }



    void getPlayerVector(ref float horizontal, ref float vertical, Vector2 player_pos)
    {
        horizontal = player_pos.x - this.transform.position.x ; 
        vertical = player_pos.y - this.transform.position.y;
        float magnitude = Mathf.Sqrt((horizontal*horizontal) + (vertical*vertical));
        horizontal /= magnitude; 
        vertical /= magnitude; 
    }
}