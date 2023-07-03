using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum SkillType
    {
        NONE,
        DOUBLE_JUMP,
        BARRIER,
        INVISIBLE,
        BUSTER,
        PRESS_DOWN,
        GIANT
    }

    public GameObject barrierSensor;
    public GameObject pressSensor;
    public GameObject giantSensor;
    public GameObject bullet;
    public float jumpPowerUp;
    public SkillType skillType;

    bool isJumpButton;  // 점프 버튼 관련 변수
    float jumpPower;
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

        // 게임 시작 시 캐릭터 이미지 가져오기
        spriteRenderer.sprite = GameManager.instance.jellySpriteList[Variables.jellyTypeNum];
    }

    void Update()
    {
        // Move Anim
        anim.SetFloat("runSpeed", GameManager.instance.speed);

        // SKill Button
        if (Input.GetKeyDown(KeyCode.K))
            GameManager.instance.OnSkill();

        // Jump
        // Input.GetButtonDown("Jump")
        if (isJumpButton && skillType != SkillType.GIANT && skillType != SkillType.BUSTER)
        {
            // double Jump가 켜져있을 때
            if (skillType == SkillType.DOUBLE_JUMP)
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
                jumpPower = 6 + jumpPowerUp;
                PlayerJump();
            }
            isJumpButton = false; // 점프한번 하면 버튼 초기화
        }

        // Bullet
        if (bullet.activeSelf)
        {
            // bullet이 화면 밖으로 나갈 때
            if (bullet.transform.position.x > 17)
                bullet.SetActive(false);

            // bullet Move
            Vector3 curPos = bullet.transform.position;
            Vector3 nextPos = Vector3.right * 5 * Time.deltaTime;
            bullet.transform.position = curPos + nextPos;
        }

        // 센서
        switch (skillType)
        {
            case SkillType.BARRIER:
                barrierSensor.SetActive(true);
                break;
            case SkillType.PRESS_DOWN:
                pressSensor.SetActive(true);
                break;
            case SkillType.GIANT:
                giantSensor.SetActive(true);
                break;
            default:
                barrierSensor.SetActive(false);
                pressSensor.SetActive(false);
                giantSensor.SetActive(false);
                break;
        }
    }

    public void PlayerJump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        anim.SetBool("isJump", true);
    }

    public void JumpButton()
    {
        isJumpButton = true;
        Debug.Log("Jump");
    }

    // ------------ 스킬 관련 함수들
    public void AlphaDown()
    {
        skillType = SkillType.INVISIBLE;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }
    public void AlphaUp()
    {
        skillType = SkillType.NONE;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public void GrowUp()
    {
        skillType = SkillType.GIANT;
        anim.SetBool("isMax", true);
    }
    public void GrowDown()
    {
        skillType = SkillType.NONE;
        anim.SetBool("isMax", false);
    }
    public void BustOn()
    {
        skillType = SkillType.BUSTER;
        if (transform.position.y < 0.1f)
            spriteRenderer.sprite = GameManager.instance.sharkSkillSprite;
    }
    public void BustOff()
    {
        skillType = SkillType.NONE;
        spriteRenderer.sprite = GameManager.instance.jellySpriteList[7];
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
