using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
public class UISortButton : MonoBehaviour
{
    private Button sortButton;
    public UnityAction onSortInventory;

    public void Init()
    {
        this.sortButton = this.GetComponent<Button>();

        sortButton.onClick.AddListener(() => {
            Debug.Log("imClicked!");
            SortInventory();            
        });
    }
    public Dictionary<int, string> DicDIc;

    public void SortInventory()
    {       
        var info = InfoManager.instance.GetInfo();
        var dic = info.playerInfo.inventory.dicItem;

        int i = 0;
        var sortDictionary = dic.OrderByDescending(x => x.Value != null ? x.Value.itemId : 0).ToDictionary(x => i++, x => x.Value);
               
        foreach (var sortDic in sortDictionary)
        {
            Debug.LogFormat("<color=red>key : {0}</color>", sortDic.Key);

            if (sortDic.Value != null)
            {
            Debug.LogFormat("<color=blue>val : {0}</color>", sortDic.Value.itemId);
            }
        }    

        info.playerInfo.inventory.dicItem = sortDictionary;

        onSortInventory();
    }

    
}
