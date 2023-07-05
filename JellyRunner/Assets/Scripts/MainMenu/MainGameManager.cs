using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

// 씬이 이동해도 계속 유지되어야하는 변수들
public static class Variables
{
    public static int jelatin = 0;
    public static int gold = 0;
    public static int jellyTypeNum = 0;
    public static bool[] isLock = { false, true, true, true, true, true, true, true, true, true, true };
    //public static bool[] isLock = { false, false, false, false, false, false, false, false, false, false, false };
}


public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    [Header("---------------[Audio]")]
    public AudioManager audioManager;

    [Header("---------------[Main UI]")]
    public Jelly[] jellyInMenu;
    public TextMeshProUGUI jelatinText;
    public TextMeshProUGUI goldText;
    public GameObject optionPanel;

    [Header("---------------[Infomation]")]
    public string[] jellyNameList;
    public string[] skillNameList;
    public float[] skillDurationList;
    public float[] skillCoolList;
    public Sprite[] jellySpriteList;
    public Sprite[] skillSpriteList;

    [Header("---------------[Collection]")]
    public int collectionPageNum;
    public Image[] imageInCollection;
    public Image[] panelInCollection;
    public Image jellyImageInCollection;
    public Image skillImageInCollection;
    public GameObject collectionPanel;
    public GameObject lockImageInCollection;
    public GameObject lockSkillImageInCollection;
    public TextMeshProUGUI jellyNumText;
    public TextMeshProUGUI jellyNameInCollection;
    public TextMeshProUGUI SubNameInCollection;
    public TextMeshProUGUI jellySkillName;
    public TextMeshProUGUI jellyDetailText;
    public TextMeshProUGUI jellySkillDuration;
    public TextMeshProUGUI jellySkillCool;
    public TextMeshProUGUI jellySkillDetail;

    [Header("---------------[Exchange]")]
    public GameObject exchangePanel;
    public Button exchangeButton;
    public TextMeshProUGUI jelatinInExchange;
    public TextMeshProUGUI goldInExchange;
    public TextMeshProUGUI jelatinRatio;
    public TextMeshProUGUI goldRatio;
    int jelainRatioInt;
    int goldRatioInt;

    [Header("---------------[Store]")]
    public GameObject buyPanel;
    public GameObject okButton;
    public GameObject againButton;
    public GameObject clickButton;
    public GameObject unableText;
    public GameObject newText;
    public Animator cardAc;
    public TextMeshProUGUI storeGoldText;
    bool isNew;
    bool isRollStart;
    float rollSpeed;

    [Header("---------------[Card]")]
    public Sprite[] cardDesign;
    public Image jellyImageInCard;
    public Image cardFrontDesign;
    public Image cardFront;
    public GameObject cardBack;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI gradeName;

    [Header("---------------[Select]")]
    public GameObject selectPanel;
    public GameObject lockImageInSelect;
    public Image jellyImageInSelect;
    public Image jellyPanelInSelect;
    public TextMeshProUGUI jellyNameInSelect;
    public TextMeshProUGUI SubNameInSelect;
    public Button selectButton;


    void Awake()
    {
        // 60프레임
        Application.targetFrameRate = 60;

        audioManager.BgmPlay("Menu");

        Variables.isLock[0] = false;
        instance = this;
        Time.timeScale = 1;

        // 불러오기
        GameLoad();

        // 메인화면씬이 시작되면 젤리 활성화
        UpdateMainJelly();

    }

    void Update()
    {
        // 보유 젤라틴, 골드 텍스트 실시간 업데이트
        jelatinText.text = Variables.jelatin.ToString("N0");
        goldText.text = Variables.gold.ToString("N0");
        storeGoldText.text = Variables.gold.ToString("N0");

        // 카드 돌리기
        if (isRollStart)
        {
            rollSpeed -= Time.timeScale * 0.03f;
            cardAc.SetFloat("rollSpeed", rollSpeed);
            // 카드 멈춤
            if (rollSpeed < 1f)
            {
                rollSpeed = 0;
                cardAc.SetBool("isRoll", false);
                againButton.SetActive(true);
                isRollStart = false;
                NewTextActiveSelf();
                // Audio
                audioManager.SfxPlay("Unlock");
            }
            else if (rollSpeed < 2f)
            {
                cardFront.gameObject.SetActive(false);
            }
            else if (rollSpeed < 2.5f)
            {
                okButton.SetActive(true);
            }
        }
    }

    // 기타 함수
    public void UpdateMainJelly()
    {
        // 뽑기가 끝나고 나올 때도 업데이트 (Return Button)
        for (int i = 0; i < 11; i++)
        {
            if (!Variables.isLock[i])
                jellyInMenu[i].gameObject.SetActive(true);
        }
    }
    public void JelatinButton()
    {
        AddJelatin(1000);
    }
    public void GoldButton()
    {
        AddGold(1000);
    }
    public void AddJelatin(int jelatinPlus)
    {
        Variables.jelatin += jelatinPlus;
        GameSave();
    }
    public void AddGold(int jelatinPlus)
    {
        Variables.gold += jelatinPlus;
        GameSave();
    }
    public void OptionButton()
    {
        if (!optionPanel.activeSelf)
        {
            Time.timeScale = 0;
            optionPanel.SetActive(true);
            audioManager.SfxPlay("Pause In");
        }
        else
        {
            Time.timeScale = 1;
            optionPanel.SetActive(false);
            audioManager.SfxPlay("Pause Out");
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

        // Audio
        audioManager.SfxPlay("Button");
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
        // 젤리 정보
        jellyDetailText.text = DetailText(collectionPageNum);
        // 스킬이름
        jellySkillName.text = skillNameList[collectionPageNum];
        // 스킬 지속시간, 쿨타임
        jellySkillDuration.text = skillDurationList[collectionPageNum].ToString();
        jellySkillCool.text = skillCoolList[collectionPageNum].ToString();
        // 스킬 이미지
        skillImageInCollection.sprite = skillSpriteList[collectionPageNum];
        // 스킬 내용
        jellySkillDetail.text = SkillText(collectionPageNum);

        // 해금여부에 따라 다르게 보이게
        if (Variables.isLock[collectionPageNum])
            LockCollectionPanel();
        else
            UnlockCollectionPanel();

        // Audio
        audioManager.SfxPlay("Button");
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
    string DetailText(int type)
    {
        string typeText = null;

        switch (type)
        {
            case 0:
                typeText = "슬라임 젤리는 매우 호기심이 많은 성격이다.";
                break;
            case 1:
                typeText = "콩 모양의 젤리. 탄성이 좋아 매우 잘 튀어오른다.";
                break;
            case 2:
                typeText = "포도 젤리는 모든 젤리들 중 가장 똑똑해서 곱셈이 가능하다.";
                break;
            case 3:
                typeText = "유일하게 다리가 존재하는 곰돌이 젤리는 적을 밟을 수 있다.";
                break;
            case 4:
                typeText = "사실 푸딩을 누가 먹은게 아니라 푸딩 스스로 사라진게 아닐까?";
                break;
            case 5:
                typeText = "다른 젤리들과 다르게 매우 튼튼하며 거친 성격을 가지고 있다.";
                break;
            case 6:
                typeText = "꿈틀이 젤리는 몸이 매우 유연해서 공중에서도 점프가 가능하다.";
                break;
            case 7:
                typeText = "상어 젤리는 언제 어디든 잠수할 수 있는 특징을 가지고 있다.";
                break;
            case 8:
                typeText = "초밥 젤리가 발사하는 초밥은 본인이 직접 만든다는 소문이 있다.";
                break;
            case 9:
                typeText = "매우 고귀하여 많은 젤리들에게 귀족처럼 여겨지는 젤리이다.";
                break;
            case 10:
                typeText = "평소에는 온화한 성격이지만 화가 나면 아무도 말릴 수 없다.";
                break;
        }

        return typeText;
    }
    string SkillText(int type)
    {
        string typeText = null;

        switch (type)
        {
            case 0:
                typeText = "3초동안 스피드가 증가합니다.";
                break;
            case 1:
                typeText = "8초동안 점프력이 증가합니다.";
                break;
            case 2:
                typeText = "5초동안 점수가 2배 증가합니다.";
                break;
            case 3:
                typeText = "15초동안 적을 밟을 수 있습니다.";
                break;
            case 4:
                typeText = "8초간 투명화되어 적들을 무시할 수 있습니다.";
                break;
            case 5:
                typeText = "한번의 공격을 무시하는 베리어를 생성합니다.\n(베리어가 파괴되면 쿨타임 시작)";
                break;
            case 6:
                typeText = "10초동안 연속으로 두번까지 점프가 가능합니다.";
                break;
            case 7:
                typeText = "부스터를 사용하여 6초동안 적들을 무시하고 스피드가 크게 증가합니다.\n(점프불가)";
                break;
            case 8:
                typeText = "초밥을 발사하고 적을 처치하면 초밥의 크기가 커지며 크기에 비례하여 추가점수를 획득합니다.\n" +
                                            "(추가점수 : 100 (+10), 최대 : 200)";
                break;
            case 9:
                typeText = "화면상의 적을 모두 처치합니다. 처치한 적 하나 당 점수가 200씩 증가합니다.\n" +
                                            "추가로 3초동안 적이 생성되지 않습니다.";
                break;
            case 10:
                typeText = "10초동안 몸집을 키운 뒤 적을 먹어치웁니다. 적을 처치할 때마다 점수가 200씩 증가합니다.\n(점프불가)";
                break;
        }

        return typeText;
    }
    void LockCollection(int typeNum)
    {
        if (Variables.isLock[typeNum])
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
        // 스킬명 비활성화
        jellySkillName.gameObject.SetActive(false);
        // 젤리 정보 비활성화
        jellyDetailText.text = "보유하지 않은 젤리입니다.";
        // 스킬 이미지, 지속시간, 쿨타임, 스킬 내용 잠금
        skillImageInCollection.gameObject.SetActive(false);
        jellySkillDuration.text = "-";
        jellySkillCool.text = "-";
        jellySkillDetail.gameObject.SetActive(false);

    }
    void UnlockCollectionPanel()
    {
        jellyImageInCollection.color = Color.white;

        lockImageInCollection.SetActive(false);
        lockSkillImageInCollection.SetActive(false);

        jellyNameInCollection.gameObject.SetActive(true);
        SubNameInCollection.gameObject.SetActive(true);

        jellySkillName.gameObject.SetActive(true);

        skillImageInCollection.gameObject.SetActive(true);
        jellySkillDetail.gameObject.SetActive(true);
    }

    // 환전소 관련 함수
    public void OpenExchangePanel()
    {
        exchangePanel.SetActive(true);
        CanExchange();

        // 보유 젤라틴과 골드 텍스트 업데이트
        jelatinInExchange.text = Variables.jelatin.ToString();
        goldInExchange.text = Variables.gold.ToString();

        // 젤라틴, 골드 비율 초기화
        jelainRatioInt = 1000;
        goldRatioInt = jelainRatioInt / 10;
        jelatinRatio.text = "1000";
        goldRatio.text = "100";

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void JelatinUpButton()
    {
        // 비율 값 변경
        jelainRatioInt += 100;

        // 보유 젤라틴보다 많아질 경우
        if (jelainRatioInt > Variables.jelatin)
        {
            jelainRatioInt -= 100;
            return;
        }

        goldRatioInt = jelainRatioInt / 10;
        // 비율 텍스트 변경
        jelatinRatio.text = jelainRatioInt.ToString();
        goldRatio.text = goldRatioInt.ToString();

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void JelatinDownButton()
    {
        // 비율 값 변경
        jelainRatioInt -= 100;

        // 기본 젤라틴보다 적어질 경우
        if (jelainRatioInt < 1000)
        {
            jelainRatioInt += 100;
            return;
        }

        goldRatioInt = jelainRatioInt / 10;
        // 비율 텍스트 변경
        jelatinRatio.text = jelainRatioInt.ToString();
        goldRatio.text = goldRatioInt.ToString();

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void AllJelatinButton()
    {
        // 기본 젤라틴보다 적은 경우
        if (Variables.jelatin < 1000)
            return;

        // 비율값을 전액으로 (백단위)
        jelainRatioInt = Variables.jelatin - Variables.jelatin % 100;
        goldRatioInt = jelainRatioInt / 10;
        // 비율 텍스트 변경
        jelatinRatio.text = jelainRatioInt.ToString();
        goldRatio.text = goldRatioInt.ToString();

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void ExchangeButton()
    {
        // 젤라틴과 골드 값 변경 (비율값만큼)
        Variables.jelatin -= jelainRatioInt;
        Variables.gold += jelainRatioInt / 10;

        // 보유 젤라틴과 골드 텍스트 변경
        jelatinInExchange.text = Variables.jelatin.ToString();
        goldInExchange.text = Variables.gold.ToString();

        // 젤라틴, 골드 비율 초기화
        jelainRatioInt = 1000;
        goldRatioInt = jelainRatioInt / 10;
        jelatinRatio.text = "1000";
        goldRatio.text = "100";

        CanExchange();

        // Audio
        audioManager.SfxPlay("Buy");

        // Save
        GameSave();
    }
    void CanExchange()
    {
        // 환전이 가능한지
        if (jelainRatioInt > Variables.jelatin)
            exchangeButton.interactable = false;
        else
            exchangeButton.interactable = true;
    }

    // 상점 관련 함수
    public void BuyJelly()
    {
        // 골드가 부족하다면
        if (Variables.gold < 1000)
        {
            unableText.SetActive(true);
            // Audio
            audioManager.SfxPlay("Fail");
            return;
        }


        // 골드 감소
        Variables.gold -= 1000;
        
        // 화면 패널 열기
        buyPanel.SetActive(true);

        // 텍스트 초기화
        isNew = false;
        newText.SetActive(false);
        unableText.SetActive(false);

        // UI 초기화
        okButton.SetActive(false);
        againButton.SetActive(false);
        clickButton.SetActive(true);
        cardFront.gameObject.SetActive(true);

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void StartRollCard()
    {
        // 카드 돌리기
        isRollStart = true;
        clickButton.SetActive(false);
        cardAc.SetBool("isRoll", true);
        rollSpeed = 3;

        // 카드 뽑기
        int num = Percentage();
        ChangeCard(num);

        // 새로운 카드인지
        if (Variables.isLock[num])
            isNew = true;
        else
            isNew = false;
        Variables.isLock[num] = false;

        // Audio
        audioManager.SfxPlay("Buy");

        // Save
        GameSave();
    }
    public void AgainCard()
    {
        BuyJelly();
        cardAc.SetTrigger("doAgain");
    }
    public void OkButton()
    {
        if (isRollStart)
        {
            // 카드가 돌고 있으면 카드 멈추기
            rollSpeed = 0;
            cardFront.gameObject.SetActive(false);
            againButton.SetActive(true);
            cardAc.SetBool("isRoll", false);
            isRollStart = false;
            NewTextActiveSelf();
            //Audio
            audioManager.SfxPlay("Unlock");
        }
        else
        {
            buyPanel.SetActive(false);
            // Audio
            audioManager.SfxPlay("Button");
        }
    }
    int Percentage()
    {
        int percent = Random.Range(0, 100);

        if (percent < 60)                   // 노말 : 60퍼
            percent = Random.Range(0, 3);
        else if (percent < 90)              // 레어 : 30퍼
            percent = Random.Range(3, 7);
        else if (percent < 97)              // 스페셜 : 8퍼
            percent = Random.Range(7, 9);
        else                            // 레전드 : 2퍼
            percent = Random.Range(9, 11);

        return percent;
    }
    void ChangeCard(int typeNum)
    {
        switch (typeNum)
        {
            case 0:
            case 1:
            case 2:
                cardFrontDesign.sprite = cardDesign[0];
                cardFront.sprite = cardDesign[0];
                gradeName.text = "Normal";
                gradeName.color = Color.black;
                break;
            case 3:
            case 4:
            case 5:
            case 6:
                cardFrontDesign.sprite = cardDesign[1];
                cardFront.sprite = cardDesign[1];
                gradeName.text = "Rare";
                gradeName.color = Color.blue;
                break;
            case 7:
            case 8:
                cardFrontDesign.sprite = cardDesign[2];
                cardFront.sprite = cardDesign[2];
                gradeName.text = "Special";
                gradeName.color = Color.red;
                break;
            case 9:
            case 10:
                cardFrontDesign.sprite = cardDesign[3];
                cardFront.sprite = cardDesign[3];
                gradeName.text = "Legend";
                gradeName.color = Color.yellow;
                break;
        }
        jellyImageInCard.sprite = jellySpriteList[typeNum];
        jellyImageInCard.SetNativeSize();
        cardName.text = jellyNameList[typeNum];
    }
    void NewTextActiveSelf()
    {
        if (isNew)
            newText.SetActive(true);
        else
            newText.SetActive(false);
    }

    // 플레이어 선택 관련 함수
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
        if (Variables.isLock[Variables.jellyTypeNum])
            LockSelectPanel();
        else
            UnlockSelectPanel();

        // Audio
        audioManager.SfxPlay("Button");
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
        audioManager.SfxPlay("Button");
        SceneManager.LoadScene(1);
    }
    public void GameExit()
    {
        audioManager.SfxPlay("Button");
        GameSave();
        Application.Quit();
    }
    public void GameSave()
    {
        // 저장값이 변경되는 순간순간 자동 저장
        // AddJelatin() AddGold() Exchange() StartRollCard() 혹시 모르니까 GameExit()에도
        PlayerPrefs.SetInt("Jelatin", Variables.jelatin);
        PlayerPrefs.SetInt("Gold", Variables.gold);
        for (int i = 0; i < jellySpriteList.Length; i++)
        {
            PlayerPrefs.SetInt("isLock" + i, Variables.isLock[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }
    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("Jelatin"))
            return;

        Variables.jelatin = PlayerPrefs.GetInt("Jelatin");
        Variables.gold = PlayerPrefs.GetInt("Gold");
        for (int i = 0; i < jellySpriteList.Length; i++)
        {
            Variables.isLock[i] = PlayerPrefs.GetInt("isLock" + i) == 1 ? true : false;
        }
    }
}
