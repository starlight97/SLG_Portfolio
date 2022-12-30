using UnityEngine;

public class GameSceneMain : SceneMain
{
    protected UIBase uiBase;
    protected Player player;
    protected MapManager mapManager;
    protected TileManager tileManager;
    protected ObjectManager objectManager;
    protected PortalManager portalManager;

    public override void Init(SceneParams param = null)
    {
        base.Init(param);
        this.uiBase = GameObject.FindObjectOfType<UIBase>();
        this.player = GameObject.FindObjectOfType<Player>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();
        this.tileManager = GameObject.FindObjectOfType<TileManager>();
        this.objectManager = GameObject.FindObjectOfType<ObjectManager>();
        this.portalManager = GameObject.FindObjectOfType<PortalManager>();

        this.uiBase.Init();
        this.player.transform.position = param.SpawnPos;
        var info = InfoManager.instance.GetInfo();
        int year = info.playerInfo.playYear;
        int day = info.playerInfo.playDay;
        TimeManager.instance.Init();
        this.tileManager.Init();
        this.portalManager.Init();
        SoundManager.instance.Init();
        #region PlayerAction
        this.player.onDecideTargetTile = (startPos, targetPos, pathList) =>
        {
            if (targetPos.x < 0)
                targetPos.x = 0;
            if (targetPos.y < 0)
                targetPos.y = 0;

            this.mapManager.PathFinding(startPos, targetPos, pathList);
            this.player.Move();
        };

        this.player.onDecideTargetObject = (startPos, targetPos, pathList) =>
        {
            if (targetPos.x < 0)
                targetPos.x = 0;
            if (targetPos.y < 0)
                targetPos.y = 0;

            this.mapManager.WallPathFinding(startPos, targetPos, pathList);
            this.player.MoveAction();
        };

        // 타일이 있냐?
        this.player.onGetTile = (pos, state) =>
        {
            bool check = tileManager.CheckTile(pos, state);

            if (check)
            {
                player.FarmingAct(pos);
            }
        };

        this.player.onCheckTile = (pos, state) => 
        {
            bool check = tileManager.CheckTile(pos, state);
            player.CheckFarmingAct(check);
        };

        // 타일 변경
        this.player.onChangeFarmTile = (pos, useTool) =>
        {
            tileManager.SetTile(pos, useTool);
        };

        this.player.onGetItem = () =>
        {
            this.uiBase.UpdateInventory();
        };

        #endregion

        #region TimeManagerAction
        TimeManager.instance.onUpdateTime = (hour, minute) =>
        {
            uiBase.TimeUpdate(hour, minute);
            if (hour == 26)
            {
                tileManager.ClearWateringTiles();
                TimeManager.instance.EndDay();
                InfoManager.instance.EndDay();
                Dispatch("EndDay");
            }
        };
        #endregion

        this.portalManager.onArrival = (sceneType, index, pos) =>
        {
            Dispatch("onArrival" + sceneType.ToString() + "Portal"+ index);
        };
        this.uiBase.onClickItem = (itemId) =>
        {
            this.player.ItemSelected(itemId);
        };
        this.uiBase.onClickGotoTitleButton = () =>
        {
            Dispatch("onClickGotoTitle");
            TimeManager.instance.Resume();
        };

    }
}
