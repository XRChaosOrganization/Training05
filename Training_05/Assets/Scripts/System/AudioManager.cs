using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    public enum SFX { Navigate, Confirm, Impact, Bounce, Plouf, Victory }
    public enum BGM { Theme }

    public GameObject sfx;
    public GameObject bgm;

    AudioSource[] sfxList;
    AudioSource[] bgmList;


    private void Awake()
    {
        current = this;
        DontDestroyOnLoad(this);

        GetAudioSources();
    }

    #region Private Methods

    void GetAudioSources()
    {
        sfxList = sfx.GetComponentsInChildren<AudioSource>();
        bgmList = bgm.GetComponentsInChildren<AudioSource>();
    }

    AudioSource BGMSwitch(AudioManager.BGM _bgmSwitch)
    {
        AudioSource target = null;

        switch (_bgmSwitch)
        {
            case AudioManager.BGM.Theme:
                target = bgmList[0];
                break;
            default:
                Debug.LogError(_bgmSwitch + " is not a valid argument");
                break;
        }

        return target;
    }

    AudioSource SFXSwitch(AudioManager.SFX _sfxSwitch)
    {
        AudioSource target = null;

        switch (_sfxSwitch)
        {
            case SFX.Confirm:
                target = bgmList[0];
                break;
            case SFX.Impact:
                target = bgmList[1];
                break;
            case SFX.Bounce:
                target = bgmList[2];
                break;
            case SFX.Plouf:
                target = bgmList[3];
                break;
            case SFX.Victory:
                target = bgmList[4];
                break;
            default:
                Debug.LogError(_sfxSwitch +" is not a valid argument");
                break;
        }


        return target;
    }


    #endregion

    #region Method Overload

    public void Play(AudioManager.BGM _bgm)
    {
        AudioSource target = BGMSwitch(_bgm);
        target.Play();
    }
    public void Play(AudioManager.SFX _sfx)
    {
        AudioSource target = SFXSwitch(_sfx);
        target.Play();
    }

    public void Stop(AudioManager.BGM _bgm)
    {
        AudioSource target = BGMSwitch(_bgm);
        target.Stop();
    }

    public void Stop(AudioManager.SFX _sfx)
    {
        AudioSource target = SFXSwitch(_sfx);
        target.Stop();
    }


    #endregion
}
