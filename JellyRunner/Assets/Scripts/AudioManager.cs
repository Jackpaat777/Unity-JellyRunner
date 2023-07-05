using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // �����̴��� ���� �������� ����
    public AudioMixer audioMixer;
    public Slider sliderBGM;
    public Slider sliderSFX;

    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;
    public AudioClip[] bgmClip;
    public AudioClip[] sfxClip;

    void Start()
    {
        // ���� ���۵� �� �����̴��� ��ġ�� ���� ������ �����ϰ� ���� >> (�ΰ��� -> ����ȭ�� / ����ȭ�� -> �ΰ���) �� ��ȯ �� �����̴� �ʱ���ġ ����
        // slider�� ��ġ�� �������� ����
        float  valueBGM, valueSFX;
        // ����� �ͼ��� ���� ���� value�� ������ (value�� �������� �ʴ� ��� false�� ��ȯ��)
        bool resultBGM = audioMixer.GetFloat("BGM", out valueBGM);
        bool resultSFX = audioMixer.GetFloat("SFX", out valueSFX);

        // slider�� ��ġ ��ȯ
        if (resultBGM)
            sliderBGM.value = valueBGM;
        if (resultSFX)
            sliderSFX.value = valueSFX;
    }

    // �����̴��� ���� �������� �Լ���
    public void BGMControl()
    {
        float sound = sliderBGM.value;

        // ���� -40�� �ּҰ����� �������־����Ƿ� -40�� �ǹ����� �ƿ� ������(������ͼ��� �Ҹ� �ּҰ��� -80)
        if (sound == -40f) audioMixer.SetFloat("BGM", -80);
        else audioMixer.SetFloat("BGM", sound);
    }

    public void SFXControl()
    {
        float sound = sliderSFX.value;

        if (sound == -40f) audioMixer.SetFloat("SFX", -80);
        else audioMixer.SetFloat("SFX", sound);
    }

    public void BgmPlay(string type)
    {
        switch (type)
        {
            case "Menu":
                bgmPlayer.clip = bgmClip[0];
                Debug.Log(type);
                break;
            case "Game":
                bgmPlayer.clip = bgmClip[1];
                Debug.Log(type);
                break;
            case "Over":
                bgmPlayer.clip = bgmClip[2];
                Debug.Log(type);
                break;
        }

        bgmPlayer.Play();
        Debug.Log("Play");
    }

    public void SfxPlay(string type)
    {
        switch (type)
        {
            case "Button":
                sfxPlayer.clip = sfxClip[0];
                break;
            case "Buy":
                sfxPlayer.clip = sfxClip[1];
                break;
            case "Unlock":
                sfxPlayer.clip = sfxClip[2];
                break;
            case "Fail":
                sfxPlayer.clip = sfxClip[3];
                break;
            case "Pause In":
                sfxPlayer.clip = sfxClip[4];
                break;
            case "Pause Out":
                sfxPlayer.clip = sfxClip[5];
                break;
            case "Jump":
                sfxPlayer.clip = sfxClip[6];
                break;
            case "Skill":
                sfxPlayer.clip = sfxClip[7];
                break;
            case "Bullet":
                sfxPlayer.clip = sfxClip[8];
                break;
            case "Explosion":
                sfxPlayer.clip = sfxClip[9];
                break;
        }

        sfxPlayer.Play();
    }
}
