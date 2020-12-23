using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sackAttack : MonoBehaviour
{
    public GameObject target;
    float counter;
    
    public Enemy enemy;
    public movement movement;
    public healthsDmg healthsDmg;
    private Vector3 scaleChange;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy.health--;
            Debug.Log(enemy.health);
        }
    }  
    public void Attack()
    {
        SpriteRenderer appear = GetComponent<SpriteRenderer>();
        CircleCollider2D hitbox = GetComponent<CircleCollider2D>();
        if (movement.direction < 0)
        {
            transform.position = target.transform.position + new Vector3(-4,2,0);
        }
        if (movement.direction > 0)
        {
            transform.position = target.transform.position + new Vector3(3, -2, 0);
        }
        transform.localScale = scaleChange; 
        appear.enabled = true;
        hitbox.enabled = true;
        counter = 0;
    }
    void Start ()
    {
        scaleChange = transform.localScale;
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

        if (counter >= 0.8)
        {
            appear.enabled = false;
            hitbox.enabled = false; 
        }
        //if (healthsDmg.presentCount >= 10)
        {
        //scaleChange = new Vector3(100,100,100);
        }
        if (healthsDmg.presentCount >= 50) // doesn't seem to be reacting with localscale 
        {
            //scaleChange = new Vector3(.01f, .01f, .01f);
            //scaleChange = transform.localScale;
            //scaleChange *= 4f;
            //scaleChange = new Vector3(4f, 4f, 4f);
            //transform.localScale = scaleChange;
        }
    }
}
