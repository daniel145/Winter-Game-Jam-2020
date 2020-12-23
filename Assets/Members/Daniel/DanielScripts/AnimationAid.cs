using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = transform.parent.gameObject;
        if (parent.CompareTag("Player"))
        {
            float colliderSize = transform.parent.GetComponent<CircleCollider2D>().radius;
            transform.position -= new Vector3(colliderSize, colliderSize, 0);
        }
        else if (parent.CompareTag("Enemy"))
        {
            Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;
            transform.position -= new Vector3(colliderSize.x / 2, colliderSize.y / 2, 0);
        }
    }
}
