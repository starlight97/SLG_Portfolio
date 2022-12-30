using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UILoadScrollView : MonoBehaviour
{
    private GameObject content;
    public GameObject uILoadPlayerInfoPrefab;

    public UnityAction<int> onSelectedPlayerId;
    public void Init()
    {
        this.content = this.transform.Find("content").gameObject;

    }

    public void SetUILoadPlayerInfo(GameInfo gameInfo)
    {
        var playerInfoGo = Instantiate<GameObject>(uILoadPlayerInfoPrefab, this.content.transform);
        var uILoadPlayerInfo = playerInfoGo.GetComponent<UILoadPlayerInfo>();
        uILoadPlayerInfo.Init();

        uILoadPlayerInfo.onSelectedPlayerId = (selectedId) =>
        {
            onSelectedPlayerId(selectedId);
        };

        uILoadPlayerInfo.SetUILoadPlayerInfo(gameInfo);

    }
}
