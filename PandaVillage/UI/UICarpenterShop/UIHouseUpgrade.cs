using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHouseUpgrade : MonoBehaviour
{
    private GameObject UIHouseUpgradeText;
    private Button yesButton;
    private Button noButton;
    private GameObject content;

    private GameInfo gameInfo;
    public void Init()
    {
        this.content = this.transform.Find("content").gameObject;
        this.UIHouseUpgradeText = content.transform.Find("UIHouseUpgradeText").gameObject;
        this.yesButton = content.transform.Find("yesButton").GetComponent<Button>();
        this.noButton = content.transform.Find("noButton").GetComponent<Button>();

        yesButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            Debug.Log("yas");
            UpgradeHouse();
        });
        noButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.gameObject.SetActive(false);
        });
        gameInfo = InfoManager.instance.GetInfo();        
        SetUIText(gameInfo.ranchInfo.houseId);
    }

    public void SetUIText(int currentHouseID)
    {      
        var buildingData = DataManager.instance.GetData<BuildingData>(currentHouseID + 1);
        UIHouseUpgradeText.GetComponentInChildren<Text>().text = string.Format(buildingData.description);
    }
    public void UpgradeHouse()
    {
        var upgradeHoudeData = DataManager.instance.GetData<BuildingData>(gameInfo.ranchInfo.houseId + 1);
        var inventory = gameInfo.playerInfo.inventory;

        if (gameInfo.playerInfo.gold >= upgradeHoudeData.require_gold)
        {
            //골드빼주기
        }
        else
        //골드모자람

        if (inventory.GetItemCount(4000) >= upgradeHoudeData.require_wood && inventory.GetItemCount(4001) >= upgradeHoudeData.require_stone)         //나무 4000, 돌 4001
        {
            //인벤토리에서 아이템 빼주기
        }
        //아이템모자람

    }
}

