using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� �̵��ص� ��� �����Ǿ���ϴ� ������
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

        // ī�� ������
        if (isRollStart)
        {
            rollSpeed -= Time.timeScale * 0.005f;
            cardAc.SetFloat("rollSpeed", rollSpeed);
            // ī�� ����
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

    // ���� ���� �Լ�
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
                jellySkillText.text = "3�ʵ��� ���ǵ尡 �����մϴ�.";
                break;
            case 1:
                jellySkillText.text = "8�ʵ��� �������� �����մϴ�.";
                break;
            case 2:
                jellySkillText.text = "8�ʵ��� ������ 3�� �����մϴ�.";
                break;
            case 3:
                jellySkillText.text = "15�ʵ��� ���� ���� �� �ֽ��ϴ�.";
                break;
            case 4:
                jellySkillText.text = "8�ʰ� ����ȭ�Ǿ� ������ ������ �� �ֽ��ϴ�.";
                break;
            case 5:
                jellySkillText.text = "�ѹ��� ������ �����ϴ� ����� �����մϴ�.\n(����� �������� ��Ÿ���� ���ư��ϴ�.)";
                break;
            case 6:
                jellySkillText.text = "10�ʵ��� �������� �ι����� ������ �����մϴ�.";
                break;
            case 7:
                jellySkillText.text = "�ν��͸� ����Ͽ� 6�ʵ��� ������ �����ϰ� ���ǵ尡 ũ�� �����մϴ�.\n(���ۺҰ�)";
                break;
            case 8:
                jellySkillText.text = "�ʹ��� �߻��Ͽ� ���� ������ �ʹ��� ���� Ŀ���ϴ�. ���� óġ�ϸ� �ʹ��� ũ�⿡ ����Ͽ� ������ �߰��� �����մϴ�.\n" +
                                            "(�ʱ� ���� : 100, ������ : 10, �ִ� ������ : 100)";
                break;
            case 9:
                jellySkillText.text = "ȭ����� ���� ��� óġ�մϴ�. óġ�� �� �ϳ� �� ������ 200�� �����մϴ�.\n" +
                                            "�߰��� 3�ʵ��� ���� �������� �ʽ��ϴ�.";
                break;
            case 10:
                jellySkillText.text = "ũ�Ⱑ Ŀ���鼭 ������ óġ�� �� �ֽ��ϴ�. ���� óġ�� ������ ������ 200�� �����մϴ�.\n(���ۺҰ�)";
                break;
        }
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
        // ��ų�̸�
        jellySkillName.text = skillNameList[collectionPageNum];
        // ��ų ���ӽð�, ��Ÿ��
        jellySkillDuration.text = skillDurationList[collectionPageNum].ToString();
        jellySkillCool.text = skillCoolList[collectionPageNum].ToString();
        // ��ų ����
        SkillText(collectionPageNum);

        // �رݿ��ο� ���� �ٸ��� ���̰�
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
        // ���� �̹��� �˰� ĥ�ϱ�
        jellyImageInCollection.color = Color.black;
        // ��� �̹��� Ȱ��ȭ
        lockImageInCollection.SetActive(true);
        lockSkillImageInCollection.SetActive(true);
        // ���� �̸� ��Ȱ��ȭ
        jellyNameInCollection.gameObject.SetActive(false);
        SubNameInCollection.gameObject.SetActive(false);
        // ��ų��, ��ų ���ӽð�, ��Ÿ��, ��ų ���� ���
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

    // �÷��̾� ���� ���� �Լ�
    // ������ ��ȣ�� �ΰ��Ӿ��� ������ �ֱ� ������ ���������� ���
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

    // �����۵� ���� �Լ�
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
