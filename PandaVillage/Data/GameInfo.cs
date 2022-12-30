using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class GameInfo
{
    public int playerId;
    public bool isNewbie;
    public List<ObjectInfo> objectInfoList;
    public List<HoeDirtTileInfo> hoeDirtTileList;
    public List<WateringDirtTileInfo> wateringDirtTileList;
    public List<CropInfo> cropInfoList;
    public Dictionary<App.eMapType, bool> dicVisited;
    public PlayerInfo playerInfo;
    public RanchInfo ranchInfo;


    public GameInfo(int id, string userName, string farmName, string favoritething, bool isNewbie, string pet)
    {
        this.playerId = id;
        this.isNewbie = isNewbie;
        this.dicVisited = new Dictionary<App.eMapType, bool>();
        this.objectInfoList = new List<ObjectInfo>();
        this.hoeDirtTileList = new List<HoeDirtTileInfo>();
        this.wateringDirtTileList = new List<WateringDirtTileInfo>();
        this.cropInfoList = new List<CropInfo>();
        this.ranchInfo = new RanchInfo();
        this.playerInfo = new PlayerInfo(userName, farmName, favoritething, pet);

        dicVisited.Add(App.eMapType.Alley, false);
        dicVisited.Add(App.eMapType.MountainRange, false);
        dicVisited.Add(App.eMapType.Farm, false);
        dicVisited.Add(App.eMapType.BusStop, false);
        dicVisited.Add(App.eMapType.PandaVillage, false);
        dicVisited.Add(App.eMapType.CindersapForest, false);
        dicVisited.Add(App.eMapType.SecretForest, false);

        playerInfo.inventory.AddItem(6000, 1);
        playerInfo.inventory.AddItem(6001, 1);
        playerInfo.inventory.AddItem(6002, 1);
        playerInfo.inventory.AddItem(6003, 1);
        playerInfo.inventory.AddItem(6004, 1);
        playerInfo.inventory.AddItem(1000, 15);

        objectInfoList.Add(new ObjectInfo("Farm", 71, 50, 9010, 0));
    }
}
