using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManiRanch : UIBase
{
    private UIShop uiShop;
    private UIAnimalShop uIAnimalShop;
    private UIMarniesRanchSelect uIMarniesRanchSelect;
    public UnityAction<int, int> onAnimalBuyButtonClick;

    public override void Init()
    {
        base.Init();
        this.uIMarniesRanchSelect = this.transform.Find("UIMarniesRanchSelect").GetComponent<UIMarniesRanchSelect>();
        this.uiShop = this.transform.Find("UIShop").GetComponent<UIShop>();
        this.uIAnimalShop = this.transform.Find("UIAnimalShop").GetComponent<UIAnimalShop>();


        this.uiShop.onExitClick = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.HideShopUI(uiShop.gameObject);
        };
        this.uIAnimalShop.onExitClick = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.HideShopUI(uIAnimalShop.gameObject);
        };
        this.uIMarniesRanchSelect.onUIAnimalShopClick = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            uIMarniesRanchSelect.gameObject.SetActive(false);
            this.uiInGameMenu.gameObject.SetActive(false);
            this.uIAnimalShop.gameObject.SetActive(true);
        };
        this.uIMarniesRanchSelect.onUIShopClick = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            uIMarniesRanchSelect.gameObject.SetActive(false);
            this.uiInGameMenu.gameObject.SetActive(false);
            this.uiShop.gameObject.SetActive(true);
        };

        //돈이 있을때만 액션 날라옴
        this.uIAnimalShop.onAnimalBuyButtonClick = (selectAnimalId, homeType) => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            onAnimalBuyButtonClick(selectAnimalId, homeType);
        };
        this.uIMarniesRanchSelect.Init();
        this.uiShop.Init();
        this.uIAnimalShop.Init();
    }

    public void ShowShopUI()
    {
        Debug.Log("UIManiRanch");
        this.uIMarniesRanchSelect.gameObject.SetActive(true);
        
    }

    public void HideShopUI(GameObject go)
    {
        go.SetActive(false);
        this.uiInGameMenu.gameObject.SetActive(true);        
        this.uiInGameMenu.RePainting(12);
        this.uiMenu.RePainting(36);
    }
}
