using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaVillageMain : GameSceneMain
{
    public override void Init(SceneParams param)
    {
        base.Init(param);


        this.objectManager.Init(App.eMapType.PandaVillage, tileManager.GetTilesPosList(TileManager.eTileType.Grass));

        var info = InfoManager.instance.GetInfo();
        if (info.dicVisited[App.eMapType.PandaVillage] == false)
        {
            info.dicVisited[App.eMapType.PandaVillage] = true;
            this.objectManager.SpawnGatheringObjects(0, Random.Range(0, 4));
        }
        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            InfoManager.instance.SaveOtherObject(App.eMapType.PandaVillage, this.objectManager.GetOtherObjectist());
            Dispatch("onArrival" + sceneType.ToString() + "Portal" + index);
        };
    }
}
