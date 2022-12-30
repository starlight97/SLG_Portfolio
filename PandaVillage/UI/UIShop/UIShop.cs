using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.Events;
using System.IO;
using System.Linq;
using System;

public enum eShopCategory
{
    GeneralStore,   //잡화점
    CarpenterShop,  //목공소
    MarniesRanch,   //마니의목장

}
public enum eDataType
{
    Tool, Materials, Seed,
}

public class UIShop : MonoBehaviour
{
    public UnityAction onExitClick;
    private UIShopItemScrollView uIShopItemScrollView;
    private UIShopItemBuy uIShopItemBuy;
    private UIPlayerGold uIShopPlayerGold;
    private Button inventoryButton;
    private Button exitButton;

    private GameInfo gameInfo;


    public eShopCategory eShopCategory;
    public AudioClip buyItemCilp;


    public void Test2(IEnumerable<ShopData> shopData)
    {
        foreach (var shop in shopData)
        {
            if (shop.shop_category == (int)eShopCategory)
            {
                //Test4(shop);
                Test5(shop);
            }
        }       

        //===========================================================
        gameInfo = InfoManager.instance.GetInfo();
        ButtonInit();

    }
   
    //이전버전
    public void Test4(ShopData shop)
    {
        int i = shop.item_id / 1000;        
        switch (i)
        {
            case 1:
                var seedData = DataManager.instance.GetData<SeedData>(shop.item_id);
                uIShopItemScrollView.SetItem(seedData);
                break;
            case 4:
                var materialData = DataManager.instance.GetData<MaterialData>(shop.item_id);
                uIShopItemScrollView.SetItem(materialData);
                break;
            case 6:
                var toolData = DataManager.instance.GetData<ToolData>(shop.item_id);
                uIShopItemScrollView.SetItem(toolData);
                break;

            default: break;
        }
    }

    //통일장이론
    public void Test5(ShopData shop)
    {
        var typeDef = DataManager.instance.GetData(shop.item_id).GetType();
        var data = DataManager.instance.GetType().GetMethod(nameof(DataManager.instance.GetData), 1, new Type[] { typeof(int) })
            .MakeGenericMethod(typeDef).Invoke(DataManager.instance, new object[] { shop.item_id });
        
        uIShopItemScrollView.SetItem(data);        
    }
    

    public void Init()
    {
        //InfoManager.instance.LoadData();
        this.uIShopItemScrollView = this.transform.Find("UIShopItemScrollView").GetComponent<UIShopItemScrollView>();
        this.uIShopItemBuy = this.transform.Find("UIShopItemBuy").GetComponent<UIShopItemBuy>();
        this.uIShopPlayerGold = this.transform.Find("UIPlayerGold").GetComponent<UIPlayerGold>();
        this.inventoryButton = this.transform.Find("InventoryButton").GetComponent<Button>();
        this.exitButton = this.transform.Find("ExitButton").GetComponent<Button>();
                
        var shopData = DataManager.instance.GetDataList<ShopData>();

        this.uIShopPlayerGold.Init();
        this.uIShopItemBuy.Init();
        Test2(shopData);
    }

    private UIShopItem selectedItem;
    private void ButtonInit()
    {
        uIShopItemScrollView.Init();

        

        exitButton.onClick.AddListener(() => 
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.onExitClick();
        });


        inventoryButton.onClick.AddListener(() => {

            Debug.Log("인벤토리 팝업, 아이템 판매기능");
        });

        //아이템이 선택되면
        uIShopItemScrollView.onItemSelected = (item) => {

            uIShopItemBuy.SetText(item);
            uIShopItemBuy.SetSliderMaxValue(gameInfo.playerInfo.gold, item.price);    //슬라이더의 최댓값은 플레이어가 살수있는 최대수량이어야함
            selectedItem = item;
        };

        var uiShopItems = uIShopItemScrollView.GetComponentsInChildren<UIShopItem>();
        foreach (var item in uiShopItems)
        {
            item.onItemSelected = (item) => {
                uIShopItemBuy.SetText(item);
                uIShopItemBuy.SetSliderMaxValue(gameInfo.playerInfo.gold, item.price);    //슬라이더의 최댓값은 플레이어가 살수있는 최대수량이어야함
                selectedItem = item;
            };
        }
        this.uIShopPlayerGold.onChangeGold(gameInfo.playerInfo.gold);


        this.uIShopItemBuy.buyButtonClicked = (amount) => {
            var bill = selectedItem.price * amount;
            Debug.Log(bill);
            if (gameInfo.playerInfo.gold - bill >= 0)
            {
                if (BuyingItem(amount, selectedItem))
                {
                    SoundManager.instance.PlaySound(buyItemCilp);
                    gameInfo.playerInfo.gold = gameInfo.playerInfo.gold - bill;
                    this.uIShopPlayerGold.onChangeGold(gameInfo.playerInfo.gold);
                    InfoManager.instance.UpdateInfo(gameInfo);
                    InfoManager.instance.SaveGame();
                }
                else
                {
                    SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
                }
            }
            else
            {
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
            }
        };

        uIShopItemScrollView.FirstItemSelect();
    }

    //구매성공하면 true 아니면 false
    public bool BuyingItem(int selectedItemAmount, UIShopItem selectedItem)
    { 
        var dicPlayerInventory = gameInfo.playerInfo.inventory;
        return dicPlayerInventory.AddItem(selectedItem.id, selectedItemAmount);
    }
}
