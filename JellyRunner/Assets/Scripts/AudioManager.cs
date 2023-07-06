using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // 슬라이더를 통한 볼륨조절 변수
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
        // 오디오 믹서에 볼륨 값 넣기
        audioMixer.SetFloat("BGM", Variables.bgmVolume);
        audioMixer.SetFloat("SFX", Variables.sfxVolume);

        // 슬라이더에 볼륨 값 넣기
        sliderBGM.value = Variables.bgmVolume;
        sliderSFX.value = Variables.sfxVolume;
    }

    // 슬라이더를 통한 볼륨조절 함수들
    public void BGMControl()
    {
        Variables.bgmVolume = sliderBGM.value;

        // 현재 -40을 최소값으로 설정해주었으므로 -40이 되버리면 아예 꺼버림(오디오믹서의 소리 최소값은 -80)
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
