using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 씬이 이동해도 계속 유지되어야하는 변수들
public static class Variables
{
    public static int jelatin;
    public static int gold;
    public static int jellyTypeNum;
}


public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    [Header("---------------[Main UI]")]
    public TextMeshProUGUI jelatinText;
    public GameObject optionPanel;

    [Header("---------------[Infomation]")]
    public bool[] isLockJelly;
    public string[] jellyNameList;
    public string[] skillNameList;
    public float[] skillDurationList;
    public float[] skillCoolList;
    public Sprite[] jellySpriteList;

    [Header("---------------[Collection]")]
    public int collectionPageNum;
    public Image[] imageInCollection;
    public Image[] panelInCollection;
    public Image jellyImageInCollection;
    public TextMeshProUGUI jellyNumText;
    public TextMeshProUGUI jellyNameInCollection;
    public TextMeshProUGUI SubNameInCollection;
    public TextMeshProUGUI jellySkillName;
    public TextMeshProUGUI jellySkillDuration;
    public TextMeshProUGUI jellySkillCool;
    public TextMeshProUGUI jellySkillText;
    public GameObject collectionPanel;
    public GameObject lockImageInCollection;
    public GameObject lockSkillImageInCollection;

    [Header("---------------[Select]")]
    public Image jellyImageInSelect;
    public Image jellyPanelInSelect;
    public TextMeshProUGUI jellyNameInSelect;
    public TextMeshProUGUI SubNameInSelect;
    public Button selectButton;
    public GameObject selectPanel;
    public GameObject lockImageInSelect;

    [Header("---------------[Store]")]
    bool isRollStart;
    float rollSpeed;
    public Animator cardAc;
    public GameObject cardFront;
    public GameObject okButton;
    public GameObject buyPanel;
    public GameObject clickButton;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1;
    }

    void Update()
    {
        jelatinText.text = Variables.jelatin.ToString("N0");

        // 카드 돌리기
        if (isRollStart)
        {
            rollSpeed -= Time.timeScale * 0.005f;
            cardAc.SetFloat("rollSpeed", rollSpeed);
            // 카드 멈춤
            if (rollSpeed < 1.1f)
            {
                rollSpeed = 0;
                cardAc.SetBool("isRoll", false);
                isRollStart = false;
            }
            else if (rollSpeed < 2f)
            {
                cardFront.SetActive(false);
            }
            else if (rollSpeed < 2.5f)
            {
                okButton.SetActive(true);
            }
        }
    }

    public void OkButton()
    {
        if (isRollStart)
        {
            rollSpeed = 0;
            cardFront.SetActive(false);
            cardAc.SetBool("isRoll", false);
            isRollStart = false;
        }
        else
        {
            buyPanel.SetActive(false);
        }
    }

    public void StartRollCard()
    {
        isRollStart = true;
        clickButton.SetActive(false);

        cardAc.SetBool("isRoll", true);
        rollSpeed = 3;
    }
    
    public void OpenBuyPanel()
    {
        buyPanel.SetActive(true);

        okButton.SetActive(false);
        clickButton.SetActive(true);

        cardFront.SetActive(true);
        cardAc.SetBool("isRoll", false);
    }

    public void JelatinButton()
    {
        AddJelatin(1000);
    }
    public void AddJelatin(int jelatinPlus)
    {
        Variables.jelatin += jelatinPlus;
    }
    public void OptionButton()
    {
        if (!optionPanel.activeSelf)
        {
            Time.timeScale = 0;
            optionPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            optionPanel.SetActive(false);
        }
    }

    // 도감 관련 함수
    public void OpenCollection()
    {
        collectionPanel.SetActive(true);

        for (int i = 0; i < jellySpriteList.Length; i++)
        {
            LockCollection(i);
        }
    }
    void LockCollection(int typeNum)
    {
        if (isLockJelly[typeNum])
        {
            imageInCollection[typeNum].color = Color.black;
            panelInCollection[typeNum].color = new Color(0.7f, 0.7f, 0.7f);
        }
        else
        {
            imageInCollection[typeNum].color = Color.white;
            panelInCollection[typeNum].color = Color.white;
        }
    }
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
    public void OpenCollectionPanel(int selectNum)
    {
        collectionPageNum = selectNum;

        // <<젤리 도감 관련 변수들 변경>>
        // 도감번호
        jellyNumText.text = (collectionPageNum + 1).ToString();
        // 이미지
        jellyImageInCollection.sprite = jellySpriteList[collectionPageNum];
        jellyImageInCollection.SetNativeSize();
        // 이름
        jellyNameInCollection.text = jellyNameList[collectionPageNum];
        // 스킬이름
        jellySkillName.text = skillNameList[collectionPageNum];
        // 스킬 지속시간, 쿨타임
        jellySkillDuration.text = skillDurationList[collectionPageNum].ToString();
        jellySkillCool.text = skillCoolList[collectionPageNum].ToString();
        // 스킬 내용
        SkillText(collectionPageNum);

        // 해금여부에 따라 다르게 보이게
        if (isLockJelly[collectionPageNum])
            LockCollectionPanel();
        else
            UnlockCollectionPanel();
    }
    public void PageUpInCollection()
    {
        collectionPageNum++;
        if (collectionPageNum > jellySpriteList.Length - 1)
            collectionPageNum = 0;

        OpenCollectionPanel(collectionPageNum);
    }
    public void PageDownInCollection()
    {
        collectionPageNum--;
        if (collectionPageNum < 0)
            collectionPageNum = jellySpriteList.Length - 1;
        OpenCollectionPanel(collectionPageNum);
    }
    void LockCollectionPanel()
    {
        // 젤리 이미지 검게 칠하기
        jellyImageInCollection.color = Color.black;
        // 잠금 이미지 활성화
        lockImageInCollection.SetActive(true);
        lockSkillImageInCollection.SetActive(true);
        // 젤리 이름 비활성화
        jellyNameInCollection.gameObject.SetActive(false);
        SubNameInCollection.gameObject.SetActive(false);
        // 스킬명, 스킬 지속시간, 쿨타임, 스킬 내용 잠금
        jellySkillName.gameObject.SetActive(false);
        jellySkillDuration.text = "-";
        jellySkillCool.text = "-";
        jellySkillText.gameObject.SetActive(false);

    }
    void UnlockCollectionPanel()
    {
        jellyImageInCollection.color = Color.white;

        lockImageInCollection.SetActive(false);
        lockSkillImageInCollection.SetActive(false);

        jellyNameInCollection.gameObject.SetActive(true);
        SubNameInCollection.gameObject.SetActive(true);

        jellySkillName.gameObject.SetActive(true);
        jellySkillText.gameObject.SetActive(true);
    }

    // 플레이어 선택 관련 함수
    // 선택한 번호가 인게임씬과 연관이 있기 때문에 전역변수를 사용
    public void OpenSelectPanel()
    {
        selectPanel.SetActive(true);

        // <<젤리 관련 변수들 변경>>
        // 이미지
        jellyImageInSelect.sprite = jellySpriteList[Variables.jellyTypeNum];
        jellyImageInSelect.SetNativeSize();
        // 이름
        jellyNameInSelect.text = jellyNameList[Variables.jellyTypeNum];

        // 해금여부에 따라 다르게 보이게
        if (isLockJelly[Variables.jellyTypeNum])
            LockSelectPanel();
        else
            UnlockSelectPanel();
    }
    public void PageUpInSelect()
    {
        Variables.jellyTypeNum++;
        if (Variables.jellyTypeNum > jellySpriteList.Length - 1)
            Variables.jellyTypeNum = 0;

        OpenSelectPanel();
    }
    public void PageDownInSelect()
    {
        Variables.jellyTypeNum--;
        if (Variables.jellyTypeNum < 0)
            Variables.jellyTypeNum = jellySpriteList.Length - 1;

        OpenSelectPanel();
    }
    void LockSelectPanel()
    {
        jellyImageInSelect.color = Color.black;
        jellyPanelInSelect.color = Color.gray;
        lockImageInSelect.SetActive(true);
        jellyNameInSelect.gameObject.SetActive(false);
        SubNameInSelect.gameObject.SetActive(false);
        selectButton.interactable = false;
    }
    void UnlockSelectPanel()
    {
        jellyImageInSelect.color = Color.white;
        jellyPanelInSelect.color = Color.white;
        lockImageInSelect.SetActive(false);
        jellyNameInSelect.gameObject.SetActive(true);
        SubNameInSelect.gameObject.SetActive(true);
        selectButton.interactable = true;
    }

    // 게임작동 관련 함수
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
