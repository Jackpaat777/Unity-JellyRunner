using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public Animator fadeAc;

    private void Awake()
    {
        VolumeLoad();
        audioManager.BgmPlay("Title");
    }

    void Update()
    {if (Input.GetMouseButtonDown(0))
        {
            GoToGame();
        }
    }

    public void GoToGame()
    {
        fadeAc.SetTrigger("doFadeOut");
        audioManager.SfxPlay("Unlock");

        StartCoroutine(GoToGameExe());
    }
    IEnumerator GoToGameExe()
    {
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("1.MainMenuScene");
    }

    void VolumeLoad()
    {
        Variables.bgmVolume = PlayerPrefs.GetFloat("Bgm");
        Variables.sfxVolume = PlayerPrefs.GetFloat("Sfx");
    }
}
