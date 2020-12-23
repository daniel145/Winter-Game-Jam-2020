using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    public float timeLimit = 30f;
    public float dampen = 4f;
    [HideInInspector]
    public AudioManager audioManager;
    private Vector3 originalPosition;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(Float());

        switch(transform.name)
        {
            case "CandyPrefab(Clone)": health = 1; break;
            case "GinBrePrefab(Clone)": health = 2; break;
            case "TurkeyPrefab(Clone)": health = 3; break;
            default: health = 0; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
            if (health != 0)
                collision.gameObject.GetComponent<HealthsDmg2>().AddHealth(health);
            else
                collision.gameObject.GetComponentInChildren<SackAttack2>().AddPresent();
        }
    }

    public void Collect()
    {
        GetComponent<Collider2D>().enabled = false;
        if (CompareTag("Food"))
            audioManager.Play("chomp");
        else
            audioManager.Play("powerup");
        StartCoroutine(Spin(1.2f, 825f));
    }

    private IEnumerator Spin(float time, float speed)
    {
        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;

        while (time > 0)
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f), Space.Self);
            if (time < 1)
                transform.localScale = new Vector3(scaleX * (time / 2 + 0.5f), scaleY * (time / 2 + 0.5f), 1);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    // Makes the item bob up and down
    // Also destorys the object after a certain time limit
    private IEnumerator Float()
    {
        float t = 0;
        while (true)
        {
            transform.position = originalPosition + new Vector3(0, Mathf.Sin(t) / dampen, 0);
            yield return null;
            t += Time.deltaTime;

            if (t > timeLimit)
            {
                StartCoroutine(Spin(0.6f, 375));
                break;
            }
        }
    }
}
