using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour
{
    public float movespeed;
    private Rigidbody2D rb;
    public Animator animator;
    private Vector2 moveVelocity;
    bool attacking;
    public float attackCD;
    public float cdLength = 10;

    private bool facingRight;
    private int animationID;
    private bool still = false;

    public AtkRecharge2 atkRecharge;
    public HealthsDmg2 healthsDmg;
    public SackAttack2 sackAttack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackCD = 0;
        atkRecharge.Recharge(attackCD);
        animationID = Animator.StringToHash("State");
    }

    // Update is called once per frame
    void Update()
    {
        if (still)
            return;

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * movespeed;

        if (moveInput != Vector2.zero)
            animator.SetInteger(animationID, 1);
        else
            animator.SetInteger(animationID, 0);

        if (moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
        //rb.AddForce(moveVelocity * Time.deltaTime);
        //transform.position += new Vector3(moveVelocity.x, moveVelocity.y, 0) * Time.deltaTime;

        attacking = Input.GetKeyDown(KeyCode.Space);
       //to react when the button is pushed
       if (attacking && attackCD == 0)
        { 
            sackAttack.Attack();
            animator.SetInteger(animationID, 2);
            attackCD = cdLength;
            StartCoroutine(Cooldown());
        }
    }

    public void StopMovement(float time)
    {
        still = true;
        Invoke("StartMovement", time);
    }

    public void SetRotation(bool dir = true)
    {
        transform.rotation = dir ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }

    private void StartMovement()
    {
        still = false;
    }

    private IEnumerator Cooldown()
    {
        while(attackCD > 0)
        {
            yield return null;
            attackCD -= Time.deltaTime;
            atkRecharge.Recharge(attackCD);
        }
        attackCD = 0;
    }
}
