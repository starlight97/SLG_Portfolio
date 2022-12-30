using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UILoadPlayerInfo : MonoBehaviour
{
    private Text uILoadPlayerNumberingText;
    private Text uILoadPlayerNameText;
    private Text uILoadPlayerInfoText;
    private Text uILoadFarmNameText;
    private Text uILoadPlayerGoldText;
    //private Text uILoadPlayTimeText;
    private Button uILoadPlayerInfoDeleteButton;
    private Button uILoadPlayerInfoButton;
    private GameInfo myGameInfo;

    public UnityAction<int> onSelectedPlayerId;
    public void Init()
    {
        uILoadPlayerNumberingText = this.transform.Find("UILoadPlayerNumberingText").GetComponent<Text>();
        uILoadPlayerNameText = this.transform.Find("UILoadPlayerNameText").GetComponent<Text>();
        uILoadPlayerInfoText = this.transform.Find("UILoadPlayerInfoText").GetComponent<Text>();
        uILoadFarmNameText = this.transform.Find("UILoadFarmNameText").GetComponent<Text>();
        uILoadPlayerGoldText = this.transform.Find("UILoadPlayerGoldText").GetComponent<Text>();
        //uILoadPlayTimeText = this.transform.Find("UILoadPlayTimeText").GetComponent<Text>();
        uILoadPlayerInfoDeleteButton = this.transform.Find("UILoadPlayerInfoDeleteButton").GetComponent<Button>();
        uILoadPlayerInfoButton = this.gameObject.GetComponent<Button>();
        this.uILoadPlayerInfoDeleteButton.onClick.AddListener(() => {
            //게임인포에서 아이디 찾아서 삭제
            InfoManager.instance.DeleteInfo(myGameInfo);
            Destroy(this.gameObject);
        });

        this.uILoadPlayerInfoButton.onClick.AddListener(() => {
            onSelectedPlayerId(myGameInfo.playerId);
        });

    }

    public void SetUILoadPlayerInfo(GameInfo gameInfo)
    {
        var playerInfo = gameInfo.playerInfo;
        this.myGameInfo = gameInfo;
        
        this.uILoadPlayerNumberingText.text = gameInfo.playerId + ".";
        this.uILoadPlayerNameText.text = playerInfo.playerName;
        this.uILoadPlayerInfoText.text = string.Format("{0}년째, {1}일째", playerInfo.playYear, playerInfo.playDay);
        this.uILoadFarmNameText.text = playerInfo.farmName + " 농장";
        this.uILoadPlayerGoldText.text = playerInfo.gold + " 골드";
        //this.uILoadPlayTimeText.text = "플탐";
    }

}
