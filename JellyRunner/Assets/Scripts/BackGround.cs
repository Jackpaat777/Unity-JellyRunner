using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public GameManager manager;
    public float speed;

    void Update()
    {
        Move();
        Scrolling();
    }

    void Move()
    {
        // ��� �������� �̵�
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * manager.speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling()
    {
        // ȭ�� ������ ���� �ٽ� ���������� �ű��
        if (transform.position.x < -6)
            transform.localPosition = transform.localPosition + Vector3.right * 24;
    }
}
