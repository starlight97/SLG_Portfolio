using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CindersapForestMain : GameSceneMain
{
    public override void Init(SceneParams param)
    {
        base.Init(param);

        this.objectManager.Init(App.eMapType.CindersapForest, tileManager.GetTilesPosList(TileManager.eTileType.Grass));

        var info = InfoManager.instance.GetInfo();
        if (info.dicVisited[App.eMapType.CindersapForest] == false)
        {
            info.dicVisited[App.eMapType.CindersapForest] = true;
            this.objectManager.SpawnGatheringObjects(0, Random.Range(0, 4));
            this.objectManager.SpawnRuckObjects(200);
        }

        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            InfoManager.instance.SaveOtherObject(App.eMapType.CindersapForest, this.objectManager.GetOtherObjectist());
            Dispatch("onArrival" + sceneType.ToString() + "Portal" + index);
        };
    }
}
