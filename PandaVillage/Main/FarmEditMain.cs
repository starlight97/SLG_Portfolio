using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FarmEditMain : GameSceneMain
{
    public enum eEditSound
    {
        Bulid,
        Demolition
    }

    public enum eEditType
    {
        Build=1,            // 건설
        Move,               // 이동
        Demolition,         // 철거
        CoopPurchase,      // 닭장 동물 구매
        BarnPurchase,      // 외양간 동물 구매
    }
    private UIFarmEdit uiFarmEdit;
    private ObjectDetector objectDetector;
    private ObjectPlaceManager objectPlaceManager;
    private eEditType editType;
    private GameObject selectedBuildingGo = null;
    public AudioClip[] arrEditClip;

    private void Start()
    {
        //var param = new FarmEditParam();
        //param.objectId = 8002;
        //param.editType = 4;

        //DataManager.instance.Init();
        //DataManager.instance.LoadAllData(this);

        //DataManager.instance.onDataLoadFinished.AddListener(() =>
        //{
        //    this.Init(param);
        //});


    }
    public override void Init(SceneParams param = null)
    {
        //base.Init(param);
        var mainParam = (FarmEditParam)param;
        this.editType = (eEditType)mainParam.editType;
        TimeManager.instance.StopAllCoroutines();
        this.objectDetector = GameObject.FindObjectOfType<ObjectDetector>();
        this.objectManager = GameObject.FindObjectOfType<ObjectManager>();
        this.objectPlaceManager = GameObject.FindObjectOfType<ObjectPlaceManager>();
        this.mapManager = GameObject.FindObjectOfType<MapManager>();
        this.uiFarmEdit = GameObject.FindObjectOfType<UIFarmEdit>();
        this.tileManager = GameObject.FindObjectOfType<TileManager>();        
        this.tileManager.Init();
        this.objectManager.Init(App.eMapType.Farm, tileManager.GetTilesPosList(TileManager.eTileType.Dirt));
        this.uiFarmEdit.Init();

        var info = InfoManager.instance.GetInfo();
        Debug.Log("editType : " + editType);
        if (editType == eEditType.Move || editType == eEditType.Demolition || editType == eEditType.CoopPurchase || editType == eEditType.BarnPurchase)
        {
            Debug.Log("디텍터");
            this.objectDetector.Init();
        }
        else
        {            
            objectPlaceManager.Init(mainParam.objectId);
            this.uiFarmEdit.ShowBtnOk();
        }
        
        

        this.objectDetector.onClickBuilding = (buildingGo) =>
        {
            if(this.editType == eEditType.Move)
            {
                objectPlaceManager.Init(buildingGo);
                this.objectDetector.StopDetecting();
            }
            else if(this.editType == eEditType.Demolition)
            {
                if (this.selectedBuildingGo != null)
                {
                    CancelSelectedBuilding();
                }
            }
            else if (this.editType == eEditType.CoopPurchase)
            {
                int objid = buildingGo.GetComponent<OtherObject>().id;
                if (objid != 9004 && objid != 9005 && objid != 9006)
                {
                    SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
                    Debug.Log("COOP 아님");
                    return;
                }
                if (this.selectedBuildingGo != null)
                {
                    CancelSelectedBuilding();
                }
            }
            else if (this.editType == eEditType.BarnPurchase)
            {
                int objid = buildingGo.GetComponent<OtherObject>().id;
                if (objid != 9007 && objid != 9008 && objid != 9009)
                {
                    SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
                    Debug.Log("COOP 아님");
                    return;
                }
                if (this.selectedBuildingGo != null)
                {
                    CancelSelectedBuilding();
                }
            }
            this.uiFarmEdit.ShowBtnOk();
            selectedBuildingGo = buildingGo;


            var spriteRenderer = this.selectedBuildingGo.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.green;

        };

        this.objectPlaceManager.onMoveComplete = (oldPos, newPos) =>
        {
            PlaySound(eEditSound.Bulid);
            this.objectDetector.Detecting();
            this.uiFarmEdit.HideBtnOk();

            CancelSelectedBuilding();

            

            var obj = info.objectInfoList.Find(x => x.sceneName == "Farm" && x.posX == oldPos.x && x.posY == oldPos.y);
            obj.posX = (int)newPos.x;
            obj.posY = (int)newPos.y;

            var coop = selectedBuildingGo.GetComponent<Coop>();

            // 사일로가 아닐때 (외양간, 닭장)
            if (coop != null)
            {
                var coopInfo = info.ranchInfo.coopInfoList.Find(x => x.posX == oldPos.x && x.posY == oldPos.y);
                coopInfo.posX = (int)newPos.x;
                coopInfo.posY = (int)newPos.y;
                info.ranchInfo.coopInfoList.Add(coopInfo);
            }

            selectedBuildingGo = null;
        };

        this.objectPlaceManager.onBuildComplete = (pos) =>
        {
            PlaySound(eEditSound.Bulid);
            var data = DataManager.instance.GetData<BuildingData>(mainParam.objectId);            
            ObjectInfo objectInfo = new ObjectInfo();
            objectInfo.objectId = mainParam.objectId;
            objectInfo.objectType = 0;
            objectInfo.sceneName = "Farm";
            objectInfo.posX = (int)pos.x;
            objectInfo.posY = (int)pos.y;
            info.objectInfoList.Add(objectInfo);

            PayPrice(mainParam.objectId);

            // 사일로가 아닐때 (외양간, 닭장)
            if(objectInfo.objectId != 9003)
            {
                var coopInfo = new CoopInfo(objectInfo.objectId, (int)pos.x, (int)pos.y);
                info.ranchInfo.coopInfoList.Add(coopInfo);
            }
            Dispatch("onEditComplete");
            
        };
        this.objectPlaceManager.onFindWallPosList = () =>
        {
            this.objectPlaceManager.wallPosArr = mapManager.GetWallPosArr();
            this.objectPlaceManager.BuildingEdit();
        };
        this.uiFarmEdit.onCLickBtnOkay = () =>
        {
            if (this.editType == eEditType.Build)
            {
                this.objectPlaceManager.BuildingBuildExecute();
            }
            else if (this.editType == eEditType.Move)
            {
                this.objectPlaceManager.BuildingMove();
            }
            else if (this.editType == eEditType.Demolition)
            {
                PlaySound(eEditSound.Demolition);
                var pos = selectedBuildingGo.transform.position;
                var objInfo = info.objectInfoList.Find(x => x.sceneName == "Farm" && x.posX == pos.x && x.posY == pos.y);
                info.objectInfoList.Remove(objInfo);

                var coop = selectedBuildingGo.GetComponent<Coop>();
                // 사일로가 아닐때 (외양간, 닭장)
                if (coop != null)
                {
                    info.ranchInfo.coopInfoList.RemoveAll(x => x.posX == pos.x && x.posY == pos.y);
                }

                Dispatch("onEditComplete");
            }
            else if (this.editType == eEditType.CoopPurchase || this.editType == eEditType.BarnPurchase)
            {
                this.uiFarmEdit.ShowUIAnimalPurchase();
            }

        };
        uiFarmEdit.onPurchaseAnimal = (animalName) =>
        {
            this.PurchaseAnimal(animalName, mainParam.objectId);
        };

        this.uiFarmEdit.onCLickBtnCancel = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            // 현재 건물 선택중이 아니라면
            if (selectedBuildingGo == null)
            {
                Dispatch("onEditComplete");
            }
            // 현재 건물 선택중 이라면
            else
            {
                if (this.editType == eEditType.Build)
                {
                    this.objectPlaceManager.BuildingBuildExecute();
                }
                else if (this.editType == eEditType.Move)
                {
                    // 현재 선택한 건물 취소
                    CancelSelectedBuilding();

                    selectedBuildingGo = null;
                    this.objectPlaceManager.BuildingEditCancel();
                    objectDetector.Detecting();


                }
                else if (this.editType == eEditType.Demolition)
                {
                    // 현재 선택한 건물 취소
                    selectedBuildingGo = null;
                }
            }

        };

    }

    private void PurchaseAnimal(string animalName, int animalId)
    {
        var info = InfoManager.instance.GetInfo();

        // 동물 중복이름 체크
        var check = info.ranchInfo.CheckAnimalName(animalName);
        if (check == false)
        {
            return;
        }
        var pos = selectedBuildingGo.transform.position;

        var coopinfo = info.ranchInfo.coopInfoList.Find(x => x.posX == pos.x && x.posY == pos.y);
        AnimalInfo animalInfo = new AnimalInfo(animalName, animalId);
        coopinfo.animalinfoList.Add(animalInfo);
        Dispatch("onEditComplete");
    }

    private void CancelSelectedBuilding()
    {
        var oldSpriteRenderer = this.selectedBuildingGo.transform.GetChild(0).GetComponent<SpriteRenderer>();
        oldSpriteRenderer.color = Color.white;
    }

    private void PlaySound(eEditSound soundType)
    {
        SoundManager.instance.PlaySound(arrEditClip[(int)soundType]);
    }   
    private void PayPrice(int objectId)
    {
        
        var buildingData = DataManager.instance.GetData<BuildingData>(objectId);

        InfoManager.instance.GetInfo().playerInfo.gold -= buildingData.require_gold;
        InfoManager.instance.GetInfo().playerInfo.inventory.RemoveItem(4000, buildingData.require_wood);
        InfoManager.instance.GetInfo().playerInfo.inventory.RemoveItem(4001, buildingData.require_stone);
    }
}
