using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMain : SceneMain
{
    public Button btnNewGame;
    public Button btnLoadGame;
    public AudioClip[] titleClips;
    public override void Init(SceneParams param = null)
    {
        this.useOnDestoryEvent = false;
        btnNewGame.onClick.AddListener(() => 
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Title);
            SoundManager.instance.StopBGMSound();
            Dispatch("onClickNewGame");
        
        });

        btnLoadGame.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Title);
            SoundManager.instance.StopBGMSound();
            Dispatch("onClickLoadGame");
        });

        SoundManager.instance.PlayBGMSound(titleClips);

    }


}
