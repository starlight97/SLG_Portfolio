using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UICreateUser : MonoBehaviour
{
    private GameObject PlayerNameInputField;
    private GameObject FarmNameInputField;
    private GameObject FavoriteThingNameInputField;
    private Button UICreatePlayerButton;

    public Action<GameInfo> onClickButton;
    
    public void Init()
    {
        PlayerNameInputField = this.transform.Find("PlayerNameInputField").gameObject;
        FarmNameInputField = this.transform.Find("FarmNameInputField").gameObject;
        FavoriteThingNameInputField = this.transform.Find("FavoriteThingNameInputField").gameObject;
        UICreatePlayerButton = this.transform.Find("UICreatePlayerButton").GetComponent<Button>();

        UICreatePlayerButton.onClick.AddListener(() => {
           var gameinfo = new GameInfo(InfoManager.instance.GetInfoCount(),
               GetUserInputText(PlayerNameInputField), GetUserInputText(FarmNameInputField),
               GetUserInputText(FavoriteThingNameInputField), true, "고양이");
            onClickButton(gameinfo);
        });

    }

    public string GetUserInputText(GameObject go)
    {
        return go.transform.Find("InputText").GetComponent<Text>().text;
    }
}
