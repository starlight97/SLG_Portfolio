using System.Collections;
using System.Collections.Generic;


public class Inventory
{
    // 인벤토리 사이즈
    public int size;
    // key : idx , value : itemId , amount 
    public Dictionary<int, InventoryData> dicItem;

    public Inventory(int size)
    {
        this.size = size;

        dicItem = new Dictionary<int, InventoryData>();
        for (int idx = 0; idx < 36; idx++)
        {
            dicItem.Add(idx, null);
        }
    }

    public void SwapItem(int key1, int key2)
    {
        var temp = dicItem[key1];
        dicItem[key1] = dicItem[key2];
        dicItem[key2] = temp;


    }


    //아이템이 들어갔으면 true 안들어갔으면 false 반환
    public bool AddItem(int addItemID, int addAmount)
    {
        int itemKey = GetItemIndex(addItemID);

        if (itemKey == -1)
        {
            for (int i = 0; i < size; i++)
            {
               if (dicItem[i] == null)
               {
                    dicItem[i] = new InventoryData(addItemID, addAmount);
                    return true;
               }
            }          
        }
        else
        {
            dicItem[itemKey].amount += addAmount;
            return true;
        }

        return false;

    }

    public void RemoveItem(int id, int amount)
    {
        int itemKey = GetItemIndex(id);
        if (itemKey != -1)
        {
            if (dicItem[itemKey].amount == 1)
            {
                dicItem[itemKey] = null;
            }
            else
            {
                dicItem[itemKey].amount -= amount;
            }
        }

        if (GetItemCount(id) == 0)
            dicItem[itemKey] = null;
    }


    //아이템 아이디를 입력해서 아이템이 있으면 키값을 반환하고 없으면 -1을 반환
    private int GetItemIndex(int ItemID)
    {
        foreach (var item in dicItem)
        {
            if (item.Value != null && item.Value.itemId == ItemID)
                return item.Key;
        }

        return -1;
    }



    public int GetItemCount(int itemId)
    {
        int cnt = 0;
        foreach (var item in dicItem)
        {
            if (item.Value != null && item.Value.itemId == itemId)
                cnt = item.Value.amount;
        }
        return cnt;
    }

    public bool SearchItem(int itemId)
    {
        foreach (var item in dicItem)
        {
            if (item.Value != null && item.Value.itemId == itemId)
                return true;
        }
        return false;
    }
}