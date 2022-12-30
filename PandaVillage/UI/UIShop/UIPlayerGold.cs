using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
public class UIPlayerGold : MonoBehaviour
{
    public Text[] GoldText;
    public UnityAction<int> onChangeGold;

    public void Init()
    {        

        onChangeGold = (val) => {            
            SetGoldText(val);
        };
    }

    public void SetGoldText(int gold)
    {
        var goldArr = gold.ToString().ToCharArray();
        Array.Reverse(goldArr);

        int i;
        for (i = 0; i < goldArr.Length; i++)        
            GoldText[i].text = goldArr[i].ToString();        

        for (int j = i; j < GoldText.Length; j++)        
            GoldText[j].text = "";        
    }


}
