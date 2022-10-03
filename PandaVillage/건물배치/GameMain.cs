using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private Player player;
    private MapManager mapManager;
    private ObjectPlaceManager objectPlaceManager;

    private void Start()
    {
        this.player = GameObject.FindObjectOfType<Player>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();
        this.objectPlaceManager = GameObject.FindObjectOfType<ObjectPlaceManager>();

        this.player.onSelectedBuilding = (selectedBuildingGo) =>
        {
            objectPlaceManager.BuildingEdit(selectedBuildingGo);
        };

        this.objectPlaceManager.onEditComplete = () =>
        {
            player.isBuildingSelected = false;
        };
        this.objectPlaceManager.onFindWallPosList = () =>
        {
            this.objectPlaceManager.wallPosArr = mapManager.GetWallPosArr();
        };
    }
}
