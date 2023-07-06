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
    public string bgmName;

    void Start()
    {
        // ����� �ͼ��� ���� �� �ֱ�
        audioMixer.SetFloat("BGM", Variables.bgmVolume);
        audioMixer.SetFloat("SFX", Variables.sfxVolume);

        // �����̴��� ���� �� �ֱ�
        sliderBGM.value = Variables.bgmVolume;
        sliderSFX.value = Variables.sfxVolume;
    }

    // �����̴��� ���� �������� �Լ���
    public void BGMControl()
    {
        Variables.bgmVolume = sliderBGM.value;

        // ���� -40�� �ּҰ����� �������־����Ƿ� -40�� �ǹ����� �ƿ� ������(������ͼ��� �Ҹ� �ּҰ��� -80)
        if (Variables.bgmVolume == -40f)
            audioMixer.SetFloat("BGM", -80);
        else
            audioMixer.SetFloat("BGM", Variables.bgmVolume);

        MainGameManager.instance.GameSave();
    }

    public void SFXControl()
    {
        Variables.sfxVolume = sliderSFX.value;

        if (Variables.sfxVolume == -40f)
            audioMixer.SetFloat("SFX", -80);
        else
            audioMixer.SetFloat("SFX", Variables.sfxVolume);

        MainGameManager.instance.GameSave();
    }

    public void BgmPlay(string type)
    {
        if (bgmName == type)
            return;

        switch (type)
        {
            case "Menu":
                bgmPlayer.clip = bgmClip[0];
                bgmName = "Menu";
                break;
            case "Game":
                bgmPlayer.clip = bgmClip[1];
                bgmName = "Game";
                break;
            case "Card":
                bgmPlayer.clip = bgmClip[2];
                bgmName = "Card";
                break;
        }

        bgmPlayer.Play();
    }

    public void BgmStop()
    {
        bgmPlayer.Stop();
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
            case "Land":
                sfxPlayer.clip = sfxClip[7];
                break;
            case "Skill":
                sfxPlayer.clip = sfxClip[8];
                break;
            case "Bullet":
                sfxPlayer.clip = sfxClip[9];
                break;
            case "Explosion":
                sfxPlayer.clip = sfxClip[10];
                break;
            case "Over":
                sfxPlayer.clip = sfxClip[11];
                break;
        }

        sfxPlayer.PlayOneShot(sfxPlayer.clip);
    }
}
