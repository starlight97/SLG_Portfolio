using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIVarietyStore : UIBase
{
    private UIShop uiShop;
    private UIInventoryUpgrade uIInventoryUpgrade;
    private UIUpgradeState uIUpgradeState;

    public UnityAction onInventoryUpgradeComplete;

    public override void Init()
    {
        base.Init();
        this.uiShop = this.transform.Find("UIShop").GetComponent<UIShop>();
        this.uIInventoryUpgrade = this.transform.Find("UIInventoryUpgrade").GetComponent<UIInventoryUpgrade>();
        this.uIUpgradeState = this.transform.Find("UIUpgradeState").GetComponent<UIUpgradeState>();
        this.uiShop.onExitClick = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.HideShopUI();
        };
        this.uIInventoryUpgrade.onFailedExpansionInventory = () => {
            uIInventoryUpgrade.gameObject.SetActive(false);
            uIUpgradeState.gameObject.SetActive(true);
            uIUpgradeState.SetText(false);
        };
        this.uIInventoryUpgrade.onSuccessExpansionInventory = () => {
            uIInventoryUpgrade.gameObject.SetActive(false);
            uIUpgradeState.gameObject.SetActive(true);
            uIUpgradeState.SetText(true);
            this.uiMenu.RePainting(36);
            this.uiInGameMenu.RePainting(12);           
            onInventoryUpgradeComplete();
        };
                
        this.uiShop.Init();
        this.uIInventoryUpgrade.Init();
        this.uIUpgradeState.Init();
    }

    public void ShowShopUI()
    {
        this.uiInGameMenu.gameObject.SetActive(false);
        this.uiShop.gameObject.SetActive(true);
        this.uiMenu.RePainting(36);
    }

    public void HideShopUI()
    {
        this.uiShop.gameObject.SetActive(false);
        this.uiInGameMenu.gameObject.SetActive(true);
        this.uiInGameMenu.RePainting(12);
    }

    public void ShowUIInventoryUpgrade()
    {
        this.uIInventoryUpgrade.gameObject.SetActive(true);
    }

}
