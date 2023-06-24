using System;
using System.Collections;
using System.Collections.Generic;
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
    public Image jellyImageInPanel;
    public TextMeshProUGUI jellyNameInPanel;
    public int jellyNum;

    [Header("---------------[Player]")]
    public Player player;
    public Animator playerAc;

    [Header("---------------[Skill]")]
    public bool onSkill;
    public float skillTime;

    [Header("---------------[InGame]")]
    public bool gameStart;
    public bool isStop;
    public GameObject optionPanel;
    public GameObject blackPanel;
    public GameObject leftButton;
    public GameObject rightButton;
    public TextMeshProUGUI scoreText;
    public int score;
    public float timer;
    public float speed;
    public bool isOver;
    public GameObject overPanel;

    [Header("---------------[Spawner]")]
    public ObjectManager objectManager;
    public float spawnTimer;
    public float spawnDelay;
    public Transform spawnPoints;

    void Awake()
    {
        instance = this;
        // ó������ �ð� ���߱�
        StopSwitch();
    }

    void Update()
    {
        // Scoring
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            // ���ǵ忡 ���� �����ø���
            score += (int)(speed * 2);
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
        }
        else
        {
            Time.timeScale = 1;
            isStop = false;
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
    //public void SpeedUp()
    //{
    //    speed += 0.5f;
    //    playerAc.SetFloat("runSpeed", speed);
    //}
    //public void SpeedDown()
    //{
    //    if (speed > 0.5f)
    //    {
    //        speed -= 0.5f;
    //        playerAc.SetFloat("runSpeed", speed);
    //    }

    //}


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
                AllKill();
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }
    void SpeedUp()
    {
        // 3�ʵ��� ���ǵ� 3�� ��
        if (skillTime < 3f)
        {
            speed = 3;
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
            player.JumpPower = 8;
        }
        else
        {
            player.JumpPower = 6;
            skillTime = 0;
            onSkill = false;
        }
    }
    void AllKill()
    {

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
