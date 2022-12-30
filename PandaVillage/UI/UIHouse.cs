using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHouse : UIBase
{
    private UISleepCheck uiSleepCheck;
    public UnityAction<bool> onSleepCheck;

    public override void Init()
    {
        base.Init();

        this.uiSleepCheck = transform.Find("UISleepCheck").GetComponent<UISleepCheck>();
        uiSleepCheck.gameObject.SetActive(false);


        this.uiSleepCheck.Init();

        this.uiSleepCheck.onSleepCheck = (check) =>
        {
            this.onSleepCheck(check);
        };
    }

    public void ShowSleepCheckUi()
    {
        uiSleepCheck.gameObject.SetActive(true);
    }
    public void HideSleepCheckUi()
    {
        uiSleepCheck.gameObject.SetActive(false);
    }
}
