using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIGameSetting : MonoBehaviour
{
    private Button gotoTitleButton;

    public UnityAction onClickGotoTitleButton;
    public void Init()
    {
        gotoTitleButton = this.transform.Find("gotoTitleButton").GetComponent<Button>();

        gotoTitleButton.onClick.AddListener(() => {
            onClickGotoTitleButton();
        });
    }
}
