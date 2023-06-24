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
        // ó������ �ð� ���߱�
        StopSwitch();
    }

    void Update()
    {
        // 0.1�ʸ��� ������ ����
        time += Time.deltaTime;
        if (time > 0.1f)
        {
            // ���ǵ忡 ���� �����ø���
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

    // ���� ��ų �ߵ�
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

    void JumpUpSkill()
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

    public void RestartGame()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
