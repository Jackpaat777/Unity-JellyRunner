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
        // 씬이 시작될 때 슬라이더의 위치는 현재 볼륨과 동일하게 설정 >> (인게임 -> 메인화면 / 메인화면 -> 인게임) 씬 전환 시 슬라이더 초기위치 조정
        // slider의 위치를 지정해줄 변수
        float  valueBGM, valueSFX;
        // 오디오 믹서의 현재 값을 value에 저장함 (value가 존재하지 않는 경우 false를 반환함)
        bool resultBGM = audioMixer.GetFloat("BGM", out valueBGM);
        bool resultSFX = audioMixer.GetFloat("SFX", out valueSFX);

        // slider의 위치 변환
        if (resultBGM)
            sliderBGM.value = valueBGM;
        if (resultSFX)
            sliderSFX.value = valueSFX;
    }

    // 슬라이더를 통한 볼륨조절 함수들
    public void BGMControl()
    {
        float sound = sliderBGM.value;

        // 현재 -40을 최소값으로 설정해주었으므로 -40이 되버리면 아예 꺼버림(오디오믹서의 소리 최소값은 -80)
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
