using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

public class FarmMain : GameSceneMain
{
    private UIFarm uiFarm;
    private CropManager cropManager;
    private RanchManager ranchManager;
    private ObjectPlaceManager objectPlaceManager;
    public CoopInfo enterCoopInfo;
    public AudioClip[] farmBgms;
    public List<AudioClip> farmClips;

    public override void Init(SceneParams param)
    {
        base.Init(param);
        this.uiFarm = GameObject.FindObjectOfType<UIFarm>();
        this.ranchManager = GameObject.FindObjectOfType<RanchManager>();
        this.cropManager = GameObject.FindObjectOfType<CropManager>();
        this.objectPlaceManager = GameObject.FindObjectOfType<ObjectPlaceManager>();

        this.objectManager.Init(App.eMapType.Farm, tileManager.GetTilesPosList(TileManager.eTileType.Dirt));
        this.ranchManager.Init();

        var info = InfoManager.instance.GetInfo();
        if (info.isNewbie == true)
        {
            info.isNewbie = false;

            this.objectManager.SpawnRuckObjects(1000);
            this.objectManager.SpawnGrass(10, mapManager.mapTopRight.y, mapManager.mapTopRight.x, mapManager.GetWallPosArr());
        }

        if (info.dicVisited[App.eMapType.Farm] == false)
        {
            var currAudioSoruce = farmBgms[Random.Range(0, 3)];
            farmClips.Insert(0, currAudioSoruce);
            SoundManager.instance.PlayBGMSound(farmClips.ToArray());
            info.dicVisited[App.eMapType.Farm] = true;
            this.objectManager.SpawnRuckObjects(Random.Range(0, 10));
        }

        InfoManager.instance.SaveOtherObject(App.eMapType.Farm, this.objectManager.GetOtherObjectist());


        this.player.onShowAnimalUI = (animal) =>
        {
            if(animal != null)
                uiFarm.ShowAnimalUI(animal.animalName, animal.friendship, animal.age);                
            else
                uiFarm.HideAnimalUI();
        };

        this.player.onShowStateUI = (silo) =>
        {
            if (silo != null)
                uiFarm.ShowSiloHayAmountUI(ranchManager.hay, ranchManager.maxHay);
        };

        this.player.onCutGrassComplete = () => 
        {
            var check = ranchManager.CheckSilo();
            if (check)
                ranchManager.AddHay();
            else
                return;
        };

        this.player.onFillHay = (hayAmount) => 
        {
            ranchManager.AddHays(hayAmount);
            uiBase.UpdateInventory();
        };

        // 씨앗 뿌리기
        this.player.onPlantCrop = (id, pos) =>
        {
            cropManager.CreateCrop(id, pos);
        };


        #region TimeManagerAction
        TimeManager.instance.onUpdateTime = (hour, minute) =>
        {
            var info = InfoManager.instance.GetInfo();

            uiBase.TimeUpdate(hour, minute);
            
            if (hour == 18)
                ranchManager.AnimalsGoHome();

            if (hour == 26)
            {
                Debug.Log("새벽 2시예요 하루가 끝났어요");                
                InfoManager.instance.SaveOtherObject(App.eMapType.Farm, this.objectManager.GetOtherObjectist());

                tileManager.ClearWateringTiles();
                TimeManager.instance.EndDay();
                InfoManager.instance.EndDay();
                Dispatch("EndDay");
            }
        };
        #endregion

        #region TileManagerAction
        this.tileManager.onFinishedSetTile = (pos) =>
        {
            //InfoManager.instance.SaveHoeDirtTilePos(tileManager.hoeDirtPosList);
            //InfoManager.instance.SaveWateringDirtTilePos(tileManager.wateringDirtPosList);
        };
        #endregion

        #region CropManagerAction
        this.cropManager.onGetFarmTile = (id, pos, crop) =>
        {
            bool check = tileManager.CheckTile(pos, TileManager.eTileType.WateringDirt);
            if (check == true)
            {
                crop.GrowUp();
            }
        };

        this.cropManager.onUseSeed = () =>
        {
            this.uiBase.UpdateInventory();
        };

        #endregion

        this.ranchManager.onDecideTargetTile = (startPos, targetPos, pathList, animal) =>
        {
            this.mapManager.PathFinding(startPos, targetPos, pathList);
            animal.Move();
        };

        this.ranchManager.onFillHayComplete = (amount) => 
        {
            uiFarm.ShowSiloFillHayUI(amount);        
        };

        this.objectPlaceManager.onFindWallPosList = () =>
        {
            this.objectPlaceManager.wallPosArr = mapManager.GetWallPosArr();
        };



        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            InfoManager.instance.SaveOtherObject(App.eMapType.Farm, this.objectManager.GetOtherObjectist());
            this.enterCoopInfo = InfoManager.instance.GetInfo().ranchInfo.coopInfoList.Find(x => x.posX == pos.x -1.5f && x.posY == pos.y);
            Dispatch("onArrival" + sceneType.ToString() + "Portal"+ index);
            //foreach (var item in this.cropManager.cropList)
            //{
            //    Debug.LogFormat("{0} {1} {2}", item.wateringCount, item.state, item.name);
            //}
            InfoManager.instance.SaveHoeDirtTilePos(tileManager.hoeDirtPosList);
            InfoManager.instance.SaveWateringDirtTilePos(tileManager.wateringDirtPosList);
            this.cropManager.GrowUpCrop();
            InfoManager.instance.SaveCrop(this.cropManager.cropList);

        };

        this.cropManager.Init();

        
    }
}



