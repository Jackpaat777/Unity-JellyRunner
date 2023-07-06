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

    void Start()
    {
        // 오디오 믹서에 볼륨 값 넣기
        audioMixer.SetFloat("BGM", Variables.BgmVolume);
        audioMixer.SetFloat("SFX", Variables.SfxVolume);

        // 슬라이더에 볼륨 값 넣기
        sliderBGM.value = Variables.BgmVolume;
        sliderSFX.value = Variables.SfxVolume;
    }

    // 슬라이더를 통한 볼륨조절 함수들
    public void BGMControl()
    {
        Variables.BgmVolume = sliderBGM.value;

        // 현재 -40을 최소값으로 설정해주었으므로 -40이 되버리면 아예 꺼버림(오디오믹서의 소리 최소값은 -80)
        if (Variables.BgmVolume == -40f)
            audioMixer.SetFloat("BGM", -80);
        else
            audioMixer.SetFloat("BGM", Variables.BgmVolume);

        MainGameManager.instance.GameSave();
    }

    public void SFXControl()
    {
        Variables.SfxVolume = sliderSFX.value;

        if (Variables.SfxVolume == -40f)
            audioMixer.SetFloat("SFX", -80);
        else
            audioMixer.SetFloat("SFX", Variables.SfxVolume);

        MainGameManager.instance.GameSave();
    }

    public void BgmPlay(string type)
    {
        switch (type)
        {
            case "Menu":
                bgmPlayer.clip = bgmClip[0];
                break;
            case "Game":
                bgmPlayer.clip = bgmClip[1];
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
