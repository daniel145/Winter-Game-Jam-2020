using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy:MonoBehaviour
{
    
    public GameObject bulletObject; //a prefab 
    private GameObject Player; 
    Animator animator; 
    private float time_elapsed = 0; 
    private bool currentlyMoving = false; 
    private bool beingKnockedBack = false; 
    private bool currentlyMeleeAttacking = false; 
    private int animatorID; 
    private SpriteRenderer spriteRenderer; 

    /* Change these to adjust difficulty */
    public int health = 3; 
    public float shooting_distance = 12.0f; //How close the player has to be before the enemy starts shooting and moving toward it 
    public float move_speed = 1.0f; 
    public float time_between_bullets = 1.0f;
    public float min_distance_from_player = 3.0f; 
    public bool canBeKnockedBack = true; 
    public float knockback_distance = 300f; 
    public float knockback_speed = 2.0f; 
    public float melee_multiplier = 3.0f; //How much faster the enemy should move when chasing the player
    public int leap_distance = 225; //How far the enemy jumps during melee attacks -- for reference, one standard step is 150 

    public enum Attack{Standard, Honing, Radial, Melee};
    public Attack attackType = Attack.Standard; 
    //convert to ints 

    void Start()
    {
        Player = GameObject.FindWithTag("Player"); 
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        animator = GetComponentInChildren<Animator>(); 
        animatorID = Animator.StringToHash("State");
        /*
        Vector2 temp = new Vector2(0, 0); //for testing 
        if(temp != (Vector2)Player.transform.position) //Don't spawn an enemy on top of a player
            transform.position = temp; */
    }

    void Update()
    {
        if(health <= 0)
        {
            animator.SetInteger(animatorID, 5);
            Destroy(this.gameObject);
        }

        bool within_shooting_distance = Vector2.Distance((Vector2)Player.transform.position, (Vector2)transform.position) <= shooting_distance;

        if(!currentlyMoving && !(currentlyMeleeAttacking && within_shooting_distance))
            StartCoroutine(Move());

        time_elapsed += Time.deltaTime; 
        if(time_elapsed >= time_between_bullets)
        {
            if(within_shooting_distance)
            {
                if(attackType == Attack.Standard)
                    shoot();
                else if(attackType == Attack.Honing)
                    StartCoroutine(shootHoning()); 
                else if(attackType == Attack.Radial)
                    shootRadial();
                else if(attackType == Attack.Melee && !currentlyMeleeAttacking)
                {
                    float h = 0; 
                    float v = 0; 
                    getVectorToPlayer(ref h, ref v, Player.transform.position);
                    float delta_x = h * Time.deltaTime * move_speed * melee_multiplier;
                    float delta_y = v * Time.deltaTime * move_speed * melee_multiplier;
                    StartCoroutine(attackMelee(delta_x, delta_y));
                }
                    
            }
                
            time_elapsed = 0; 
        }
    }


    //Move in a random direction (up/down/left/right), but don't get too close to the player
    //If player is within attacking distance, move towards the player
    IEnumerator Move()
    {
        currentlyMoving = true; 
        
        Vector2 player_pos = Player.transform.position; 
        float player_upper_bound = player_pos.y + min_distance_from_player;  
        float player_lower_bound = player_pos.y - min_distance_from_player; 
        float player_right_bound = player_pos.x + min_distance_from_player;
        float player_left_bound = player_pos.x - min_distance_from_player; 


        float h = 0; 
        float v = 0; 
        getVectorToPlayer(ref h, ref v, Player.transform.position); 

        //Setting how far & in which direction the enemy should move 
        float delta_x = 0; 
        float delta_y = 0; 
        int dir = Random.Range(0, 4); //0, 1, 2, and 3 represent the 4 directions
        setMovementDirection(ref delta_x, ref delta_y, dir);

        //Make enemy move in player's direction if they are within range 
        if(Vector2.Distance((Vector2)Player.transform.position, (Vector2)transform.position) <= shooting_distance) 
        {
            if((h > 0 && delta_x < 0) || (h < 0 && delta_x > 0)) 
            {
                delta_x *= -1; 
                if(dir == 0)
                    dir = 1; 
                if(dir == 1)
                    dir = 0; 
            }
                
            
            if((v > 0 && delta_y < 0) || (v < 0 && delta_y > 0))
            {
                delta_y *= -1;  
                if(dir == 2)
                    dir = 3; 
                if(dir == 3)
                    dir = 2;  
            }
                
        } 

        //Also prevent the enemy from moving outside the bounds of the game
        if(isInBounds(player_left_bound - delta_x, player_right_bound - delta_x, player_upper_bound - delta_y, player_lower_bound - delta_y) ) 
        {
            Debug.Log("Hit edge of player box");
            
            //Reverse direction
            //Change if time; possible to get stuck in player box 
            if(dir == 0)
                dir = 1; 
            else if(dir == 1)
                dir = 0; 
            else if(dir == 2)
                dir = 3; 
            else if(dir == 3)
                dir = 2; 

            setMovementDirection(ref delta_x, ref delta_y, dir);
            
            //Debug.Log("beep beep reversing"); 
            yield return takeSteps(delta_x, delta_y, dir);
            currentlyMoving = false;
            yield break; 
        }

        yield return takeSteps(delta_x,  delta_y, dir);
        currentlyMoving = false;
    }

    IEnumerator takeSteps(float delta_x, float delta_y, int dir)
    {
        switch (dir)
        {
            case 0: //right 
                //Debug.Log("Moving right");
                spriteRenderer.flipX = false; 
                animator.SetInteger(animatorID, 1);
                for(int i = 0; i < 150; i++)
                {
                    
                    transform.position = new Vector2(transform.position.x + delta_x, transform.position.y);
                    yield return null; 
                }
                    
                break; 

            case 1: //left
                //Debug.Log("Moving left");
                spriteRenderer.flipX = true; 
                animator.SetInteger(animatorID, 1);
                for(int i = 0; i < 150; i++)
                {
                    transform.position = new Vector2(transform.position.x + delta_x, transform.position.y);
                    yield return null; 
                }
                    
                break; 
            
            case 2: //up
                //Debug.Log("Moving up");
                animator.SetInteger(animatorID, 1); 
                for(int i = 0; i < 150; i++)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + delta_y);
                    yield return null; 
                }
                    
                break; 

            case 3: //down
                animator.SetInteger(animatorID, 1); 
                for(int i = 0; i < 150; i++)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + delta_y);
                    yield return null; 
                }

                break; 

            default: 
                yield break; 
        }

    }

    void OnTriggerEnter2D(Collider2D other)  
    {
        if(other.gameObject.CompareTag("Weapon")) 
            health--; 

        if(health > 0 && other.gameObject.CompareTag("Weapon") && canBeKnockedBack && !beingKnockedBack) 
        {
            //Debug.Log("Beginning knockback");
            StartCoroutine(getKnockedBack(other.gameObject));
            //Debug.Log("Finished knockback");
        }
    }

    void shoot()
    {
        setShootingAnimation(); 

        //Calculate the path the bullet should go on 
        float h = 0; 
        float v = 0; 
        getVectorToPlayer(ref h, ref v, Player.transform.position);


        GameObject temp = Instantiate(bulletObject, (Vector2)transform.position, Quaternion.identity); 
        Bullet b = temp.GetComponent<Bullet> (); 
        b.horizontal = h; 
        b.vertical = v; 
    }

    IEnumerator shootHoning() 
    {
        setShootingAnimation(); 

        float h = 0;
        float v = 0; 
        getVectorToPlayer(ref h, ref v, Player.transform.position);

        GameObject temp = Instantiate(bulletObject, (Vector2)transform.position, Quaternion.identity); 
        Bullet b = temp.GetComponent<Bullet> (); 
        b.horizontal = h; 
        b.vertical = v; 

        //Bullet changes path to follow the player for x frames -- maybe change 
        for(int i = 0; i < 1500; i++)
        {
            getVectorToPlayer(ref h, ref v, Player.transform.position); 
            b.horizontal = h; 
            b.vertical = v; 
            yield return null; 
        }
    }

    void shootRadial()
    {
        setShootingAnimation();
        
        int numbullets = 8; 
        float theta = 2 * Mathf.PI / numbullets; 
        for(int i = 1; i <= numbullets; i++)
        {
            float firing_angle = theta * i; 
            GameObject temp = Instantiate(bulletObject, (Vector2)transform.position, Quaternion.identity); 
            Bullet b = temp.GetComponent<Bullet> (); 
            b.horizontal = Mathf.Cos(firing_angle);
            b.vertical = Mathf.Sin(firing_angle);
        }
    }

    IEnumerator attackMelee(float delta_x, float delta_y)
    {
        currentlyMeleeAttacking = true; 
        Debug.Log("Melee attack!");
        if(delta_x > 0)
            spriteRenderer.flipX = false; 
        else if(delta_x < 0)
            spriteRenderer.flipX = true; 
        animator.SetInteger(animatorID, 3);
        //Leaps at the player 
        for(int i = 0; i < leap_distance; i++)
        {
            transform.position = new Vector2(transform.position.x + delta_x, transform.position.y + delta_y);
            yield return null; 
        }
        currentlyMeleeAttacking = false; 
    }

    void getVectorToPlayer(ref float horizontal, ref float vertical, Vector2 player_pos) //kind of a misnomer 
    {
        horizontal = player_pos.x - this.transform.position.x ; 
        vertical = player_pos.y - this.transform.position.y;
        float magnitude = Mathf.Sqrt((horizontal*horizontal) + (vertical*vertical));
        horizontal /= magnitude; 
        vertical /= magnitude; 
    }

    void setMovementDirection(ref float delta_x, ref float delta_y, int dir)
    {
        if(dir == 0) //right
            delta_x = 1 * Time.deltaTime * move_speed; 
        else if (dir == 1) //left
            delta_x = -1 * Time.deltaTime * move_speed; 
        else if (dir == 2) //up
            delta_y = 1 * Time.deltaTime * move_speed; 
        else if (dir == 3) //down
            delta_y = -1 * Time.deltaTime * move_speed; 
    }

    IEnumerator getKnockedBack(GameObject g)
    {
        beingKnockedBack = true; 
        float h = 0; 
        float v = 0; 
        getVectorToPlayer(ref h, ref v, g.transform.position); 

        if(h > 0) 
            spriteRenderer.flipX = false; 
        else if (h < 0)
            spriteRenderer.flipX = true; 
        animator.SetInteger(animatorID, 4);

        //Move the enemy 
        for(int i = 0; i < knockback_distance; i++)
        {
            transform.position = new Vector2(transform.position.x - (h * Time.deltaTime * knockback_speed), transform.position.y - (v * Time.deltaTime * knockback_speed));
            yield return null; 
        }
        beingKnockedBack = false; 
    }

    void setShootingAnimation()
    {
        float h = 0;
        float v = 0; 
        getVectorToPlayer(ref h, ref v, Player.transform.position);
 
        if(h > 0) 
            spriteRenderer.flipX = false; 
        else if (h < 0)
            spriteRenderer.flipX = true; 
        
        animator.SetInteger(animatorID, 3);

    }

    bool isInBounds(float leftb, float rb, float ub, float lowerb)
    {
        return (transform.position.x < rb && transform.position.x > leftb 
        && transform.position.y < ub && transform.position.y > lowerb);
    }
}