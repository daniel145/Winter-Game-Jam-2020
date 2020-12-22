using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Collect", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        GetComponent<Collider2D>().enabled = false;
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
}
