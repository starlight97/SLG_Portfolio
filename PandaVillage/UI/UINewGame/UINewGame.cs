using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UINewGame : MonoBehaviour
{
    private UICreateUser uICreateUser;
    private Button UINewGameExit;

    public Action<GameInfo> onClickButton;
    public UnityAction onExitButtonClick;
    public void Init()
    {
        uICreateUser = this.transform.Find("UICreateUser").GetComponent<UICreateUser>();
        UINewGameExit = this.transform.Find("UINewGameExit").GetComponent<Button>();
        uICreateUser.Init();
        uICreateUser.onClickButton = (gameInfo) => {
            onClickButton(gameInfo);
        };

        UINewGameExit.onClick.AddListener(() => {
            onExitButtonClick();
        });
    }
}
