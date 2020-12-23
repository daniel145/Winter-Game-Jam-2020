using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAid : MonoBehaviour
{
    public GameObject explosionPrefab;
    private bool running = true;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TrailRenderer>().endWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (running)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private void OnApplicationQuit()
    {
        running = false;
    }
}
