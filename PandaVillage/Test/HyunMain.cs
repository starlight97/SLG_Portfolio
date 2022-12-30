using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class HyunMain : MonoBehaviour
{
    private Player player;
    private MapManager mapManager;
    private TileManager tileManager;
    private TimeManager timeManager;
    private CropManager cropManager;

    void Start()
    {
        this.player = GameObject.FindObjectOfType<Player>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();
        this.tileManager = GameObject.FindObjectOfType<TileManager>();
        this.timeManager = GameObject.FindObjectOfType<TimeManager>();
        this.cropManager = GameObject.FindObjectOfType<CropManager>();

        //this.timeManager.Init();
        this.cropManager.Init();

        this.player.onDecideTargetTile = (startPos, targetPos, pathList) =>
        {
            this.mapManager.PathFinding(startPos, targetPos, pathList);
            this.player.Move();
        };

        // 해당 위치에 플레이어가 원하는 타일이 있다면 농사 행위 수행
        this.player.onGetTile = (pos, state) =>
        {
            bool check = tileManager.CheckTile(pos, state);
            if (check)
                player.FarmingAct(pos); 
        };

        // 타일 변경
        this.player.onChangeFarmTile = (pos, state) =>
        {
            tileManager.SetTile(pos, state);
        };

        //// 씨앗 뿌리기
        //this.player.onPlantCrop = (pos) => 
        //{
        //    cropManager.CreateCrop(pos);   
        //};

        this.timeManager.onUpdateTime = (hour, minute) =>
        {
            //cropManager.CheckWateringDirt();
            tileManager.ClearWateringTiles();
        };

        //this.cropManager.onGetFarmTile = (pos, crop) =>
        //{
        //    bool check = tileManager.CheckTile(pos, TileManager.eTileType.WateringDirt);
        //    if (check == true)
        //    {
        //        cropManager.GrowUpCrop(crop);
        //    }
        //};
    }
}
