using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class Jelly : MonoBehaviour
{
    public float changeTime;
    public float moveX;
    public float moveY;

    float timer;
    bool isWalk;
    Animator anim;
    SpriteRenderer spriteRen;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();

        // 시작 값 초기화
        timer = 0;
        int randWalk = Random.Range(0, 2);
        if (randWalk == 0)
            isWalk = true;
        else
            isWalk = false;
        RandomPosition();
    }
    void RandomPosition()
    {
        float randX = Random.Range(-5f, 5f);
        float randY = Random.Range(-2f, 1f);
        transform.position = new Vector3(randX, randY, randY);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isWalk)
            Move();
        else
            Idle();

        Turn();
    }

    void Move()
    {
        // Walk
        Vector3 nextVec = new Vector3(moveX, moveY, moveY) * Time.deltaTime;
        transform.Translate(nextVec);

        // Animation
        if (moveX < 0)
            spriteRen.flipX = true;
        else
            spriteRen.flipX = false;
        anim.SetBool("isWalk", true);

        // Change State
        if (timer > changeTime)
        {
            isWalk = false;
            timer = 0;
            changeTime = Random.Range(2f, 8f);
        }
    }
    void Idle()
    {
        anim.SetBool("isWalk", false);

        // Change State
        if (timer > changeTime)
        {
            isWalk = true;
            moveX = Random.Range(-0.8f, 0.8f);
            moveY = Random.Range(-0.8f, 0.8f);
            timer = 0;
            changeTime = Random.Range(2f, 8f);
        }
    }
    void Turn()
    {
        if (transform.position.x < -5.5f || transform.position.x > 5.5f)
            moveX *= -1;
        if (transform.position.y < -2.2f || transform.position.y > 1.2f)
            moveY *= -1;
    }
}
