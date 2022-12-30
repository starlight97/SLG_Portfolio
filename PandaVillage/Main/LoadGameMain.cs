using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameMain : SceneMain
{
    private UILoadGame uILoadGame;
    public AudioClip[] loadGameClip;

    public override void Init(SceneParams param = null)
    {
        base.Init(param);

        this.uILoadGame = GameObject.FindObjectOfType<UILoadGame>();

        uILoadGame.Init();

        uILoadGame.onSelectedPlayerId = (selectedId) =>
        {
            InfoManager.instance.Init(selectedId);
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Title);
            SoundManager.instance.StopBGMSound();

            Dispatch("onLoadGame");
        };
        uILoadGame.onExitButtonClick = () => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            SoundManager.instance.StopBGMSound();
            Dispatch("onExitBtnClick");
        };

        SoundManager.instance.PlayBGMSound(loadGameClip);
    }
}
