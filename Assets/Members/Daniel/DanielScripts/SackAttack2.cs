using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SackAttack2 : MonoBehaviour
{
    public GameObject target;
    private int presents = 0;
    private float counter;
    private float attackDuration = 1.1f;
    private bool attacking = false;

    private SpriteRenderer appear;
    private CircleCollider2D hitbox;
    private Movement2 movement;

    private void Start()
    {
        appear = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        movement = target.GetComponent<Movement2>();
    }

    public void Attack()
    {
        if (attacking)
            return;
        else
            attacking = true;

        movement.StopMovement(1.1f);
        transform.rotation = Quaternion.Euler(0, 0, 80);
        appear.enabled = true;
        hitbox.enabled = true;
        counter = 0;
        StartCoroutine(WeaponSwing());
    }

    public void AddPresent()
    {
        presents++;
    }

    private IEnumerator WeaponSwing()
    {
        while(counter < 1.1f)
        {
            transform.RotateAround(target.transform.position, new Vector3(0, 0, 45), counter * 150);
            yield return null;
            counter += Time.deltaTime;
        }
        appear.enabled = false;
        hitbox.enabled = false;
        attacking = false;
    }
}
