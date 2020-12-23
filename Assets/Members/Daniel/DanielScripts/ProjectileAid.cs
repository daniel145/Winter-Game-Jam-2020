﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAid : MonoBehaviour
{
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<TrailRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
