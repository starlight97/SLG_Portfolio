using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoop : UIBase
{
    private UIAnimalState uIAnimalState;
    public override void Init()
    {
        base.Init();
        uIAnimalState = this.transform.Find("UIAnimalState").GetComponent<UIAnimalState>();
        uIAnimalState.Init();

        uIAnimalState.onHideState = () => {
            HideAnimalUI();
        };
    }
    public void ShowAnimalUI(string name, int friendship, int age)
    {
        uIAnimalState.gameObject.SetActive(true);
        uIAnimalState.ShowAnimalUI(name, friendship, age);
    }
    public void HideAnimalUI()
    {
        uIAnimalState.gameObject.SetActive(false);
    }
    public void GetProductItem()
    {
        this.uiInGameMenu.RePainting(12);
    }
}
