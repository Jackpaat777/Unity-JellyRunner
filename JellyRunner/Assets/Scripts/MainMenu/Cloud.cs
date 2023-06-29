using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;

    void Update()
    {
        Move();
        Scrolling();
    }

    void Move()
    {
        // 배경 왼쪽으로 이동
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling()
    {
        // 화면 밖으로 가면 다시 오른쪽으로 옮기기
        if (transform.position.x < -6)
            transform.localPosition = transform.localPosition + Vector3.right * 12;
    }
}
