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
    private Vector3 initialPosition;

    private SpriteRenderer appear;
    private CircleCollider2D hitbox;
    private Movement2 movement;
    private AudioManager audioManager;

    private void Start()
    {
        appear = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<CircleCollider2D>();
        movement = target.GetComponent<Movement2>();
        audioManager = FindObjectOfType<AudioManager>();
        initialPosition = transform.localPosition;
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
        //transform.localScale = new Vector3(presents + 1, presents + 1, 0);
    }

    public int PresentCount()
    {
        return presents;
    }

    private IEnumerator WeaponSwing()
    {
        transform.localScale = new Vector3(presents + 1, presents + 1, 0);
        transform.localPosition = initialPosition;
        transform.rotation = Quaternion.identity;
        Invoke("PlayWeaponSound", 0.6f);
        while(counter < 1.1f)
        {
            transform.RotateAround(target.transform.position, new Vector3(0, 0, -90), counter * 10);
            yield return null;
            counter += Time.deltaTime;
        }
        appear.enabled = false;
        hitbox.enabled = false;
        attacking = false;
    }

    private void PlayWeaponSound()
    {
        audioManager.Play("swish");
    }
}
