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
    public float cdLength = 5;

    public atkRecharge atkRecharge;
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

        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", moveVelocity.magnitude);
       
        attacking = Input.GetButtonDown("Fire1");
       //to react when the button is pushed
       if (attacking && attackCD <=0)
        {
            Debug.Log("Attack");
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
            Debug.Log(attackCD);
        }
    }
   
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
