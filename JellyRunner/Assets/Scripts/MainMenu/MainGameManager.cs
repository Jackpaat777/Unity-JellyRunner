using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    [Header("---------------[Collection]")]
    public int pageNum;
    public Image jellyImageInPanel;
    public string[] jellyNameList;
    public string[] skillNameList;
    public float[] skillDurationList;
    public float[] skillCoolList;
    public Sprite[] jellySpriteList;
    public TextMeshProUGUI jellyNumText;
    public TextMeshProUGUI jellyName;
    public TextMeshProUGUI jellySkillName;
    public TextMeshProUGUI jellySkillText;
    public TextMeshProUGUI jellySkillDuration;
    public TextMeshProUGUI jellySkillCool;

    [Header("---------------[Option]")]
    public GameObject optionPanel;

    void Awake()
    {
        Time.timeScale = 1;
    }

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    // 도감 관련 함수
    void SkillText(int type)
    {
        switch (type)
        {
            case 0:
                jellySkillText.text = "3초동안 스피드가 증가합니다.";
                break;
            case 1:
                jellySkillText.text = "8초동안 점프력이 증가합니다.";
                break;
            case 2:
                jellySkillText.text = "8초동안 점수가 3배 증가합니다.";
                break;
            case 3:
                jellySkillText.text = "15초동안 적을 밟을 수 있습니다.";
                break;
            case 4:
                jellySkillText.text = "8초간 투명화되어 적들을 무시할 수 있습니다.";
                break;
            case 5:
                jellySkillText.text = "한번의 공격을 무시하는 베리어를 생성합니다.\n(베리어가 없어지면 쿨타임이 돌아갑니다.)";
                break;
            case 6:
                jellySkillText.text = "10초동안 연속으로 두번까지 점프가 가능합니다.";
                break;
            case 7:
                jellySkillText.text = "부스터를 사용하여 6초동안 적들을 무시하고 스피드가 크게 증가합니다.\n(조작불가)";
                break;
            case 8:
                jellySkillText.text = "초밥을 발사하여 적이 죽으면 초밥이 점점 커집니다. 적을 처치하면 초밥의 크기에 비례하여 점수가 추가로 증가합니다.\n" +
                                            "(초기 점수 : 100, 증가량 : 10, 최대 증가량 : 100)";
                break;
            case 9:
                jellySkillText.text = "화면상의 적을 모두 처치합니다. 처치한 적 하나 당 점수가 200씩 증가합니다.\n" +
                                            "추가로 3초동안 적이 생성되지 않습니다.";
                break;
            case 10:
                jellySkillText.text = "크기가 커지면서 적들을 처치할 수 있습니다. 적을 처치할 때마다 점수가 200씩 증가합니다.\n(조작불가)";
                break;
        }
    }
    public void OpenPanel(int selectNum)
    {
        pageNum = selectNum;

        // 젤리 도감 관련 변수들 변경
        jellyNumText.text = (pageNum + 1).ToString();
        jellyImageInPanel.sprite = jellySpriteList[pageNum];
        jellyImageInPanel.SetNativeSize();
        jellyName.text = jellyNameList[pageNum];
        jellySkillName.text = skillNameList[pageNum];
        jellySkillDuration.text =skillDurationList[pageNum].ToString();
        jellySkillCool.text = skillCoolList[pageNum].ToString();
        SkillText(pageNum);
    }
    public void PageUp()
    {
        pageNum++;
        if (pageNum > 10)
            pageNum = 0;
        OpenPanel(pageNum);
    }
    public void PageDown()
    {
        pageNum--;
        if (pageNum < 0)
            pageNum = 10;
        OpenPanel(pageNum);
    }

    // 게임작동 관련 함수
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
}
