using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FarmMain : SceneMain
{
    private Player player;
    private MapManager mapManager;

    public override void Init(SceneParams param = null)
    {
        this.player = GameObject.FindObjectOfType<Player>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();

        this.player.onDecideTargetTile = (startPos, targetPos, pathList) =>
        {
            this.mapManager.PathFinding(startPos, targetPos, pathList);
            this.player.Move();
        };
    }
}



