using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarietyStoreMain : GameSceneMain
{
    private UIVarietyStore uiVarietyStore;
    private ShopObject[] shopObject;
    
    public override void Init(SceneParams param = null)
    {
        base.Init(param);
        this.uiVarietyStore = this.uiBase.GetComponent<UIVarietyStore>();
        this.shopObject = GameObject.FindObjectsOfType<ShopObject>();
        
        this.shopObject[0].onShowShopUI = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Shop);
            this.uiVarietyStore.ShowShopUI();
        };

        this.shopObject[1].onInventoryUpgrade = () =>
        {
            this.uiVarietyStore.ShowUIInventoryUpgrade();
        };

        this.uiVarietyStore.onInventoryUpgradeComplete = () => {
            shopObject[1].GetComponent<InventoryUpgrade>().SetGameObject(InfoManager.instance.GetInfo().playerInfo.inventory.size);
        };

        shopObject[1].GetComponent<InventoryUpgrade>().Init();
    }

}
