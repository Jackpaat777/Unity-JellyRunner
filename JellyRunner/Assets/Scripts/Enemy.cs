using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    void Update()
    {
        // Move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * GameManager.instance.speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        // Destroy
        if (transform.position.x < -4)
            gameObject.SetActive(false);
    }
}
