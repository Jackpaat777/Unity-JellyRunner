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

    [Header("---------------[Audio]")]
    public AudioManager audioManager;

    [Header ("---------------[Jelly]")]
    public Sprite[] jellySpriteList;
    public string[] jellyNameList;
    public Sprite sharkSkillSprite;

    [Header("---------------[Player]")]
    public Player player;
    public Animator playerAc;

    [Header("---------------[Skill]")]
    public float[] skillDurationList;
    public float[] skillDurationUpList;
    public float[] skillCoolList;
    public float[] skillCoolDownList;
    public string[] skillName;
    public Sprite[] skillSpriteList;
    public Button skillButton;
    public Image skillBackImage;
    public Slider skillSlider;
    public GameObject skillSliderArea;
    public float skillDurationTime;
    public float skillCoolTime;
    public float skillTimer;
    public float coolTimer;
    bool onSkill;
    bool onCool;

    [Header("---------------[Spawner]")]
    public ObjectManager objectManager;
    public Transform spawnPoint;
    float spawnTimer;
    float spawnDelay;

    [Header("---------------[InGame]")]
    public float speed;
    public float speedTimer;
    public float speedLevel;
    public float speedUp;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI highScoreText;
    public int groundCount;
    int scoreUp;
    float scoreTimer;

    [Header("---------------[UI]")]
    public TextMeshProUGUI jelatinOver;
    public GameObject optionPanel;
    public GameObject overPanel;
    public Slider speedSlider;

    void Awake()
    {
        instance = this;
        GameStart();
    }

    void GameStart()
    {
        Time.timeScale = 1;
        scoreUp = 1;
        scoreTimer = 0;
        speed = 1;
        skillSlider.value = 0;
        speedSlider.value = 1;

        audioManager.BgmPlay("Game");
        
        // ��ų ���ӽð�, ��Ÿ�� ���ϱ�
        int idx = Variables.jellyTypeNum;
        skillDurationTime = skillDurationList[idx] + (Variables.skillLevel[idx] * skillDurationUpList[idx]);
        skillCoolTime = skillCoolList[idx] - (Variables.skillLevel[idx] * skillCoolDownList[idx]);
    }

    void Update()
    {
        // Speed Level
        // ���� �ð����� ���ǵ� ����
        speedTimer += Time.deltaTime;
        speedSlider.value = speedTimer / 30;
        if (speedTimer > 30)
        {
            speedLevel += 0.5f;
            // �ִ밪 ����
            if (speedLevel > 10.5f)
                speedLevel = 10.5f;

            // Level Text
            speedText.text = "Lv." + (speedLevel * 2 - 1);

            speedTimer = 0;
        }
        // ���� speed ��
        speed = speedLevel + speedUp;

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
        skillBackImage.sprite = skillSpriteList[Variables.jellyTypeNum];
        skillButton.image.sprite = skillBackImage.sprite;

        if (onCool)     // ��Ÿ�� ����
        {
            coolTimer += Time.deltaTime;
            SkillCoolImage();

            // ��Ÿ�� �ð��� ������
            if (coolTimer > skillCoolTime)
            {
                CoolTimeOff();
            }
        }
        else            // ��Ÿ�� ����
        {
            if (onSkill)    // ��Ÿ���� ������ ��ų �ߵ� ����
            {
                skillTimer += Time.deltaTime;
                SkillDurationSlider();
                ExecuteJellySkill();
            }
        }


    }

    public void StopSwitch()
    {
        // ������ ���߰� Ű�� ����ġ (��ư���۵� ���Ƴ���)
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            skillButton.interactable = false;
        }
        else
        {
            Time.timeScale = 1;
            skillButton.interactable = true;
        }
    }
    public void OptionButton()
    {
        StopSwitch();
        if (Time.timeScale == 0)
        {
            optionPanel.SetActive(true);
            // Audio
            audioManager.SfxPlay("Pause In");
        }
        else
        {
            optionPanel.SetActive(false);
            // Audio
            audioManager.SfxPlay("Pause Out");
        }
    }
    void SpawnEnemy()
    {
        // ���� �� ���� (������Ʈ�Ŵ������� �������� �ε����� �Ѱ���)
        int randNum = Random.Range(0, jellySpriteList.Length);
        GameObject enemy = objectManager.Get(randNum);
        enemy.transform.position = spawnPoint.position;

    }

    // ------------- ��ų ���� �Լ���
    public void OnSkill()
    {
        if (onCool)
            return;

        onSkill = true;

        // Audio
        if (Variables.jellyTypeNum == 8)
            audioManager.SfxPlay("Bullet");
        else
            audioManager.SfxPlay("Skill");
    }
    public void OffSkill()
    {
        onSkill = false;
    }
    public void OnCool()
    {
        onCool = true;
    }
    void CoolTimeOff()
    {
        skillButton.interactable = true;
        coolTimer = 0;
        onCool = false;
    }
    void SkillDurationSlider()
    {
        if (skillDurationTime == 0)
            return;

        skillSlider.value = 1 - (skillTimer / skillDurationTime);
    }
    void SkillCoolImage()
    {
        skillButton.image.fillAmount = 1 - (coolTimer / skillCoolTime);

        if (skillButton.image.fillAmount <= 0)
            skillButton.image.fillAmount = 1;
    }
    void ExecuteJellySkill()
    {
        switch (Variables.jellyTypeNum)
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
    void SpeedUp()
    {
        // ���ǵ� 2 ����
        if (skillTimer < skillDurationTime)
        {
            speedUp = 2;
        }
        else
        {
            speedUp = 0;

            skillTimer = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void JumpUp()
    {
        // ���� ����
        if (skillTimer < skillDurationTime)
        {
            player.jumpPowerUp = 2;
        }
        else
        {
            player.jumpPowerUp = 0;

            skillTimer = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void ScoreUp()
    {
        // ���ھ� 2��
        if (skillTimer < skillDurationTime)
        {
            scoreUp = 2;
        }
        else
        {
            scoreUp = 1;

            skillTimer = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void PressDown()
    {
        // ��� ����
        if (skillTimer < skillDurationTime)
        {
            player.skillType = Player.SkillType.PRESS_DOWN;
        }
        else
        {
            player.skillType = Player.SkillType.NONE;

            skillTimer = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void Invisibility()
    {
        // ����ȭ
        if (skillTimer < skillDurationTime)
        {
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();

            skillTimer = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void GetBarrier()
    {
        player.skillType = Player.SkillType.BARRIER;
        skillTimer = 0;
    }
    void DoubleJump()
    {
        // �������� ����
        if (skillTimer < skillDurationTime)
        {
            player.skillType = Player.SkillType.DOUBLE_JUMP;
        }
        else
        {
            player.skillType = Player.SkillType.NONE;

            skillTimer = 0;
            onSkill = false;
            onCool = true;
        }
    }
    void Buster()
    {
        // ���� �ν�Ʈ
        if (skillTimer < skillDurationTime - 1f)
        {
            player.BustOn();
            speedUp = 5;
        }
        else if (skillTimer < skillDurationTime)
        {
            player.BustOff();
            speedUp = 0;
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();

            skillTimer = 0;
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
        skillTimer = 0;
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
        for (int i = 0; i < jellySpriteList.Length; i++)
        {
            objectManager.DisableEnemy(i);
        }
        // �߰� �� ��������
        spawnTimer = 0;
        spawnDelay = 5;

        skillTimer = 0;
        onSkill = false;
        onCool = true;
    }
    void GiantJelly()
    {
        // ���� �Ŵ�ȭ
        if (skillTimer < skillDurationTime - 2f)
        {
            player.GrowUp();
        }
        else if (skillTimer < skillDurationTime)
        {
            player.GrowDown();
            player.AlphaDown();
        }
        else
        {
            player.AlphaUp();

            skillTimer = 0;
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
        
        // Audio
        audioManager.SfxPlay("Explosion");
    }


    // ------------- �����۵� ���� �Լ���
    public void GameOver()
    {
        jelatinOver.text = "+ " + score.ToString("F0");
        MainGameManager.instance.AddJelatin(score);
        StopSwitch();
        overPanel.SetActive(true);

        // High Score Update
        Variables.highScore = Mathf.Max(Variables.highScore, score);
        highScoreText.text = "High : " + Variables.highScore.ToString("F0");

        // Audio
        audioManager.SfxPlay("Over");
        audioManager.BgmStop();

        // Save
        MainGameManager.instance.GameSave();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("1.MainMenuScene");
    }
}
