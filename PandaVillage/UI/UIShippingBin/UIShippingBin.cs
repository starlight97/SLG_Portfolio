using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class UIShippingBin : MonoBehaviour
{
    public UIInventory uIInventory;
    private UIShippingBinItem uIShippingBinItem;
    private Button closeButton;

    public UnityAction onExitClick;
    public UnityAction onLastShippingItemButton;
    public void Init()
    {
        this.uIInventory = transform.Find("UIInventory").GetComponent<UIInventory>();
        this.uIShippingBinItem = transform.Find("UIShippingBinItem").GetComponent<UIShippingBinItem>();
        this.closeButton = transform.Find("closeButton").GetComponent<Button>();

        uIInventory.Init(UIInventory.eInventoryType.Ui, 36);


        uIShippingBinItem.Init();
        uIInventory.RePainting(36);

        uIInventory.onShippingItem = (lastItem) => {
            ShippingItem(lastItem);
            uIShippingBinItem.ShippedItem();
        };
        uIShippingBinItem.onLastShippingItemButton = () => {
            var lastItem = InfoManager.instance.GetInfo().playerInfo.lastShippedItem;
            var salePrice = GetSaleGold(lastItem.itemId);
            InfoManager.instance.GetInfo().playerInfo.inventory.AddItem(lastItem.itemId, lastItem.amount);
            InfoManager.instance.GetInfo().playerInfo.dailySaleGold -= salePrice * lastItem.amount;
            uIInventory.RePainting(36);
        };
        closeButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            onExitClick();
        });

    }

    public void ShippingItem(InventoryData lastItem)
    {
        var salePrice = GetSaleGold(lastItem.itemId);
        InfoManager.instance.GetInfo().playerInfo.dailySaleGold += lastItem.amount * salePrice;
    }

    public int GetSaleGold(int itemID)
    {
        int i = itemID / 1000;
        int salePrice;
        switch (i)
        {
            case 1:
                salePrice = DataManager.instance.GetData<SeedData>(itemID).sale_price;
                break;
            case 3:
                salePrice = DataManager.instance.GetData<GatheringData>(itemID).sale_price;
                break;

            case 4:
                salePrice = DataManager.instance.GetData<MaterialData>(itemID).sale_price;
                break;
            

            default: Debug.Log("이상해요"); return -1;
        }
        return salePrice;
    }

}
