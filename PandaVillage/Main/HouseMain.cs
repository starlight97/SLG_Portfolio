using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseMain : GameSceneMain
{
    private UIHouse uiHouse;
    private Bed bed;

    public override void Init(SceneParams param = null)
    {
        base.Init(param);
        this.bed = GameObject.FindObjectOfType<Bed>();
        
        bed.onTrigger = () =>
        {
            this.uiHouse.ShowSleepCheckUi();
        };
        
        this.uiHouse = this.uiBase.GetComponent<UIHouse>();

        this.uiHouse.onSleepCheck = (check) =>
        {
            if(check == true)
            {
                Debug.Log("잤어요");
                tileManager.ClearWateringTiles();
                TimeManager.instance.EndDay();
                InfoManager.instance.EndDay();
                Dispatch("EndDay");
            }
            else
            {
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
                this.uiHouse.HideSleepCheckUi();
            }
        };
    }
}
