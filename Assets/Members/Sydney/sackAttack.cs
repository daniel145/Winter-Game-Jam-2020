using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sackAttack : MonoBehaviour
{
    public GameObject target;
    float counter;
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //decrease enemy hp value 
        }
    }  
    public void Attack()
    {
        SpriteRenderer appear = GetComponent<SpriteRenderer>();
        CircleCollider2D hitbox = GetComponent<CircleCollider2D>();
        transform.rotation = Quaternion.Euler(0,0, 80);
        appear.enabled = true;
        hitbox.enabled = true;
        counter = 0;
    }
    void Update()
    {
        SpriteRenderer appear = GetComponent<SpriteRenderer>();
        CircleCollider2D hitbox = GetComponent<CircleCollider2D>();
        if (appear.enabled == true)
        {
            transform.RotateAround(target.transform.position, new Vector3(0, 0, 45), 150 * Time.deltaTime);
            counter += Time.deltaTime;
        }  

        if (counter >= 1)
        {
            appear.enabled = false;
            hitbox.enabled = false; 
        }
    }
}
