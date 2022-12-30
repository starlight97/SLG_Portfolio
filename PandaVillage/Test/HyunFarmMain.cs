using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HyunFarmMain : SceneMain
{
    private UIBase ui;
    private Player player;
    private MapManager mapManager;
    private TileManager tileManager;
    private TimeManager timeManager;
    private CropManager cropManager;

    private void Start()
    {
        this.Init();
    }

    public override void Init(SceneParams param = null)
    {
        InfoManager.instance.Init(10);
        DataManager.instance.Init();
        DataManager.instance.LoadAllData(this);

        InfoManager.instance.LoadData();
        this.player = GameObject.FindObjectOfType<Player>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();
        this.tileManager = GameObject.FindObjectOfType<TileManager>();
        this.timeManager = GameObject.FindObjectOfType<TimeManager>();
        this.cropManager = GameObject.FindObjectOfType<CropManager>();
        this.ui = GameObject.FindObjectOfType<UIBase>();
        this.timeManager.Init();
        this.tileManager.Init();
        ui.Init();
        DataManager.instance.onDataLoadFinished.AddListener(() =>
        {
            this.cropManager.Init();
        });
        

        #region PlayerAction
        this.player.onDecideTargetTile = (startPos, targetPos, pathList) =>
        {
            this.mapManager.PathFinding(startPos, targetPos, pathList);
            this.player.Move();
        };
        // 타일이 있냐?
        this.player.onGetTile = (pos, state) =>
        {
            bool check = tileManager.CheckTile(pos, state);
            if (check)
                player.FarmingAct(pos);
        };

        // 타일 변경
        this.player.onChangeFarmTile = (pos, state) =>
        {
            //tileManager.SetTile(pos, state);
        };

        // 씨앗 뿌리기
        this.player.onPlantCrop = (id, pos) =>
        {
            cropManager.CreateCrop(id, pos);
        };

        this.player.onShowAnimalUI = (animal) =>
        {
            //if (animal != null)
            //    uiVillage.ShowAnimalUI(animal.animalName, animal.friendship, animal.age);
            //else
            //    uiVillage.HideAnimalUI();
        };

        this.player.onGetItem = () =>
        {
            this.ui.UpdateInventory();
        };
        #endregion

        #region TimeManagerAction
        TimeManager.instance.onUpdateTime = (hour, minute) =>
        {
            var info = InfoManager.instance.GetInfo();
            // 10분당 1로 저장
            // ex 하루 = 1320 분
            // 하루마다 132 씩 ++
            //info.playerInfo.playMinute += 132;

            if (hour == 6 && minute == 1)
            {
                InfoManager.instance.SaveCrop(cropManager.cropList);
                InfoManager.instance.EndDay();
                Debug.Log("1시에요 하루가 끝났어요");
                timeManager.EndDay();
                //cropManager.CheckWateringDirt();
                tileManager.ClearWateringTiles();
            }
        };
        #endregion

        this.tileManager.onFinishedSetTile = (pos) => 
        {
            //InfoManager.instance.SaveTilePos(tileManager.posList);
        };

        #region CropManagerAction
        this.cropManager.onGetFarmTile = (id, pos, crop) =>
        {
            bool check = tileManager.CheckTile(pos, TileManager.eTileType.WateringDirt);
            if (check == true)
            {
                crop.wateringCount++;
            }
        };
        #endregion

        DataManager.instance.onDataLoadFinished.AddListener(() =>
        {

        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //InfoManager.instance.SaveCrop(cropManager.cropList);
        }
    }
}
