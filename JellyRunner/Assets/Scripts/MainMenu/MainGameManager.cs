using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    public int pageNum;
    public Image jellyImageInPanel;
    public string[] jellyNameList;
    public string[] skillNameList;
    public float[] skillDurationList;
    public float[] skillCoolList;
    public Sprite[] jellySpriteList;
    public TextMeshProUGUI jellyName;
    public TextMeshProUGUI jellySkillName;
    public TextMeshProUGUI jellySkillText;
    public TextMeshProUGUI jellySkillDuration;
    public TextMeshProUGUI jellySkillCool;

    // ���� ���� �Լ�
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

    public void OpenPanel(int selectNum)
    {
        pageNum = selectNum;

        // ���� ���� ���� ������ ����
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
}
