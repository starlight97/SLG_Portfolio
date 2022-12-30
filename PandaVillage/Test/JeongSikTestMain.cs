using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class JeongSikTestMain : MonoBehaviour
{
    private Player player;
    private MapManager mapManager;
    private TileManager tileManager;
    private TimeManager timeManager;
    private ObjectPlaceManager objectPlaceManager;
    private ObjectManager objectManager;

    private void Start()
    {
        InfoManager.instance.Init(10);
        DataManager.instance.Init();
        DataManager.instance.LoadAllData(this);

        this.player = GameObject.FindObjectOfType<Player>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();
        this.tileManager = GameObject.FindObjectOfType<TileManager>();
        this.timeManager = GameObject.FindObjectOfType<TimeManager>();
        this.objectPlaceManager = GameObject.FindObjectOfType<ObjectPlaceManager>();
        this.objectManager = GameObject.FindObjectOfType<ObjectManager>();

        this.tileManager.Init();
        this.timeManager.Init();
        DataManager.instance.onDataLoadFinished.AddListener(() =>
        {
            //// check 불러온 인포가 있다면 true
            //// 없다면 false반환 하므로 뉴비처리
            //bool check = InfoManager.instance.LoadData();
            //this.objectManager.Init("JeongTest", tileManager.GetTilesPosList(TileManager.eTileType.Grass));

            //if (check == false)
            //{
            //    this.objectManager.SpawnGatheringObjects(0, 20);
            //}
            //SaveGame();
        });


        //this.objectManager.Init("JeongTest", tileManager.GetTilesPosList(Farming.eFarmTileType.Grass));


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
            tileManager.SetTile(pos, state);
        };

        // 씨앗 뿌리기
        //this.player.onPlantCrop = (pos) =>
        //{
        //    //cropManager.CreateCrop(pos);
        //};

        this.player.onShowAnimalUI = (animal) =>
        {
            //if (animal != null)
            //    uiFarm.ShowAnimalUI(animal.animalName, animal.friendship, animal.age);
            //else
            //    uiFarm.HideAnimalUI();
        };

        #endregion

        this.timeManager.onUpdateTime = (hour, minute) =>
        {
        };

        this.objectPlaceManager.onFindWallPosList = () =>
        {
            this.objectPlaceManager.wallPosArr = mapManager.GetWallPosArr();
        };
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveGame();
    }

    private void SaveGame()
    {
        List<OtherObject> otherObjList = this.objectManager.GetOtherObjectist();

        var info = InfoManager.instance.GetInfo();
        // 10분당 1로 저장
        // ex 하루 = 1320 분
        // 하루마다 132 씩 ++
        //info.playerInfo.playMinute += 132;
        foreach (var obj in otherObjList)
        {
            ObjectInfo objectInfo = new ObjectInfo();
            objectInfo.objectId = obj.id;
            objectInfo.posX = (int)obj.gameObject.transform.position.x;
            objectInfo.posY = (int)obj.gameObject.transform.position.y;
            objectInfo.objectType = obj.objectType;
            objectInfo.sceneName = "JeongTest";

            info.objectInfoList.Add(objectInfo);
        }

        var objectInfoList = info.objectInfoList;
        for (int index = objectInfoList.Count-1; index >= 0; index--)
        {
            if (objectInfoList[index].objectType == 2)
            {
                objectInfoList.RemoveAt(index);
                Debug.Log("DELETE");
            }
        }

        InfoManager.instance.SaveInfo();
    }
}
