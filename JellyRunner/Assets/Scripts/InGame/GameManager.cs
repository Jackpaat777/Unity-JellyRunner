using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header ("---------------[Jelly]")]
    public Sprite[] jellySpriteList;
    public string[] jellyNameList;
    public Sprite sharkSkillSprite;
    public Image jellyImageInPanel;
    public TextMeshProUGUI jellyNameInPanel;
    public int jellyNum;

    [Header("---------------[Player]")]
    public Player player;
    public Animator playerAc;

    [Header("---------------[Skill]")]
    public string[] skillName;
    public float[] skillDurationList;
    public float[] skillCoolList;
    public Button skillButton;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillTimeText;
    public TextMeshProUGUI coolTimeText;
    float skillTime;
    float coolTime;
    bool onSkill;
    bool onCool;

    [Header("---------------[Spawner]")]
    public ObjectManager objectManager;
    public Transform spawnPoints;
    float spawnTimer;
    float spawnDelay;

    [Header("---------------[InGame]")]
    public float speed;
    public float speedUp;
    public bool gameStart;
    public int score;
    public TextMeshProUGUI scoreText;
    public float timer;
    bool isStop;
    int scoreUp;
    float scoreTimer;

    [Header("---------------[UI]")]
    public GameObject optionPanel;
    public GameObject blackPanel;
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject overPanel;

    void Awake()
    {
        instance = this;
        GameStart();
    }

    void GameStart()
    {
        scoreUp = 1;
        scoreTimer = 0;
        // 처음에는 시간 멈추기
        StopSwitch();
    }

    void Update()
    {
        // Speed for Time
        timer += Time.deltaTime;
        // 20초마다 스피드 1증가
        speed = timer / 20 + 1 + speedUp;
        if (timer / 20 > 10)
            speed = 11 + speedUp;

        // Scoring
        scoreTimer += Time.deltaTime;
        if (scoreTimer > 0.1f)
        {
            // 스피드에 따른 점수올리기
            score += (int)speed * 2 * scoreUp;
            scoreText.text = score.ToString("N0");
            scoreTimer = 0;
        }

        // Enemy Spawn
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnDelay)
        {
            SpawnEnemy();
            spawnTimer = 0;
            spawnDelay = Random.Range(1.5f, 3f);
        }

        // Skill
        skillNameText.text = skillName[jellyNum];

        if (onCool)     // 쿨타임 시작
        {
            coolTime += Time.deltaTime;
            coolTimeText.gameObject.SetActive(true);

            // 쿨타임 시간이 지나면
            if (coolTime > skillCoolList[jellyNum])
            {
                CoolTimeOff();
            }
        }
        else            // 쿨타임 종료
        {
            coolTimeText.gameObject.SetActive(false);

            if (onSkill)    // 쿨타임이 끝나고 나서 스킬 발동 가능
            {
                skillTime += Time.deltaTime;
                UniqueSkill();
            }
        }
        skillTimeText.text = (skillDurationList[jellyNum] - skillTime).ToString("F1");
        coolTimeText.text = (skillCoolList[jellyNum] - coolTime).ToString("F1");
    }

    public void StopSwitch()
    {
        // 게임을 멈추고 키는 스위치 (버튼조작도 막아놓기)
        if (!isStop)
        {
            Time.timeScale = 0;
            isStop = true;
            skillButton.interactable = false;
        }
        else
        {
            Time.timeScale = 1;
            isStop = false;
            skillButton.interactable = true;
        }
    }
    public void JellySelect()
    {
        // 게임매니저에서 고른 스프라이트를 플레이어에게 넘겨주기
        player.PlayerSpriteSelect();
        // 선택했으면 게임시작
        gameStart = true;
        StopSwitch();
    }
    public void OptionButton()
    {
        // 게임이 멈추면 옵션패널 보이기
        StopSwitch();
        optionPanel.SetActive(isStop);
        blackPanel.SetActive(isStop);
    }
    void SpawnEnemy()
    {
        // 랜덤 적 생성 (오브젝트매니저에서 랜덤값의 인덱스가 넘겨짐)
        GameObject enemy = objectManager.Get(Random.Range(0, 11));
        enemy.transform.position = spawnPoints.position;

    }


    // ------------- 버튼 관련 함수들
    public void PageUp()
    {
        jellyNum++;
        if (jellyNum > 10)
            jellyNum = 0;

        jellyImageInPanel.sprite = jellySpriteList[jellyNum];
        jellyImageInPanel.SetNativeSize();
        jellyNameInPanel.text = jellyNameList[jellyNum];
    }
    public void PageDown()
    {
        jellyNum--;
        if (jellyNum < 0)
            jellyNum = 10;


        jellyImageInPanel.sprite = jellySpriteList[jellyNum];
        jellyImageInPanel.SetNativeSize();
        jellyNameInPanel.text = jellyNameList[jellyNum];
    }


    // ------------- 스킬 관련 함수들
    public void OnSkill()
    {
        if (onCool)
            return;

        onSkill = true;
    }
    public void OffSkill()
    {
        onSkill = false;
    }
    public void OnCool()
    {
        onCool = true;
    }
    void UniqueSkill()
    {
        switch (jellyNum)
        {
            case 0:
                SpeedUp();
                break;
            case 1:
                JumpUp();
                break;
            case 2:
                ScoreUp();
                break;
            case 3:
                PressDown();
                break;
            case 4:
                Invisibility();
                break;
            case 5:
                GetBarrier();
                break;
            case 6:
                DoubleJump();
                break;
            case 7:
                Buster();
                break;
            case 8:
                SushiAttack();
                break;
            case 9:
                AllKill();
                break;
            case 10:
                GiantJelly();
                break;
            default:
                break;
        }

        // 스킬버튼 누르면 쿨타임동안 못누르게
        skillButton.interactable = false;
    }
    void CoolTimeOff()
    {
        skillButton.interactable = true;
        coolTime = 0;
        onCool = false;
    }
    void SpeedUp()
    {
        // 스피드 2 증가
        if (skillTime < skillDurationList[jellyNum])
        {
            speedUp = 2;
        }
        else
        {
            speedUp = 0;
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void JumpUp()
    {
        // 점프 증가
        if (skillTime < skillDurationList[jellyNum])
        {
            player.jumpPower = 8;
        }
        else
        {
            player.jumpPower = 6;
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void ScoreUp()
    {
        // 스코어 3배
        if (skillTime < skillDurationList[jellyNum])
        {
            scoreUp = 3;
        }
        else
        {
            scoreUp = 1;
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void PressDown()
    {
        // 밟기 가능
        if (skillTime < skillDurationList[jellyNum])
        {
            player.isPress = true;
        }
        else
        {
            player.isPress = false;
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void Invisibility()
    {
        // 투명화
        if (skillTime < skillDurationList[jellyNum])
        {
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void GetBarrier()
    {
        player.isBarrier = true;
        skillTime = 0;
    }
    void DoubleJump()
    {
        // 더블점프 가능
        if (skillTime < skillDurationList[jellyNum])
        {
            player.isdoubleJump = true;
        }
        else
        {
            player.isdoubleJump = false;
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void Buster()
    {
        // 무적 부스트
        if (skillTime < skillDurationList[jellyNum] - 1f)
        {
            player.BustOn();
        }
        else if (skillTime < skillDurationList[jellyNum])
        {
            player.BustOff();
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void SushiAttack()
    {
        if (player.bullet.activeSelf)
        {
            onSkill = false;
            return;
        }

        // 초밥 발사
        ReRoad();
        player.bullet.SetActive(true);
        skillTime = 0;
        onSkill = false;
        onCool = true;
    }
    public void ReRoad()
    {
        player.bullet.transform.position = player.transform.position;
    }
    public void SushiGrowUp()
    {
        if (player.bullet.transform.localScale.x > 1.5)
            return;

        player.bullet.transform.localScale += Vector3.one * 0.1f;
    }
    void AllKill()
    {
        // 올 킬
        for (int i = 0; i < 11; i++)
        {
            objectManager.DisableEnemy(i);
        }
        // 추가 적 생성막기
        spawnTimer = 0;
        spawnDelay = 5;

        skillTime = 0;
        onSkill = false;
        onCool = true;
    }
    void GiantJelly()
    {
        // 무적 거대화
        if (skillTime < skillDurationList[jellyNum] - 2f)
        {
            player.GrowUp();
        }
        else if (skillTime < skillDurationList[jellyNum])
        {
            player.GrowDown();
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();
            skillTime = 0;
            onSkill = false;
            onCool = true;
        }
    }
    public void CallExplosion(Vector3 pos)
    {
        GameObject explosion = objectManager.Get(11);
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion();
    }


    // ------------- 게임작동 관련 함수들
    public void GameOver()
    {
        StopSwitch();
        overPanel.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
