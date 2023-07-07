using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    public string typeName;

    void Update()
    {
        Move();
        Scrolling();
    }
    void Move()
    {
        // 배경 왼쪽으로 이동
        if (typeName == "Main")
        {
            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
            transform.position = curPos + nextPos;
        }
        else if (typeName == "Game")
        {
            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.left * speed * GameManager.instance.speed * Time.deltaTime;
            transform.position = curPos + nextPos;
        }
    }
    void Scrolling()
    {
        // 화면 밖으로 가면 다시 오른쪽으로 옮기기
        if (typeName == "Main")
        {
            if (transform.position.x < -6)
            {
                transform.localPosition = transform.localPosition + Vector3.right * 12;
            }
        }
        else if (typeName == "Game")
        {
            if (transform.position.x < -3)
            {
                transform.localPosition = transform.localPosition + Vector3.right * 18;
            }
        }
    }
}
