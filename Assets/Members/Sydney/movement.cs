using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float movespeed;
    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 moveVelocity;
    bool attacking;
    public float attackCD;
    public float cdLength = 10;
    public float direction = -1;

    public atkRecharge atkRecharge;
    public healthsDmg healthsDmg;
    public sackAttack sackAttack;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackCD = 0;
        atkRecharge.Recharge(attackCD);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * movespeed;

        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", moveVelocity.magnitude);
        //to keep direction for animating, probably could be better but :O 
        if (moveInput.x > 0)
        {
            direction = 1;
            animator.SetFloat("Direction", 1);
        }
        else if (moveInput.x < 0)
        {
            direction = -1;
            animator.SetFloat("Direction", -1);
        }
       
        attacking = Input.GetButtonDown("Fire1");
       //to react when the button is pushed
       if (attacking && attackCD <=0)
        { 
            sackAttack.Attack();
            animator.SetBool("Attack", true);
            attackCD = cdLength;
        }
        if (!attacking)
        {
           animator.SetBool("Attack", false);
        }
        // decrease cooldown back to zero
        if (attackCD > 0)
        {
            atkRecharge.Recharge(attackCD);
            attackCD -= Time.fixedDeltaTime;
            //Debug.Log(attackCD)
        }
    }
   
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
    }
