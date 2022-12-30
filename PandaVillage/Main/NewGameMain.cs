using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMain : SceneMain
{
    private UINewGame uINewGame;
    public AudioClip[] newGameClip;

    public override void Init(SceneParams param = null)
    {
        base.Init();
        uINewGame = GameObject.FindObjectOfType<UINewGame>();
        uINewGame.Init();

        uINewGame.onClickButton = (gameinfo) =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Title);
            SoundManager.instance.StopBGMSound();
            this.CreateUser(gameinfo);
        };
        uINewGame.onExitButtonClick = () => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            SoundManager.instance.StopBGMSound();
            Dispatch("onExitBtnClick");
        };

        SoundManager.instance.PlayBGMSound(newGameClip);
    }


    private void CreateUser(GameInfo gameinfo)
    {       
        InfoManager.instance.InsertInfo(gameinfo);
        InfoManager.instance.Init(gameinfo.playerId);
        Dispatch("onCreateUser");
    }


}
