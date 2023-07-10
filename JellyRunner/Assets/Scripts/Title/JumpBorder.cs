using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBorder : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.AddForce(Vector2.up * 5);
    }
}
