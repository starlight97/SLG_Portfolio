using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarnAnimal : Animal
{//하루가 지나면 플레이어와 상호작용해서 product를 생산할수 있어야함
    public UnityAction onProduceItem;

    public override bool Produce()
    {
        var animalInfo = InfoManager.instance.GetInfo().ranchInfo.GetAnimalInfo(this.animalName);
        var animalData = DataManager.instance.GetData<AnimalData>(animalInfo.animalId);

        if(this.isFull && animalInfo.yummyDay >6 && ToolCheck(animalData.tool_id))
        {
            int productId = DataManager.instance.GetData<AnimalData>(id).product_id;
            bool addItem = InfoManager.instance.GetInfo().playerInfo.inventory.AddItem(productId, 1);

            //아이템 칸이 있을때만 넣어준다
            if (addItem)
            {
                StartCoroutine(SetAnimalEmote());
                this.isFull = false;
                animalInfo.isFull = false;
                animalInfo.yummyDay++;
                onProduceItem();

                return true;
            }            
        }        
        return false;
        
    }
    public bool ToolCheck(int toolId)
    {
        var check = InfoManager.instance.GetInfo().playerInfo.inventory.SearchItem(toolId);
        Debug.Log("ToolCheck`````````" + check);
        return check;
    }
}
