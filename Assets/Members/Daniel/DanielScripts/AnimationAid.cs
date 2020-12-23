using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;
        float colliderWidth = colliderSize.x;
        transform.position -= new Vector3(colliderWidth / 2, colliderSize.y / 2, 0);
    }
}
