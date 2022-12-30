using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.Events;
using System.IO;
using System.Linq;
using System;


public class UIAnimalShop : MonoBehaviour
{
    private UIShopItemScrollView uIShopItemScrollView;
    private UIShopItemBuy uIShopItemBuy;
    private UIPlayerGold uIShopPlayerGold;
    private Button exitButton;

    private GameInfo gameInfo;

   
    public UnityAction onExitClick;
    public UnityAction<int, int> onAnimalBuyButtonClick;
    private void Start()
    {
        //DataManager.instance.Init();
        //DataManager.instance.LoadAllData(this);
        //DataManager.instance.onDataLoadFinished.AddListener(() => {
        //    this.Init();
        //});
        //InfoManager.instance.LoadData();
        //InfoManager.instance.Init(0);
    }

    public void Init()
    {
        //InfoManager.instance.LoadData();
        this.uIShopItemScrollView = this.transform.Find("UIShopItemScrollView").GetComponent<UIShopItemScrollView>();
        this.uIShopItemBuy = this.transform.Find("UIShopItemBuy").GetComponent<UIShopItemBuy>();
        this.uIShopPlayerGold = this.transform.Find("UIPlayerGold").GetComponent<UIPlayerGold>();
        this.exitButton = this.transform.Find("ExitButton").GetComponent<Button>();

        this.uIShopPlayerGold.Init();
        this.uIShopItemBuy.Init();

        //슬라이더 꺼버리기
        this.uIShopItemBuy.transform.Find("BuySlider").gameObject.SetActive(false);

        var animalData = DataManager.instance.GetDataList<AnimalData>();
        foreach (var animal in animalData)
        {
            uIShopItemScrollView.SetItem(animal);
        }



        gameInfo = InfoManager.instance.GetInfo();
        ButtonInit();
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
       

        //아이템이 선택되면
        uIShopItemScrollView.onItemSelected = (item) => {
            uIShopItemBuy.SetText(item);
            //uIShopItemBuy.SetSliderMaxValue(gameInfo.playerInfo.gold, item.price);    //슬라이더의 최댓값은 플레이어가 살수있는 최대수량이어야함
            selectedItem = item;
        };

        var uiShopItems = uIShopItemScrollView.GetComponentsInChildren<UIShopItem>();
        foreach (var item in uiShopItems)
        {
            item.onItemSelected = (item) => {
                uIShopItemBuy.SetText(item);
                //uIShopItemBuy.SetSliderMaxValue(gameInfo.playerInfo.gold, item.price);    //슬라이더의 최댓값은 플레이어가 살수있는 최대수량이어야함
                selectedItem = item;
            };
        }
        this.uIShopPlayerGold.onChangeGold(gameInfo.playerInfo.gold);


        this.uIShopItemBuy.buyButtonClicked = (garbageamount) => {
            var bill = selectedItem.price;
            Debug.Log(bill);

            //씬전환 후 원하는 건물에 동물을 선택한후 다시돌아와서 실행해야함
            if (gameInfo.playerInfo.gold - bill >= 0)
            {
                Debug.Log("돈있음");
                //돈이있는경우에만 액션 실행
                var selectAnimal = DataManager.instance.GetData<AnimalData>(selectedItem.id);
                var homeType = selectAnimal.home_type;
                onAnimalBuyButtonClick(selectedItem.id, homeType);
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);

                //gameInfo.playerInfo.gold = gameInfo.playerInfo.gold - bill;
                //this.uIShopPlayerGold.onChangeGold(gameInfo.playerInfo.gold);
            }
            else
            {
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
            }
        };

        uIShopItemScrollView.FirstItemSelect();
    }

    ////구매성공하면 true 아니면 false
    //public bool BuyingItem(int selectedItemAmount, UIShopItem selectedItem)
    //{
    //    var dicPlayerInventory = gameInfo.playerInfo.inventory;
    //    return dicPlayerInventory.AddItem(selectedItem.id, selectedItemAmount);
    //}

}
