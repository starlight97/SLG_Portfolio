using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIBuildBuildingDescription : MonoBehaviour
{
    private Text nameText;
    private Text descriptionText;
    private Text requireGoldText;
    private Text requireWoodText;
    private Text requireStoneText;

    public void Init()
    {
        this.nameText = this.transform.Find("nameText").GetComponent<Text>();
        this.descriptionText = this.transform.Find("descriptionText").GetComponent<Text>();
        this.requireGoldText = descriptionText.transform.Find("requireGoldText").GetComponent<Text>();
        this.requireWoodText = descriptionText.transform.Find("requireWoodText").GetComponent<Text>();
        this.requireStoneText = descriptionText.transform.Find("requireStoneText").GetComponent<Text>();
    }

    public void setDesc(string buildingName,string desc,int gold, int wood, int stone)
    {
        nameText.text = string.Format("{0}", buildingName);
        descriptionText.text = string.Format("{0}", desc);
        requireGoldText.text = string.Format("{0} 골드", gold);
        requireWoodText.text = string.Format("{0} 나무", wood);
        requireStoneText.text = string.Format("{0} 돌", stone);
    }
}

