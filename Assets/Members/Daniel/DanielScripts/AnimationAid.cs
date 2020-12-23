using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 colliderSize = transform.parent.GetComponent<BoxCollider2D>().size;
        transform.position -= new Vector3(colliderSize.x / 2, colliderSize.y / 2, 0);
    }
}
