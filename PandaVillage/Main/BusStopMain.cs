using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStopMain : GameSceneMain
{

    public override void Init(SceneParams param)
    {
        base.Init(param);

        this.objectManager.Init(App.eMapType.BusStop, this.tileManager.GetTilesPosList(TileManager.eTileType.Grass));


        var info = InfoManager.instance.GetInfo();
        if (info.dicVisited[App.eMapType.BusStop] == false)
        {
            info.dicVisited[App.eMapType.BusStop] = true;
            this.objectManager.SpawnGatheringObjects(0, Random.Range(0, 4));
        }

        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            InfoManager.instance.SaveOtherObject(App.eMapType.BusStop, this.objectManager.GetOtherObjectist());
            Dispatch("onArrival" + sceneType.ToString() + "Portal" + index);
        };
    }
}
