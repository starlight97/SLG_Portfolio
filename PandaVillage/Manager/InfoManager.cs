using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

public class InfoManager
{
    public int PlayerId
    {
        private set;
        get;
    }
    public static readonly InfoManager instance = new InfoManager();

    private Dictionary<int, GameInfo> dicInfos = new Dictionary<int, GameInfo>(); 
    public void Init(int playerId)
    {
        LoadData();
        this.PlayerId = playerId;
    }

    // path에 데이터 파일이 있을경우 기존유저 처리해야함 true 반환
    // 없을경우 신규유저 처리해야함 false 반환
    public void LoadData()
    {
        dicInfos.Clear();
        var path = string.Format("{0}/game_info.json", Application.persistentDataPath);

        if (File.Exists(path))
        {
            Debug.Log("기존 유저");
            var json = File.ReadAllText(path);
            var datas = JsonConvert.DeserializeObject<GameInfo[]>(json);
            datas.ToDictionary(x => x.playerId).ToList().ForEach(x => dicInfos.Add(x.Key, x.Value));
        }
        else
        {
            //Debug.Log("신규 유저 입니다.");
            //GameInfo gameInfo = new GameInfo(PlayerId, "길동이", "dd", "dd", true, "강아지");
            //dicInfos.Add(gameInfo.playerId, gameInfo);
        }
        SaveGame();
    }


    // id값이랑 class type 일치하는 info데이터 반환
    public GameInfo GetInfo()
    {
        return this.dicInfos[PlayerId];
    }

    public List<GameInfo> GetInfoList()
    {
        return this.dicInfos.Values.ToList();
    }

    public int GetInfoCount()
    {
        return this.dicInfos.Count;
    }


    public void UpdateInfo(GameInfo info)
    {
        this.dicInfos[info.playerId] = info;
    }
    public void InsertInfo(GameInfo info)
    {
        this.dicInfos.Add(info.playerId, info);
        SaveGame();
    }
    public void DeleteInfo(GameInfo info)
    {
        var infoId = info.playerId;       
        for (int index = infoId; index < dicInfos.Count-1; index++)
        {
            dicInfos[index] = dicInfos[index + 1];
            dicInfos[index].playerId = index;
        }
        dicInfos.Remove(dicInfos.Count-1);
        SaveGame();
    }

    public void SaveInfo()
    {
        //IEnumerable<RawData> col = this.dicInfos.Values.Where(x => x.GetType().Equals(typeof(T)));
        //var json = JsonConvert.SerializeObject(col.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList());

        var json = JsonConvert.SerializeObject(this.dicInfos.Values);
        var path = string.Format("{0}/game_info.json", Application.persistentDataPath);
        File.WriteAllText(path, json);
    }

    public void SaveGame()
    {
        //IEnumerable<RawData> col = this.dicInfos.Values.Where(x => x.GetType().Equals(typeof(T)));
        //var json = JsonConvert.SerializeObject(col.Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList());

        var json = JsonConvert.SerializeObject(this.dicInfos.Values);
        var path = string.Format("{0}/game_info.json", Application.persistentDataPath);
        File.WriteAllText(path, json);
    }

    public void SaveOtherObject(App.eMapType mapType, List<OtherObject> objList)
    {
        List<OtherObject> otherObjList = objList;

        var info = this.GetInfo();


        // 여기부터봐야함
        // 건물 인포 다 날리고있어서 문제임
        info.objectInfoList.RemoveAll(x => x.sceneName == mapType.ToString());

        foreach (var obj in otherObjList)
        {
            ObjectInfo objectInfo = new ObjectInfo();
            objectInfo.objectId = obj.id;
            objectInfo.posX = (int)obj.gameObject.transform.position.x;
            objectInfo.posY = (int)obj.gameObject.transform.position.y;
            objectInfo.objectType = obj.objectType;
            objectInfo.sceneName = mapType.ToString();

            info.objectInfoList.Add(objectInfo);
        }
    }

    public void SaveHoeDirtTilePos(List<Vector3Int> posList)
    {
        var info = this.GetInfo();
        info.hoeDirtTileList.Clear();
        foreach(var pos in posList)
        {
            HoeDirtTileInfo tileInfo = new HoeDirtTileInfo();
            tileInfo.posX = pos.x;
            tileInfo.posY = pos.y;

            info.hoeDirtTileList.Add(tileInfo);
        }
    }

    public void SaveWateringDirtTilePos(List<Vector3Int> posList)
    {
        var info = this.GetInfo();
        info.wateringDirtTileList.Clear();
        foreach (var pos in posList)
        {
            WateringDirtTileInfo tileInfo = new WateringDirtTileInfo();
            tileInfo.posX = pos.x;
            tileInfo.posY = pos.y;

            info.wateringDirtTileList.Add(tileInfo);
        }
    }

    public void SaveCrop(List<Crop> cropList)
    {
        var info = this.GetInfo();

        info.cropInfoList.Clear();

        foreach (var crop in cropList)
        {
            CropInfo cropInfo = new CropInfo();
            cropInfo.id = crop.id;
            cropInfo.state = crop.state;
            cropInfo.wateringCount = crop.wateringCount;
            cropInfo.posX = (int)crop.gameObject.transform.position.x;
            cropInfo.posY = (int)crop.gameObject.transform.position.y;
            cropInfo.isWatering = crop.isWatering;

            info.cropInfoList.Add(cropInfo);
        }
    }

    // 하루가 끝나면 채집오브젝트는 다 삭제
    public void EndDay()
    {
        var info = this.GetInfo();

        info.objectInfoList.RemoveAll(x => x.objectType == 2);

        for (int index = 0; index < info.dicVisited.Count; index++)
        {
            info.dicVisited[info.dicVisited.Keys.ToList()[index]] = false;
        }

        info.playerInfo.playDay += 1; 
        // 계절이 하나뿐이라 28일로 고정
        //if(info.playerInfo.playDay == 113)
        if(info.playerInfo.playDay == 28)
        {
            info.playerInfo.playDay = 1;
            info.playerInfo.playYear += 1;
            info.hoeDirtTileList.Clear();
        }

        info.wateringDirtTileList.Clear();

        foreach (var crop in info.cropInfoList)
        {
            crop.isWatering = false;
        }
        info.playerInfo.gold += info.playerInfo.dailySaleGold;
        info.playerInfo.dailySaleGold = 0;
        info.playerInfo.lastShippedItem = null;

        //동물들 업데이트해주기
        info.ranchInfo.NextDay();


        this.SaveGame();
    }

}
