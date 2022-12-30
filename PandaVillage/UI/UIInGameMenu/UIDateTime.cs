using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIDateTime : MonoBehaviour
{
    private Text uIDateText;
    private Text uITimeText;
    private UIPlayerGold uIPlayerGold;

    private int day;
    private string[] dayArr = new string[] {"일","월", "화", "수", "목", "금", "토"};

    public void Init()
    {
        this.uIDateText = this.transform.Find("UIDateText").GetComponent<Text>();
        this.uITimeText = this.transform.Find("UITimeText").GetComponent<Text>();
        this.uIPlayerGold = this.transform.Find("UIPlayerGold").GetComponent<UIPlayerGold>();

        var info = InfoManager.instance.GetInfo();

        SetUIDateText(info);
        SetUITimeText(TimeManager.instance.hour, TimeManager.instance.minute);        
        uIPlayerGold.Init();
        uIPlayerGold.onChangeGold(InfoManager.instance.GetInfo().playerInfo.gold);
    }   
    private void SetUIDateText(GameInfo info)
    {        
        this.day = info.playerInfo.playDay;           
        uIDateText.text = string.Format("{0}. {1}", dayArr[day % 7], day);
    }
    public void SetUITimeText(int hour, int minute)
    {
        this.uITimeText.text = hour + " : " + minute +"0";
    }   
    public void SetUIPlayerGold()
    {
        uIPlayerGold.onChangeGold(InfoManager.instance.GetInfo().playerInfo.gold);
    }

}
