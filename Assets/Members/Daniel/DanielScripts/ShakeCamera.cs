using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShakeCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCo(duration, magnitude));
    }

    private IEnumerator ShakeCo(float duration, float magnitude)
    {
        Vector3 original = Camera.main.transform.position;

        while (duration > 0)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(x, y, original.z);

            duration -= Time.deltaTime;
            yield return null;
        }

        transform.position = original;
    }
}
