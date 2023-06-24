using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager manager;
    public float JumpPower;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!manager.gameStart)
        {
            PlayerSpriteSelect();
        }

        // Jump
        // 물리법칙이지만 Fixed에서는 프레임이 느리기 때문에 Update에서 처리
        // 모바일에서는 Fixed에서 잘 처리되는지 확인해보기
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }
    }

    public void PlayerSpriteSelect()
    {
        // 게임매니저에 따라 플레이어 종류 변경
        spriteRenderer.sprite = manager.jellySpriteList[manager.jellyNum];
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 착지
        if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
        }
    }
}
