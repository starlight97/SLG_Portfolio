using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopSceneMain : GameSceneMain
{
    private Coop coop;
    private UICoop uICoop;
    private CoopParam mainParam;
    public override void Init(SceneParams param = null)
    {
        base.Init(param); 
        mainParam = (CoopParam)param;

        this.uICoop = this.uiBase.GetComponent<UICoop>();
        this.coop = GameObject.FindObjectOfType<Coop>();
        
        PlayerSpawn();

        coop.onDecideTargetTile = (startPos, targetPos, pathList, animal) =>
        {            
            this.mapManager.PathFinding(startPos, targetPos, pathList);
            animal.Move();            
        };
        coop.setAnimalPos = (animal) => {
            animal.transform.position = InfoManager.instance.GetInfo().ranchInfo.CoopSceneRandomPosition(mainParam.coopInfo.coopId);
        };
        coop.onProduceItem = () => {
           uICoop.GetProductItem();
        };

        this.player.onShowAnimalUI = (animal) =>
        {
            if (animal != null)
                uICoop.ShowAnimalUI(animal.animalName, animal.friendship, animal.age);           
        };

        coop.InitCoopScene(mainParam.coopInfo);
    }

    public void PlayerSpawn()
    {        
        var buildingData= DataManager.instance.GetData<BuildingData>(mainParam.coopInfo.coopId);
        player.transform.position = new Vector3(buildingData.player_spawn_pos_x, buildingData.player_spawn_pos_y, 0); 
    } 

}
