using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIBuildBuilding : MonoBehaviour
{
    private GameObject uIBuildBuildingPhoto;
    private UIBuildBuildingDescription uIBuildBuildingDescription;
    private Button exitButton;
    private Button buildingMoveButton;
    private Button buildButton;
    private Button buildingDestroyButton;
    private Button leftButton;
    private Button rightButton;

    public SpriteAtlas buildAtlas;

    
    public UnityAction onExitClick;
    public UnityAction<int, int> selectBuildingId;
    public List<BuildingData> bulidingList = new List<BuildingData>();

    private BuildingData selectedBuilding;

    private int index =0;
    public AudioClip btnClip;
        
    public void Init()
    {
        this.uIBuildBuildingPhoto = this.transform.Find("UIBuildBuildingPhoto").gameObject;
        this.uIBuildBuildingDescription = this.transform.Find("UIBuildBuildingDescription").GetComponent<UIBuildBuildingDescription>();
        uIBuildBuildingDescription.Init();

        this.exitButton = this.transform.Find("ExitButton").GetComponent<Button>();
        this.buildingMoveButton = this.transform.Find("BuildingMoveButton").GetComponent<Button>();
        this.buildButton = this.transform.Find("BuildButton").GetComponent<Button>();
        this.buildingDestroyButton = this.transform.Find("BuildingDestroyButton").GetComponent<Button>();
        this.leftButton = this.transform.Find("LeftButton").GetComponent<Button>();
        this.rightButton = this.transform.Find("RightButton").GetComponent<Button>();

        exitButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            onExitClick();
        });
        buildingMoveButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            selectBuildingId(2 , -1);
        });
        buildButton.onClick.AddListener(() => {
            //구매할수 없다면 넘어갈수 없음
            BuildingBuyChecker(selectedBuilding);
        });
        buildingDestroyButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            selectBuildingId(3, -1);
        });
        leftButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(btnClip);
            LeftButtonClick();
        });
        rightButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(btnClip);
            RightButtonClick();
        });



        var shopData = DataManager.instance.GetDataList<ShopData>();        
        foreach (var shop in shopData)
        {
            if (shop.shop_category == 3)
            {
                var building = DataManager.instance.GetData<BuildingData>(shop.item_id);
                this.bulidingList.Add(building);
            }
        }
        
        SetBuildingDesc(bulidingList[index]);
    }

    private void RightButtonClick()
    {
        bool checker;
        do
        {            
            if (index < (bulidingList.Count - 1))
            {
                index++;
                SetBuildingDesc(bulidingList[index]);
            }
            else
            {
                this.index = 0;
                SetBuildingDesc(bulidingList[index]);
            }

            checker = CheckInfoFarmHasBuilding(bulidingList[index].required_upgrade);
            //if (bulidingList[index].required_upgrade)
            //{
            //    checker = true;
            //    var gameInfo = InfoManager.instance.GetInfo();
            //    var farmObject = gameInfo.objectInfoList.Find(x => x.objectId == bulidingList[index].id -1);

            //    if (farmObject != null)                
            //        checker = false;
            //}
            //else
            //    checker = false;
        }
        while(checker);

        
    }

    private void LeftButtonClick()
    {
        bool checker;
        do
        {
            if (index > 0)
            {
                index--;
                SetBuildingDesc(bulidingList[index]);
            }
            else
            {
                this.index = bulidingList.Count - 1;
                SetBuildingDesc(bulidingList[index]);
            }

            checker = CheckInfoFarmHasBuilding(bulidingList[index].required_upgrade);
        }
        while (checker);
    }


    private bool CheckInfoFarmHasBuilding(bool checker)
    {
        if (checker == false)
            return false;

        var gameInfo = InfoManager.instance.GetInfo();
        var farmObject = gameInfo.objectInfoList.Find(x => x.objectId == bulidingList[index].id - 1);

        if (farmObject != null)
            return false;

        return true;
    }
    public void SetBuildingDesc(BuildingData buildingData)
    {        
        var img = uIBuildBuildingPhoto.transform.Find("buildingSprite").GetComponent<Image>();        
        img.sprite = buildAtlas.GetSprite(buildingData.sprite_name);
        img.preserveAspect = true;
        uIBuildBuildingDescription.setDesc(buildingData.buliding_name ,buildingData.description, buildingData.require_gold, buildingData.require_wood, buildingData.require_stone);
        selectedBuilding = buildingData;
    }

    //건물을 살수있는지 체크한후 살수 있을경우 씬이동
    public void BuildingBuyChecker(BuildingData buildingData)
    {
        var playerInfo = InfoManager.instance.GetInfo().playerInfo;        
        var inventory = playerInfo.inventory;

        if (playerInfo.gold >= buildingData.require_gold && inventory.GetItemCount(4000) >= buildingData.require_wood &&
            inventory.GetItemCount(4001) >= buildingData.require_stone)
        {
            selectBuildingId(1, selectedBuilding.id);
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
            Debug.Log("못삼");
        }
    }
}
