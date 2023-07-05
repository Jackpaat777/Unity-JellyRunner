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

// ���� �̵��ص� ��� �����Ǿ���ϴ� ������
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
        // 60������
        Application.targetFrameRate = 60;

        audioManager.BgmPlay("Menu");

        Variables.isLock[0] = false;
        instance = this;
        Time.timeScale = 1;

        // �ҷ�����
        GameLoad();

        // ����ȭ����� ���۵Ǹ� ���� Ȱ��ȭ
        UpdateMainJelly();

    }

    void Update()
    {
        // ���� ����ƾ, ��� �ؽ�Ʈ �ǽð� ������Ʈ
        jelatinText.text = Variables.jelatin.ToString("N0");
        goldText.text = Variables.gold.ToString("N0");
        storeGoldText.text = Variables.gold.ToString("N0");

        // ī�� ������
        if (isRollStart)
        {
            rollSpeed -= Time.timeScale * 0.03f;
            cardAc.SetFloat("rollSpeed", rollSpeed);
            // ī�� ����
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

    // ��Ÿ �Լ�
    public void UpdateMainJelly()
    {
        // �̱Ⱑ ������ ���� ���� ������Ʈ (Return Button)
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

    // ���� ���� �Լ�
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

        // <<���� ���� ���� ������ ����>>
        // ������ȣ
        jellyNumText.text = (collectionPageNum + 1).ToString();
        // �̹���
        jellyImageInCollection.sprite = jellySpriteList[collectionPageNum];
        jellyImageInCollection.SetNativeSize();
        // �̸�
        jellyNameInCollection.text = jellyNameList[collectionPageNum];
        // ���� ����
        jellyDetailText.text = DetailText(collectionPageNum);
        // ��ų�̸�
        jellySkillName.text = skillNameList[collectionPageNum];
        // ��ų ���ӽð�, ��Ÿ��
        jellySkillDuration.text = skillDurationList[collectionPageNum].ToString();
        jellySkillCool.text = skillCoolList[collectionPageNum].ToString();
        // ��ų �̹���
        skillImageInCollection.sprite = skillSpriteList[collectionPageNum];
        // ��ų ����
        jellySkillDetail.text = SkillText(collectionPageNum);

        // �رݿ��ο� ���� �ٸ��� ���̰�
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
                typeText = "������ ������ �ſ� ȣ����� ���� �����̴�.";
                break;
            case 1:
                typeText = "�� ����� ����. ź���� ���� �ſ� �� Ƣ�������.";
                break;
            case 2:
                typeText = "���� ������ ��� ������ �� ���� �ȶ��ؼ� ������ �����ϴ�.";
                break;
            case 3:
                typeText = "�����ϰ� �ٸ��� �����ϴ� ������ ������ ���� ���� �� �ִ�.";
                break;
            case 4:
                typeText = "��� Ǫ���� ���� ������ �ƴ϶� Ǫ�� ������ ������� �ƴұ�?";
                break;
            case 5:
                typeText = "�ٸ� ������� �ٸ��� �ſ� ưư�ϸ� ��ģ ������ ������ �ִ�.";
                break;
            case 6:
                typeText = "��Ʋ�� ������ ���� �ſ� �����ؼ� ���߿����� ������ �����ϴ�.";
                break;
            case 7:
                typeText = "��� ������ ���� ���� ����� �� �ִ� Ư¡�� ������ �ִ�.";
                break;
            case 8:
                typeText = "�ʹ� ������ �߻��ϴ� �ʹ��� ������ ���� ����ٴ� �ҹ��� �ִ�.";
                break;
            case 9:
                typeText = "�ſ� ����Ͽ� ���� �����鿡�� ����ó�� �������� �����̴�.";
                break;
            case 10:
                typeText = "��ҿ��� ��ȭ�� ���������� ȭ�� ���� �ƹ��� ���� �� ����.";
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
                typeText = "3�ʵ��� ���ǵ尡 �����մϴ�.";
                break;
            case 1:
                typeText = "8�ʵ��� �������� �����մϴ�.";
                break;
            case 2:
                typeText = "5�ʵ��� ������ 2�� �����մϴ�.";
                break;
            case 3:
                typeText = "15�ʵ��� ���� ���� �� �ֽ��ϴ�.";
                break;
            case 4:
                typeText = "8�ʰ� ����ȭ�Ǿ� ������ ������ �� �ֽ��ϴ�.";
                break;
            case 5:
                typeText = "�ѹ��� ������ �����ϴ� ����� �����մϴ�.\n(����� �ı��Ǹ� ��Ÿ�� ����)";
                break;
            case 6:
                typeText = "10�ʵ��� �������� �ι����� ������ �����մϴ�.";
                break;
            case 7:
                typeText = "�ν��͸� ����Ͽ� 6�ʵ��� ������ �����ϰ� ���ǵ尡 ũ�� �����մϴ�.\n(�����Ұ�)";
                break;
            case 8:
                typeText = "�ʹ��� �߻��ϰ� ���� óġ�ϸ� �ʹ��� ũ�Ⱑ Ŀ���� ũ�⿡ ����Ͽ� �߰������� ȹ���մϴ�.\n" +
                                            "(�߰����� : 100 (+10), �ִ� : 200)";
                break;
            case 9:
                typeText = "ȭ����� ���� ��� óġ�մϴ�. óġ�� �� �ϳ� �� ������ 200�� �����մϴ�.\n" +
                                            "�߰��� 3�ʵ��� ���� �������� �ʽ��ϴ�.";
                break;
            case 10:
                typeText = "10�ʵ��� ������ Ű�� �� ���� �Ծ�ġ��ϴ�. ���� óġ�� ������ ������ 200�� �����մϴ�.\n(�����Ұ�)";
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
        // ���� �̹��� �˰� ĥ�ϱ�
        jellyImageInCollection.color = Color.black;
        // ��� �̹��� Ȱ��ȭ
        lockImageInCollection.SetActive(true);
        lockSkillImageInCollection.SetActive(true);
        // ���� �̸� ��Ȱ��ȭ
        jellyNameInCollection.gameObject.SetActive(false);
        SubNameInCollection.gameObject.SetActive(false);
        // ��ų�� ��Ȱ��ȭ
        jellySkillName.gameObject.SetActive(false);
        // ���� ���� ��Ȱ��ȭ
        jellyDetailText.text = "�������� ���� �����Դϴ�.";
        // ��ų �̹���, ���ӽð�, ��Ÿ��, ��ų ���� ���
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

    // ȯ���� ���� �Լ�
    public void OpenExchangePanel()
    {
        exchangePanel.SetActive(true);
        CanExchange();

        // ���� ����ƾ�� ��� �ؽ�Ʈ ������Ʈ
        jelatinInExchange.text = Variables.jelatin.ToString();
        goldInExchange.text = Variables.gold.ToString();

        // ����ƾ, ��� ���� �ʱ�ȭ
        jelainRatioInt = 1000;
        goldRatioInt = jelainRatioInt / 10;
        jelatinRatio.text = "1000";
        goldRatio.text = "100";

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void JelatinUpButton()
    {
        // ���� �� ����
        jelainRatioInt += 100;

        // ���� ����ƾ���� ������ ���
        if (jelainRatioInt > Variables.jelatin)
        {
            jelainRatioInt -= 100;
            return;
        }

        goldRatioInt = jelainRatioInt / 10;
        // ���� �ؽ�Ʈ ����
        jelatinRatio.text = jelainRatioInt.ToString();
        goldRatio.text = goldRatioInt.ToString();

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void JelatinDownButton()
    {
        // ���� �� ����
        jelainRatioInt -= 100;

        // �⺻ ����ƾ���� ������ ���
        if (jelainRatioInt < 1000)
        {
            jelainRatioInt += 100;
            return;
        }

        goldRatioInt = jelainRatioInt / 10;
        // ���� �ؽ�Ʈ ����
        jelatinRatio.text = jelainRatioInt.ToString();
        goldRatio.text = goldRatioInt.ToString();

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void AllJelatinButton()
    {
        // �⺻ ����ƾ���� ���� ���
        if (Variables.jelatin < 1000)
            return;

        // �������� �������� (�����)
        jelainRatioInt = Variables.jelatin - Variables.jelatin % 100;
        goldRatioInt = jelainRatioInt / 10;
        // ���� �ؽ�Ʈ ����
        jelatinRatio.text = jelainRatioInt.ToString();
        goldRatio.text = goldRatioInt.ToString();

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void ExchangeButton()
    {
        // ����ƾ�� ��� �� ���� (��������ŭ)
        Variables.jelatin -= jelainRatioInt;
        Variables.gold += jelainRatioInt / 10;

        // ���� ����ƾ�� ��� �ؽ�Ʈ ����
        jelatinInExchange.text = Variables.jelatin.ToString();
        goldInExchange.text = Variables.gold.ToString();

        // ����ƾ, ��� ���� �ʱ�ȭ
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
        // ȯ���� ��������
        if (jelainRatioInt > Variables.jelatin)
            exchangeButton.interactable = false;
        else
            exchangeButton.interactable = true;
    }

    // ���� ���� �Լ�
    public void BuyJelly()
    {
        // ��尡 �����ϴٸ�
        if (Variables.gold < 1000)
        {
            unableText.SetActive(true);
            // Audio
            audioManager.SfxPlay("Fail");
            return;
        }


        // ��� ����
        Variables.gold -= 1000;
        
        // ȭ�� �г� ����
        buyPanel.SetActive(true);

        // �ؽ�Ʈ �ʱ�ȭ
        isNew = false;
        newText.SetActive(false);
        unableText.SetActive(false);

        // UI �ʱ�ȭ
        okButton.SetActive(false);
        againButton.SetActive(false);
        clickButton.SetActive(true);
        cardFront.gameObject.SetActive(true);

        // Audio
        audioManager.SfxPlay("Button");
    }
    public void StartRollCard()
    {
        // ī�� ������
        isRollStart = true;
        clickButton.SetActive(false);
        cardAc.SetBool("isRoll", true);
        rollSpeed = 3;

        // ī�� �̱�
        int num = Percentage();
        ChangeCard(num);

        // ���ο� ī������
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
            // ī�尡 ���� ������ ī�� ���߱�
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

        if (percent < 60)                   // �븻 : 60��
            percent = Random.Range(0, 3);
        else if (percent < 90)              // ���� : 30��
            percent = Random.Range(3, 7);
        else if (percent < 97)              // ����� : 8��
            percent = Random.Range(7, 9);
        else                            // ������ : 2��
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

    // �÷��̾� ���� ���� �Լ�
    public void OpenSelectPanel()
    {
        selectPanel.SetActive(true);

        // <<���� ���� ������ ����>>
        // �̹���
        jellyImageInSelect.sprite = jellySpriteList[Variables.jellyTypeNum];
        jellyImageInSelect.SetNativeSize();
        // �̸�
        jellyNameInSelect.text = jellyNameList[Variables.jellyTypeNum];

        // �رݿ��ο� ���� �ٸ��� ���̰�
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

    // �����۵� ���� �Լ�
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
        // ���尪�� ����Ǵ� �������� �ڵ� ����
        // AddJelatin() AddGold() Exchange() StartRollCard() Ȥ�� �𸣴ϱ� GameExit()����
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
