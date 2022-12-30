using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIItemDescription : MonoBehaviour
{
    private Text ItemNameText;
    private Text ItemDescText;

    public void Init()
    {
        this.ItemNameText = this.transform.Find("ItemNameText").GetComponent<Text>();
        this.ItemDescText = this.transform.Find("ItemDescText").GetComponent<Text>();
    }

    public void SetDesc(string itemName, string itemDecs)
    {
        this.ItemNameText.text = itemName;
        this.ItemDescText.text = itemDecs;
    }
}
