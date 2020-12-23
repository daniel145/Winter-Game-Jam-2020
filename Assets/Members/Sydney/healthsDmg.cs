using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthsDmg : MonoBehaviour
{
    public int fullHealth = 20; //daniel has health ui
    public int currentHP;
    public float presentCount;
    private float hitProtect; 
    public Animator animator; 

    void Start()
    {
        currentHP = fullHealth;
        hitProtect = 0;
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            animator.SetBool("Dead", true);
            Destroy(this.gameObject);
        }
        if (hitProtect > 0)
        {
            hitProtect -= Time.fixedDeltaTime;
            //Debug.Log(hitProtect);
        }
        if (currentHP > 20)
        {
            currentHP = fullHealth;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && hitProtect <=0) 
        {
            if(hitProtect <= 0)
            {
                currentHP--; 
                hitProtect = 14;
                Debug.Log("Damage taken");
                animator.SetTrigger("Hurt");
            }
            Debug.Log("Hit and enemy");
        }
        if (other.gameObject.tag == "Present")
        {
            presentCount++;
            Debug.Log("Present collected");
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Food")
        {
            if (currentHP < 20)
            { //candy cane = +1 hp, ginger bread=+2 turkey wing =+3 
               //if (other.transform.name == "Candy Cane")
                {
                   //currentHP++;
                   // Debug.Log("ate a candy cane");
                }
                //if (other.transform.name == "Gingerbread Man")
                {
               //     currentHP + 2;
                }
               //if (other.gameObject.name == "Turkey Wing")
                {
               //     currentHP + 3;
                }
                Debug.Log("Health restored");
            }
            Debug.Log("Food consumed");
            Destroy(other.gameObject);
        } 
    }
}
