using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UILoadGame : MonoBehaviour
{
    private UILoadScrollView uILoadScrollView;
    private Button UILoadGameExit;

    public UnityAction<int> onSelectedPlayerId;
    public UnityAction onExitButtonClick;
    public void Init()
    {
        this.uILoadScrollView = this.transform.Find("UILoadScrollView").GetComponent<UILoadScrollView>();
        this.UILoadGameExit = this.transform.Find("UILoadGameExit").GetComponent<Button>();
        this.uILoadScrollView.Init();

        var gameInfoList = InfoManager.instance.GetInfoList();

        foreach (var info in gameInfoList)
        {
            uILoadScrollView.SetUILoadPlayerInfo(info);
        }

        uILoadScrollView.onSelectedPlayerId = (selectedId) => {
            onSelectedPlayerId(selectedId);
        };

        UILoadGameExit.onClick.AddListener(() => {
            onExitButtonClick();
        });


    }
}
