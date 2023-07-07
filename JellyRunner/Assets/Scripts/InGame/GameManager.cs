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

    [Header("---------------[InGame]")]
    public float speed;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI highScoreText;
    float speedTimer;
    float speedLevel;
    float speedUp;
    float scoreTimer;
    int scoreUp;
    bool isOver;
    bool isOption;

    [Header ("---------------[Information]")]
    public Sprite[] jellySpriteList;
    public Sprite[] skillSpriteList;
    public float[] skillDurationList;
    public float[] skillDurationUpList;
    public float[] skillCoolList;
    public float[] skillCoolDownList;

    [Header("---------------[Player]")]
    public Player player;
    public Animator playerAc;

    [Header("---------------[Skill]")]
    public Button skillButton;
    public Image skillBackImage;
    public Slider skillSlider;
    public GameObject skillSliderArea;
    float skillDurationTime;
    float skillCoolTime;
    float skillTimer;
    float coolTimer;
    bool onSkill;
    bool onCool;

    [Header("---------------[Spawner]")]
    public ObjectManager objectManager;
    public Transform spawnPoint;
    float spawnTimer;
    float spawnDelay;

    [Header("---------------[UI]")]
    public TextMeshProUGUI jelatinOver;
    public GameObject optionPanel;
    public GameObject overPanel;
    public Slider speedSlider;
    public Animator fadeAc;

    [Header("---------------[Audio]")]
    public AudioManager audioManager;

    void Awake()
    {
        instance = this;
        GameStart();
    }
    void GameStart()
    {
        // 초기화
        speed = 1;
        speedTimer = 0;
        speedLevel = 1;
        speedUp = 0;

        score = 0;
        scoreTimer = 0;
        scoreUp = 1;

        skillSlider.value = 0;
        speedSlider.value = 1;

        isOver = false;
        isOption = false;

        player.gameObject.SetActive(true);
        
        // 스킬 지속시간, 쿨타임 구하기
        int idx = Variables.jellyTypeNum;
        skillDurationTime = skillDurationList[idx] + (Variables.skillLevel[idx] * skillDurationUpList[idx]);
        skillCoolTime = skillCoolList[idx] - (Variables.skillLevel[idx] * skillCoolDownList[idx]);

        // 스킬 이미지
        skillBackImage.sprite = skillSpriteList[Variables.jellyTypeNum];
        skillButton.image.sprite = skillBackImage.sprite;

        audioManager.BgmPlay("Game");
        fadeAc.SetTrigger("doFadeIn");
    }

    void Update()
    {
        SpeedLevel();
        Scoring();
        Spawner();
        Skill();
    }
    void SpeedLevel()
    {
        // 일정 시간마다 스피드 증가
        speedTimer += Time.deltaTime;
        speedSlider.value = speedTimer / 30;
        if (speedTimer > 30)
        {
            speedLevel += 0.5f;
            // 최대값 제한
            if (speedLevel > 10.5f)
                speedLevel = 10.5f;

            // Level Text
            if (!isOver)
                levelText.text = "Stage " + (speedLevel * 2 - 1);

            speedTimer = 0;
        }
        // 최종 speed 값
        speed = speedLevel + speedUp;
    }
    void Scoring()
    {
        if (isOver)
            return;

        scoreTimer += Time.deltaTime;
        if (scoreTimer > 0.1f)
        {
            // 스피드에 따른 점수올리기
            score += (int)speed * 2 * scoreUp;
            scoreText.text = score.ToString("N0");
            scoreTimer = 0;
        }
    }
    void Spawner()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnDelay)
        {
            SpawnEnemy();
            spawnTimer = 0;
            spawnDelay = Random.Range(1.5f, 3f);
        }
    }
    void Skill()
    {
        if (isOver)
            return;

        if (onCool)     // 쿨타임 시작
        {
            coolTimer += Time.deltaTime;
            SkillCoolImage();

            // 쿨타임 시간이 지나면
            if (coolTimer > skillCoolTime)
            {
                CoolTimeOff();
            }
        }
        else            // 쿨타임 종료
        {
            if (onSkill)    // 쿨타임이 끝나면 스킬 발동 가능
            {
                skillTimer += Time.deltaTime;
                SkillDurationSlider();
                ExecuteJellySkill();
            }
        }
    }

    public void OptionButton()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            optionPanel.SetActive(true);
            // Audio
            audioManager.SfxPlay("Pause In");
        }
        else
        {
            Time.timeScale = 1;
            optionPanel.SetActive(false);
            // Audio
            audioManager.SfxPlay("Pause Out");
        }
    }
    void SpawnEnemy()
    {
        // 랜덤 적 생성 (오브젝트매니저에서 랜덤값의 인덱스가 넘겨짐)
        int randNum = Random.Range(0, skillSpriteList.Length);
        GameObject enemy = objectManager.Get(randNum);
        enemy.transform.position = spawnPoint.position;
    }

    // ------------- 스킬 관련 함수들
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

        // 스킬버튼 누르면 쿨타임동안 못누르게
        skillButton.interactable = false;
    }
    void SpeedUp()
    {
        // 스피드 2 증가
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
        // 점프 증가
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
        // 스코어 2배
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
        // 밟기 가능
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
        // 투명화
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
        // 더블점프 가능
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
        // 무적 부스트
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

        // 초밥 발사
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
        // 올 킬
        for (int i = 0; i < skillSpriteList.Length; i++)
        {
            objectManager.DisableEnemy(i);
        }
        // 추가 적 생성막기
        spawnTimer = 0;
        spawnDelay = 5;

        skillTimer = 0;
        onSkill = false;
        onCool = true;
    }
    void GiantJelly()
    {
        // 무적 거대화
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


    // ------------- 게임작동 관련 함수들
    public void GameOver()
    {
        isOver = true;

        // Player
        CallExplosion(player.transform.position + Vector3.up * 0.5f);
        player.gameObject.SetActive(false);

        if (isOption)
            return;

        overPanel.SetActive(true);
        jelatinOver.text = "+ " + score.ToString("F0");
        MainGameManager.instance.AddJelatin(score);

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
        fadeAc.SetTrigger("doFadeOut");
        audioManager.SfxPlay("Button");

        MainGameManager.instance.GameSave();

        StartCoroutine(GoToMenuExe(2));
    }
    public void GoToMenu()
    {
        // 타임스케일은 1로 만들기 (옵션에서 타임스케일값이 0임)
        Time.timeScale = 1;

        // 순간적으로 게임오버되어도 bool변수 덕에 게임오버와 관련된 내용은 나오지 않음
        isOption = true;


        fadeAc.SetTrigger("doFadeOut");
        audioManager.SfxPlay("Button");

        MainGameManager.instance.GameSave();

        StartCoroutine(GoToMenuExe(1));
    }
    IEnumerator GoToMenuExe(int type)
    {
        yield return new WaitForSeconds(1.0f);

        if (type == 1)
            SceneManager.LoadScene("1.MainMenuScene");
        else if (type == 2)
            SceneManager.LoadScene("2.InGameScene");
    }
}
