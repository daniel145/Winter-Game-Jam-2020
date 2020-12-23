using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthsDmg2 : MonoBehaviour
{
    public int fullHealth = 9; //daniel has health ui
    public int currentHP;
    public float protectionLength = 1.5f;

    public Animator animator;
    public GameManager gm;
    public Movement2 movement;

    private float hitProtect;
    private int stateID;

    void Start()
    {
        currentHP = fullHealth;
        hitProtect = 0;
        stateID = Animator.StringToHash("State");
    }

    void Update()
    {

    }

    private IEnumerator DecreaseProtection()
    {
        while (hitProtect > 0)
        {
            yield return null;
            hitProtect -= Time.deltaTime;
        }
        hitProtect = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && hitProtect == 0) 
        {
            currentHP--;
            gm.SetHealth(currentHP);
            hitProtect = 2;
            movement.StopMovement(0.4f);
            movement.SetRotation(other.transform.position.x - transform.position.x > 0);
            animator.SetInteger(stateID, 3);
            hitProtect = protectionLength;
            StartCoroutine(DecreaseProtection());
        }
    }

    public void AddHealth(int health)
    {
        currentHP += health;
        Mathf.Clamp(currentHP, 1, fullHealth);
        gm.SetHealth(currentHP);
    }
}
