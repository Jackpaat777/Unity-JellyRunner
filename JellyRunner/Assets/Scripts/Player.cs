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
        // ������Ģ������ Fixed������ �������� ������ ������ Update���� ó��
        // ����Ͽ����� Fixed���� �� ó���Ǵ��� Ȯ���غ���
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }
    }

    public void PlayerSpriteSelect()
    {
        // ���ӸŴ����� ���� �÷��̾� ���� ����
        spriteRenderer.sprite = manager.jellySpriteList[manager.jellyNum];
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ����
        if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
        }
    }
}
