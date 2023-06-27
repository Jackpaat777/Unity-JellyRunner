using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject barrierSensor;
    public GameObject pressSensor;
    public GameObject giantSensor;
    public GameObject bullet;
    public float jumpPower;
    public bool isdoubleJump;
    public bool isBarrier;
    public bool isInvisibile;
    public bool isBust;
    public bool isPress;
    public bool isGiant;

    int jumpNum;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        jumpNum = 0;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 게임 시작 시 캐릭터 선택
        if (!GameManager.instance.gameStart)
        {
            PlayerSpriteSelect();
        }

        // Move Anim
        anim.SetFloat("runSpeed", GameManager.instance.speed);

        // SKill Button
        if (Input.GetKeyDown(KeyCode.K))
            GameManager.instance.OnSkill();

        // Jump
        if (Input.GetButtonDown("Jump") && !isGiant && !isBust)
        {
            // double Jump가 켜져있을 때
            if (isdoubleJump)
            {
                if (jumpNum < 2) // 점프를 두번하기 전까지 계속 점프
                {
                    jumpPower = rigid.velocity.y > 0 ? 3 : 6; // 이미 올라가고 있을 때는 점프력 감소
                    PlayerJump();
                    jumpNum++;
                }
            }
            // double Jump가 안켜져있을 때
            else if (!anim.GetBool("isJump"))
            {
                PlayerJump();
            }
        }

        // Bullet
        if (bullet.activeSelf)
        {
            // bullet이 화면 밖으로 나갈 때
            if (bullet.transform.position.x > 17)
                bullet.SetActive(false);

            // 총알은 회전하며, 오른쪽으로 이동
            Vector3 curPos = bullet.transform.position;
            Vector3 nextPos = Vector3.right * 5 * Time.deltaTime;
            bullet.transform.position = curPos + nextPos;
        }

        // -----------------센서
        // Barrier Sensor
        if (isBarrier)
            barrierSensor.SetActive(true);
        else
            barrierSensor.SetActive(false);

        // Press Sensor
        if (isPress)
            pressSensor.gameObject.SetActive(true);
        else
            pressSensor.gameObject.SetActive(false);

        // Giant Sensor
        if (isGiant)
            giantSensor.SetActive(true);
        else
            giantSensor.SetActive(false);
    }

    public void PlayerSpriteSelect()
    {
        // 캐릭터 선택
        spriteRenderer.sprite = GameManager.instance.jellySpriteList[GameManager.instance.jellyNum];
    }
    public void PlayerJump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        anim.SetBool("isJump", true);
    }

    // ------------ 스킬 관련 함수들
    public void AlphaDown()
    {
        isInvisibile = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }
    public void AlphaUp()
    {
        isInvisibile = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public void GrowUp()
    {
        isGiant = true;
        anim.SetBool("isMax", true);
    }
    public void GrowDown()
    {
        isGiant = false;
        anim.SetBool("isMax", false);
    }
    public void BustOn()
    {
        isBust = true;
        if (transform.position.y < 0.1f)
            spriteRenderer.sprite = GameManager.instance.sharkSkillSprite;
        GameManager.instance.speedUp = 5;
    }
    public void BustOff()
    {
        isBust = false;
        spriteRenderer.sprite = GameManager.instance.jellySpriteList[7];
        GameManager.instance.speedUp = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 착지
        if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
            jumpNum = 0; // 더블점프용 변수
        }

        if (collision.gameObject.tag == "Enemy")
        {
            // 센서 뚫고 플레이어랑 닿으면 게임 종료
            GameManager.instance.GameOver();
        }
    }
}