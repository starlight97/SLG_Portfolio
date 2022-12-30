using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIInGameMenu : MonoBehaviour
{
    private UIInventory uiInventoryInGame;
    public UIDateTime uiDateTime;

    private Button uiMenuButton;

    public UnityAction<int> onClickItem;
    public UnityAction onClickUIMenuButton;

    public AudioClip clickItem;

    public void Init()
    {
        this.uiInventoryInGame = transform.Find("UIInventoryInGame").GetComponent<UIInventory>();
        this.uiDateTime = transform.Find("UIDateTime").GetComponent<UIDateTime>();
        this.uiMenuButton = transform.Find("UIMenuButton").GetComponent<Button>();

        this.uiInventoryInGame.Init(UIInventory.eInventoryType.InGame, 12);
        this.uiDateTime.Init();

        this.uiInventoryInGame.onClickItem = (id) => {
            SoundManager.instance.PlaySound(clickItem);
            this.onClickItem(id);
        };
        this.uiMenuButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            onClickUIMenuButton();
        });

        SoundManager.instance.Init();
    }
    public void RePainting(int index)
    {
        uiInventoryInGame.RePainting(index);
        uiDateTime.SetUIPlayerGold();

    }

    public void SetUITimeText(int hour, int minute)
    {
        uiDateTime.SetUITimeText(hour, minute);
    }


}
