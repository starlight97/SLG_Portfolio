using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIMenu : MonoBehaviour
{
    private UIMenuButton uIMenuButton;
    private UIInventory uIInventory;
    private UIGameSetting uIGameSetting;

    private Button MenuCloseButton;

    public UnityAction onClickCloseMenuButton;
    public UnityAction onClickGotoTitleButton;


    public GameObject[] menuGo;

    

    public void Init()
    {    
        this.uIMenuButton = this.transform.Find("UIMenuButton").GetComponent<UIMenuButton>();
        this.uIInventory = this.transform.Find("UIInventory").GetComponent<UIInventory>();
        this.uIGameSetting = this.transform.Find("UIGameSetting").GetComponent<UIGameSetting>();
        this.MenuCloseButton = this.transform.Find("MenuCloseButton").GetComponent<Button>();

        uIMenuButton.Init();
        uIGameSetting.Init();
        uIInventory.Init(UIInventory.eInventoryType.Ui, 36);

        this.MenuCloseButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            onClickCloseMenuButton();
        });

        uIMenuButton.onMenuButtonClicked = (text) => {
            foreach (var go in menuGo)
            {
                if (go.name == text)
                    go.SetActive(true);
                else
                    go.SetActive(false);
            }
        };        
        uIMenuButton.FirstItemSelect();

        uIGameSetting.onClickGotoTitleButton = () => 
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            TimeManager.instance.StopTimeRoutine();
            this.onClickGotoTitleButton();
        };

        SoundManager.instance.Init();
    }     

    public void RePainting(int index)
    {
        uIInventory.RePainting(index);
    }
}
