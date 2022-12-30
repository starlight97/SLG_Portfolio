using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainRangeMain : GameSceneMain
{
    public override void Init(SceneParams param)
    {
        base.Init(param);


        this.objectManager.Init(App.eMapType.MountainRange, tileManager.GetTilesPosList(TileManager.eTileType.Grass));

        var info = InfoManager.instance.GetInfo();
        if (info.dicVisited[App.eMapType.MountainRange] == false)
        {
            info.dicVisited[App.eMapType.MountainRange] = true;
            this.objectManager.SpawnGatheringObjects(0, Random.Range(0, 4));
        }

        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            InfoManager.instance.SaveOtherObject(App.eMapType.MountainRange, this.objectManager.GetOtherObjectist());
            Dispatch("onArrival" + sceneType.ToString() + "Portal" + index);
        };
    }
}
