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
    public Sprite sharkJellySkill;
    public Image jellyImageInPanel;
    public TextMeshProUGUI jellyNameInPanel;
    public int jellyNum;

    [Header("---------------[Player]")]
    public Player player;
    public Animator playerAc;

    [Header("---------------[Skill]")]
    public bool onSkill;
    public float skillTime;
    public string[] skillName;
    public Button skillButton;
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillTimeText;

    [Header("---------------[Spawner]")]
    public ObjectManager objectManager;
    public float spawnTimer;
    public float spawnDelay;
    public Transform spawnPoints;

    [Header("---------------[InGame]")]
    public float speed;
    public bool gameStart;
    public bool isStop;
    public GameObject optionPanel;
    public GameObject blackPanel;
    public GameObject leftButton;
    public GameObject rightButton;
    public TextMeshProUGUI scoreText;
    public GameObject overPanel;
    public int score;
    int scoreUp;
    float timer;

    void Awake()
    {
        instance = this;
        GameStart();
    }

    void GameStart()
    {
        scoreUp = 1;
        timer = 0;
        // ó������ �ð� ���߱�
        StopSwitch();
    }

    void Update()
    {
        skillNameText.text = skillName[jellyNum];

        // Scoring
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            // ���ǵ忡 ���� �����ø���
            score += (int)(speed * 2 * scoreUp);
            scoreText.text = score.ToString("N0");
            timer = 0;
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
        if (onSkill)
        {
            skillTime += Time.deltaTime;
            UniqueSkill();
        }
        skillTimeText.text = skillTime.ToString("F1");
    }

    void SpawnEnemy()
    {
        // ���� �� ���� (������Ʈ�Ŵ������� �������� �ε����� �Ѱ���)
        GameObject enemy = objectManager.Get(Random.Range(0, 11));
        enemy.transform.position = spawnPoints.position;

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

    // ������ ���߰� Ű�� ����ġ
    public void StopSwitch()
    {
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
    

    // ------------- ��ư ���� �Լ���
    public void PageUp()
    {
        jellyNum++;
        if (jellyNum == 10)
            rightButton.SetActive(false);
        else // pageUp�� ���� �� jellyNum�� 0~9
            leftButton.SetActive(true);

        jellyImageInPanel.sprite = jellySpriteList[jellyNum];
        jellyImageInPanel.SetNativeSize();
        jellyNameInPanel.text = jellyNameList[jellyNum];
    }
    public void PageDown()
    {
        jellyNum--;
        if (jellyNum == 0)
            leftButton.SetActive(false);
        else // pageDown�� ���� �� jellyNum�� 1~10
            rightButton.SetActive(true);

        jellyImageInPanel.sprite = jellySpriteList[jellyNum];
        jellyImageInPanel.SetNativeSize();
        jellyNameInPanel.text = jellyNameList[jellyNum];
    }


    // ------------- ��ų ���� �Լ���
    public void OnSkill()
    {
        onSkill = true;
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
    }
    void SpeedUp()
    {
        // 3�ʵ��� ���ǵ� 2�� ��
        if (skillTime < 3f)
        {
            speed = 2;
        }
        else
        {
            speed = 1;
            skillTime = 0;
            onSkill = false;
        }
    }
    void JumpUp()
    {
        // 10�ʵ��� ���� ����
        if (skillTime < 10f)
        {
            player.jumpPower = 8;
        }
        else
        {
            player.jumpPower = 6;
            skillTime = 0;
            onSkill = false;
        }
    }
    void ScoreUp()
    {
        // 10�ʵ��� ���ھ� 5��
        if (skillTime < 10f)
        {
            scoreUp = 5;
        }
        else
        {
            scoreUp = 1;
            skillTime = 0;
            onSkill = false;
        }
    }
    void PressDown()
    {
        // 10�ʵ��� ��� ����
        if (skillTime < 10f)
        {
            player.isPress = true;
        }
        else
        {
            player.isPress = false;
            skillTime = 0;
            onSkill = false;
        }
    }
    void Invisibility()
    {
        // 8�ʵ��� ����ȭ
        if (skillTime < 8f)
        {
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();
            skillTime = 0;
            onSkill = false;
        }
    }
    void GetBarrier()
    {
        player.isBarrier = true;
        skillTime = 0;
    }
    void DoubleJump()
    {
        // 10�ʵ��� �������� ����
        if (skillTime < 10f)
        {
            player.isdoubleJump = true;
        }
        else
        {
            player.isdoubleJump = false;
            skillTime = 0;
            onSkill = false;
        }
    }
    void Buster()
    {
        // 8�ʵ��� ���� �ν�Ʈ
        if (skillTime < 7f)
        {
            player.BustOn();
        }
        else if (skillTime < 8f)
        {
            player.BustOff();
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();
            skillTime = 0;
            onSkill = false;
        }
    }
    void SushiAttack()
    {
        if (player.bullet.activeSelf)
        {
            onSkill = false;
            return;
        }

        ReRoad();
        player.bullet.SetActive(true);
        skillTime = 0;
        onSkill = false;
    }
    public void ReRoad()
    {
        player.bullet.transform.position = player.transform.position;
    }
    void AllKill()
    {
        for (int i = 0; i < 11; i++)
        {
            objectManager.DisableEnemy(i);
        }
        // 3�ʵ��� �߰� �� ��������
        spawnTimer = 0;
        spawnDelay = 3;

        skillTime = 0;
        onSkill = false;
    }
    void GiantJelly()
    {
        // 10�ʵ��� ���� �Ŵ�ȭ
        if (skillTime < 8f)
        {
            player.GrowUp();
        }
        else if (skillTime < 10f)
        {
            player.GrowDown();
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();
            skillTime = 0;
            onSkill = false;
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
        SceneManager.LoadScene("InGameScene");
    }
}
