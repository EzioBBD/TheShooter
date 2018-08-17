using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour
{
    public float speed = 1.5f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < -80)
            transform.position = new Vector3(transform.position.x + 4 * 38.4f, transform.position.y, 0);
    }
}
