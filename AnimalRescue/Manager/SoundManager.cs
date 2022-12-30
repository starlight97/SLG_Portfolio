using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public enum eButtonAudio
    {
        Button1,
        Button2,
        Button3
    }
    public static SoundManager instance;

    public AudioClip[] btnAudioArr;

    public AudioMixer audioMixer;

    private AudioSource bgmAudioSource;
    private AudioSource sfxAudioSource;
    private Coroutine playSoundRoutine;

    private void Awake()
    {
        instance = this;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        AudioSettings.Reset(AudioSettings.GetConfiguration());
    }

    public void Init()
    {
        bgmAudioSource = transform.Find("BGMAudio").GetComponent<AudioSource>();
        sfxAudioSource = transform.Find("SFXAudio").GetComponent<AudioSource>();

        float bgmVolume;
        float sfxVolume;
        if (PlayerPrefs.HasKey("BgmVolume") && PlayerPrefs.HasKey("SfxVolume"))
        {
            bgmVolume = PlayerPrefs.GetFloat("BgmVolume");
            sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        }
        else
        {
            bgmVolume = -20;
            sfxVolume = -20;
        }
        AidioInit(bgmVolume, sfxVolume);
    }

    private void AidioInit(float bgmVolume, float sfxVolume)
    {
        audioMixer.SetFloat("BGM", bgmVolume);
        audioMixer.SetFloat("SFX", sfxVolume);
    }
    public void PlayBGMSound(AudioClip[] audiobgmArr)
    {
        if (playSoundRoutine != null)
            StopBGMSound();
        playSoundRoutine = StartCoroutine(this.PlayBGMSoundRoutine(audiobgmArr));
    }
    public void PlaySound(AudioClip audio)
    {
        this.sfxAudioSource.PlayOneShot(audio);
    }
    public void PlaySound(eButtonAudio eButtonAudio)
    {
        this.sfxAudioSource.PlayOneShot(btnAudioArr[(int)eButtonAudio]);
    }

    public void StopBGMSound()
    {
        StopCoroutine(playSoundRoutine);
        bgmAudioSource.Stop();
        playSoundRoutine = null;
    }
    public void StopSound()
    {
        sfxAudioSource.Stop();
    }

    private IEnumerator PlayBGMSoundRoutine(AudioClip[] audiobgmArr)
    {
        int bgmCount = audiobgmArr.Length;
        int currentIndex = 0;

        while (true)
        {
            if (currentIndex == bgmCount)
            {
                currentIndex = 0;
            }
            yield return new WaitForSeconds(0.5f);
            if (!bgmAudioSource.isPlaying)
            {
                bgmAudioSource.clip = audiobgmArr[currentIndex];
                bgmAudioSource.Play();
                currentIndex++;
            }
        }
    }
}
