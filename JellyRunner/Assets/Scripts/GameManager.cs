using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    public float time;
    public float speed;

    void Awake()
    {
        // 처음에는 시간 멈추기
        StopSwitch();
    }

    void Update()
    {
        // 0.1초마다 점수가 오름
        time += Time.deltaTime;
        if (time > 0.1f)
        {
            // 스피드에 따른 점수올리기
            score += (int)(speed * 2);
            scoreText.text = score.ToString("N0");
            time = 0;
        }

        // Skill
        if (onSkill)
        {
            skillTime += Time.deltaTime;
            UniqueSkill();
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

    // 게임을 멈추고 키는 스위치
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
    

    // ------------- 버튼 관련 함수들
    public void PageUp()
    {
        jellyNum++;
        if (jellyNum == 10)
            rightButton.SetActive(false);
        else // pageUp을 누를 때 jellyNum이 0~9
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
        else // pageDown을 누를 때 jellyNum이 1~10
            rightButton.SetActive(true);

        jellyImageInPanel.sprite = jellySpriteList[jellyNum];
        jellyImageInPanel.SetNativeSize();
        jellyNameInPanel.text = jellyNameList[jellyNum];
    }
    public void SpeedUp()
    {
        speed += 0.5f;
        playerAc.SetFloat("runSpeed", speed);
    }
    public void SpeedDown()
    {
        if (speed > 0.5f)
        {
            speed -= 0.5f;
            playerAc.SetFloat("runSpeed", speed);
        }

    }

    public void OnSkill()
    {
        onSkill = true;
    }

    // 고유 스킬 발동
    void UniqueSkill()
    {
        switch (jellyNum)
        {
            case 0:
                SpeedUpSkill();
                break;
            case 1:
                JumpUpSkill();
                break;
            case 2:
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

    void SpeedUpSkill()
    {
        // 3초동안 스피드 3배 업
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

    void JumpUpSkill()
    {
        // 10초동안 점프 증가
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

    public void RestartGame()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
