using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPlayerEquipmentInformation : MonoBehaviour
{
    private Text currentGoldText;

    public void Init()
    {
        this.currentGoldText = this.transform.Find("currentGoldText").GetComponent<Text>();
        currentGoldText.text = string.Format("현재 소지금 : {0}골드", InfoManager.instance.GetInfo().playerInfo.gold);
    }
}
