using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanchInfo
{
    public List<CoopInfo> coopInfoList;

    // 사일로
    public int hay;
    public int houseId;

    public RanchInfo()
    {
        this.coopInfoList = new List<CoopInfo>();    
        this.hay = 0;
        this.houseId = 8999;
    }

    public bool CheckAnimalName(string animalName)
    {
        foreach (var coopInfo in coopInfoList)
        {
            var animalInfo = coopInfo.animalinfoList.Find(x => x.animalName == animalName);
            if (animalInfo != null)
                return false;
        }

        return true;
    }

    public AnimalInfo GetAnimalInfo(string animalName)
    {
        foreach (var coopInfo in coopInfoList)
        {
            var animalInfo = coopInfo.animalinfoList.Find(x => x.animalName == animalName);
            if (animalInfo != null)
                return animalInfo;
        }
        return null;
    }

    public void NextDay()
    {        
        foreach (var coop in this.coopInfoList)
        {           
            foreach (var animal in coop.animalinfoList)
            {
                //나이먹기
                animal.age++;
                //쓰다듬초기화
                animal.isPatted = false;
                animal.isAnimalOut = false;



                //배부르면 생산품생산함
                if (animal.isFull)
                {
                    animal.isFull = false;
                    animal.yummyDay++;
                }


                // 사일로의 건초가 있으면 먹이주기
                if (InfoManager.instance.GetInfo().ranchInfo.hay > 0)
                {
                    InfoManager.instance.GetInfo().ranchInfo.hay--;
                    animal.isFull = true;

                    //닭장동물들 생산품 coopinfo의 productlist에 저장
                    AddProductCoopAnimal(animal, coop);
                }
                else
                {
                    animal.isFull = false;
                }

                
            }
        }
    }
    public void AddProductCoopAnimal(AnimalInfo animal, CoopInfo coop)
    {
        if (animal.yummyDay < 6)
            return;
        //닭장동물들 생산품
        var animalData = DataManager.instance.GetData<AnimalData>(animal.animalId);
        if (animalData.home_type == 0)
        {
            var spawnPos = CoopSceneRandomPosition(coop.coopId);
            coop.productInfoList.Add(new ProductInfo(animalData.product_id, spawnPos.x, spawnPos.y));            
        }
    }


    #region CoopSceneRandomPosition
    public Vector3 CoopSceneRandomPosition(int coopId)
    {
        switch (coopId)
        {
            //닭장
            case 9004:
                return new Vector3(Random.Range(31, 40), Random.Range(31, 35), 0);
            //빅닭장
            case 9005:
                return new Vector3(Random.Range(61, 74), Random.Range(31, 35), 0);
            //디럭스닭장
            case 9006:
                return new Vector3(Random.Range(91, 111), Random.Range(31, 35), 0);
            //외양간
            case 9007:
                return new Vector3(Random.Range(36, 46), Random.Range(61, 70), 0);
            //빅외양간
            case 9008:
                return new Vector3(Random.Range(81, 95), Random.Range(61, 70), 0);
            //디럭스외양간
            case 9009:
                return new Vector3(Random.Range(126, 143), Random.Range(60, 74), 0);
        }
        return new Vector3(0, 0, 0);
    }
    #endregion

}
