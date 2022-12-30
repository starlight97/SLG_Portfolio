using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectManager : MonoBehaviour
{
    public ObjectSpawner objectSpawner;
    private List<Vector3> spawnTilePosList = new List<Vector3>();

    public void Init(App.eMapType mapType, List<Vector3> spawnTilePosList)
    {
        this.objectSpawner = GameObject.FindObjectOfType<ObjectSpawner>();
        this.spawnTilePosList = spawnTilePosList;
        this.objectSpawner.Init(this.spawnTilePosList);
        this.SpawnSceneObject(mapType);
    }

    private void SpawnSceneObject(App.eMapType mapType)
    {
        var info = InfoManager.instance.GetInfo();
        var datas = info.objectInfoList.FindAll(x => x.sceneName == mapType.ToString());

        foreach (var data in datas)
        {
            Vector3 pos = new Vector3(data.posX, data.posY, 0);
            var objType = DataManager.instance.GetData(data.objectId).GetType().ToString();

            if (objType == "RuckData")
            {
                var objData = DataManager.instance.GetData<RuckData>(data.objectId);
                this.objectSpawner.SpawnObject(objData.prefab_name, objData.sprite_name, pos);
            }
            else if (objType == "GatheringData")
            {
                var objData = DataManager.instance.GetData<GatheringData>(data.objectId);
                this.objectSpawner.SpawnObject(objData.prefab_name, objData.sprite_name, pos);
            }
            else if (objType == "BuildingData")
            {
                var objData = DataManager.instance.GetData<BuildingData>(data.objectId);
                this.objectSpawner.SpawnObject(objData.prefab_path, objData.sprite_name, pos);
            }
        }
    }

    public List<OtherObject> GetOtherObjectist()
    {
        return objectSpawner.OtherObjectList;
    }

    public void SpawnGatheringObjects(int spawnPlace, int amount)
    {
        var datas = DataManager.instance.GetDataList<GatheringData>().ToList();
        List<GatheringData> seasonGatheringDataList = new List<GatheringData>();
        foreach (var data in datas)
        {
            if (data.spawn_place == spawnPlace)
                seasonGatheringDataList.Add(data);
        }

        for (int count = 0; count < amount; count++)
        {
            var rand = Random.Range(0, seasonGatheringDataList.Count - 1);

            this.objectSpawner.SpawnObject(seasonGatheringDataList[rand].prefab_name, seasonGatheringDataList[rand].sprite_name);
        }
    }

    public void SpawnRuckObjects(int amount)
    {        
        var datas = DataManager.instance.GetDataList<RuckData>().ToList();
        datas.RemoveAll(x => x.ruck_name == "잔디");
        List<int> idList = new List<int>();

        foreach (var data in datas)
        {
            idList.Add(data.id);
        }

        for (int count = 0; count < amount; count++)
        {
            var randObjIdIndex = Random.Range(0, idList.Count - 1);
            var objData = DataManager.instance.GetData<RuckData>(idList[randObjIdIndex]);
            this.objectSpawner.SpawnObject(objData.prefab_name, objData.sprite_name);
        }
    }

    public void SpawnGrass(int amount, int y, int x, bool[,] wallPosArr)
    {
        this.objectSpawner.SpawnGrassInit(y, x, wallPosArr, amount);
    }





}
