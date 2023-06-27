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
        // ó������ �ð� ���߱�
        StopSwitch();
    }

    void Update()
    {
        // Speed for Time
        timer += Time.deltaTime;
        // 20�ʸ��� ���ǵ� 1����
        speed = timer / 20 + 1 + speedUp;
        if (timer / 20 > 10)
            speed = 11 + speedUp;

        // Scoring
        scoreTimer += Time.deltaTime;
        if (scoreTimer > 0.1f)
        {
            // ���ǵ忡 ���� �����ø���
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

        if (onCool)     // ��Ÿ�� ����
        {
            coolTime += Time.deltaTime;
            coolTimeText.gameObject.SetActive(true);

            // ��Ÿ�� �ð��� ������
            if (coolTime > skillCoolList[jellyNum])
            {
                CoolTimeOff();
            }
        }
        else            // ��Ÿ�� ����
        {
            coolTimeText.gameObject.SetActive(false);

            if (onSkill)    // ��Ÿ���� ������ ���� ��ų �ߵ� ����
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
        // ������ ���߰� Ű�� ����ġ (��ư���۵� ���Ƴ���)
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
        // ���ӸŴ������� �� ��������Ʈ�� �÷��̾�� �Ѱ��ֱ�
        player.PlayerSpriteSelect();
        // ���������� ���ӽ���
        gameStart = true;
        StopSwitch();
    }
    public void OptionButton()
    {
        // ������ ���߸� �ɼ��г� ���̱�
        StopSwitch();
        optionPanel.SetActive(isStop);
        blackPanel.SetActive(isStop);
    }
    void SpawnEnemy()
    {
        // ���� �� ���� (������Ʈ�Ŵ������� �������� �ε����� �Ѱ���)
        GameObject enemy = objectManager.Get(Random.Range(0, 11));
        enemy.transform.position = spawnPoints.position;

    }


    // ------------- ��ư ���� �Լ���
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


    // ------------- ��ų ���� �Լ���
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

        // ��ų��ư ������ ��Ÿ�ӵ��� ��������
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
        // ���ǵ� 2 ����
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
        // ���� ����
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
        // ���ھ� 3��
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
        // ��� ����
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
        // ����ȭ
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
        // �������� ����
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
        // ���� �ν�Ʈ
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

        // �ʹ� �߻�
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
        // �� ų
        for (int i = 0; i < 11; i++)
        {
            objectManager.DisableEnemy(i);
        }
        // �߰� �� ��������
        spawnTimer = 0;
        spawnDelay = 5;

        skillTime = 0;
        onSkill = false;
        onCool = true;
    }
    void GiantJelly()
    {
        // ���� �Ŵ�ȭ
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


    // ------------- �����۵� ���� �Լ���
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
