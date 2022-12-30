using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RanchManager : MonoBehaviour
{

    public Silo[] siloArr;
    public Coop[] coopArr;

    public UnityAction<Vector2Int, Vector2Int, List<Vector3>, Animal> onDecideTargetTile;
    public UnityAction<int> onFillHayComplete;
    public int hay;
    public int maxHay;

    private GameInfo gameInfo;
    public UnityAction onAnimalGoHomeTime;

    public void Init()
    {

        gameInfo = InfoManager.instance.GetInfo();          

        this.siloArr = GameObject.FindObjectsOfType<Silo>();
        this.coopArr = GameObject.FindObjectsOfType<Coop>();

        this.hay = gameInfo.ranchInfo.hay;     
        this.maxHay = siloArr.Length * 240;
        
        CoopInit();
        AnimalsInit();

    }   
    private void CoopInit()
    {
        foreach (var coop in coopArr)
        {
            var coopInfo = gameInfo.ranchInfo.coopInfoList.Find(x => x.posX == coop.transform.position.x && x.posY == coop.transform.position.y);
            coop.Init(coopInfo);
            Debug.LogFormat("ranchManager CoopInit : {0}", coopInfo);
        }
    }
    
    // 사일로가 존재하면 true 반환, 아니면 false
    public bool CheckSilo()
    {
        if (siloArr.Length > 0)
            return true;
        else
            return false;
    }

    // 플레이어가 가지고 있는 건초들 추가
    public void AddHays(int hayAmount)
    {
        var currHayAmount = maxHay - hay;
        if (hay == maxHay)
            return;
        if (currHayAmount >= hayAmount)
            this.hay += hayAmount;
        else if (currHayAmount < hayAmount)
        {
            hayAmount = currHayAmount;
            this.hay += hayAmount;
        }
        gameInfo.playerInfo.inventory.RemoveItem(4004, hayAmount);
        onFillHayComplete(hayAmount);
        gameInfo.ranchInfo.hay = this.hay;
    }

    // 낫으로 잔디 베면 건초 1개 추가
    public void AddHay()
    {
        if (hay < maxHay)
            this.hay++;
        else if (hay <= 0)
            this.hay = 0;
        else if (hay >= maxHay)
            this.hay = this.maxHay;
        gameInfo.ranchInfo.hay = this.hay;
    }

    //사일로 UI보이게하기
    public void ShowSiloUI(Vector3 mousePos)
    {
        foreach (var silo in siloArr)
        {
            if (mousePos == silo.transform.position)
            {
                Debug.LogFormat("건초의양 : {0} / {1}", hay, maxHay);
            }
        }
    }
   
    public void DoorOpen()
    {
        foreach (var coop in this.coopArr)
        {
            coop.DoorOpen();
        }
    }

    public void AnimalsGoHome()
    {
        foreach (var coop in this.coopArr)
        {
            coop.AnimalsGoHome();
        }
    }

    public void AnimalsInit()
    {
        //animal들 초기화          

        foreach (var coop in this.coopArr)
        {
            //동물들 움직이기
            coop.onDecideTargetTile = (startPos, targetPos, pathList, animal) =>
            {
                this.onDecideTargetTile(startPos, targetPos, pathList, animal);
            };



            foreach (var animal in coop.animalList)
            {
                //동물들이 집으로 가게하기
                animal.goHome = (startPos, pathList) =>
                {
                    //coop의 문의 포지션을 가져옴
                    var DoorPos = coop.transform.GetChild(1).position;
                    Vector2Int targetPos = new Vector2Int((int)DoorPos.x, (int)DoorPos.y - 1);
                    this.onDecideTargetTile(startPos, targetPos, pathList, animal);
                };
            }
        }
    }
    
}
