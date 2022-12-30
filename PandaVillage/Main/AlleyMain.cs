using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlleyMain : GameSceneMain
{
    public override void Init(SceneParams param)
    {
        base.Init(param);
        this.objectManager.Init(App.eMapType.Alley, tileManager.GetTilesPosList(TileManager.eTileType.Grass));

        var info = InfoManager.instance.GetInfo();
        if(info.dicVisited[App.eMapType.Alley] == false)
        {
            info.dicVisited[App.eMapType.Alley] = true;
            this.objectManager.SpawnGatheringObjects(0, Random.Range(0,4));
        }        
        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            InfoManager.instance.SaveOtherObject(App.eMapType.Alley, this.objectManager.GetOtherObjectist());
            Dispatch("onArrival" + sceneType.ToString() + "Portal" + index);
        };
    }
}
