using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public enum eButtonSound
    {
        Title,
        Menu,
        Exit,
        Portal,
        Tab,
        Shop,
        Fail
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
            bgmVolume = -10;
            sfxVolume = -10;
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

    public void PlaySound(eButtonSound soundType)
    {
        this.sfxAudioSource.PlayOneShot(btnAudioArr[(int)soundType]);
    }

    public void PlayLoopSound(AudioClip audio)
    {
        this.sfxAudioSource.clip = audio;
        this.sfxAudioSource.loop = true;
        this.sfxAudioSource.Play();
    }

    public void StopLoopSound()
    {
        this.sfxAudioSource.Stop();
    }

    public void StopSound()
    {
        this.sfxAudioSource.Stop();
    }

    public void StopBGMSound()
    {
        StopCoroutine(playSoundRoutine);
        bgmAudioSource.Stop();
        playSoundRoutine = null;
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

    public static IEnumerator FadeOut(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 0.5f;

            yield return null;
        }

        //audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        //audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / 0.5f;

            yield return null;
        }

        audioSource.volume = 1f;
    }
}
