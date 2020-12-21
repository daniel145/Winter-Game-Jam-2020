using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect2 : MonoBehaviour
{
    public float width = 0.05f;
    public float time = 0.25f;
    public Color startColor = new Color();

    private TrailRenderer trail;

    private void Awake()
    {
        trail = gameObject.AddComponent<TrailRenderer>();
        trail.startWidth = width;
        trail.endWidth = width;
        trail.startColor = startColor;
        trail.endColor = new Color(0, 0, 0, 0);
        trail.time = time;

        trail.receiveShadows = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
