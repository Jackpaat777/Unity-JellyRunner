using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Update()
    {
        // Move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * 3 * GameManager.instance.speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        // Destroy
        if (transform.position.x < -4)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // Explosion
            GameManager.instance.CallExplosion(collision.transform.position);
            collision.gameObject.SetActive(false);
            // Score
            GameManager.instance.score += 100;
            // Bullet ReRoad
            gameObject.SetActive(false);
            GameManager.instance.ReRoad();
        }
    }
}
