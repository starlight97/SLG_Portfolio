using UnityEngine;
using UnityEngine.Events;

public class UIBase : MonoBehaviour
{
    public UIInGameMenu uiInGameMenu;
    protected UIMenu uiMenu;
    public UnityAction<int> onClickItem;
    public UnityAction onClickGotoTitleButton;
    virtual public void Init()
    {
        this.uiInGameMenu = transform.Find("UIInGameMenu").GetComponent<UIInGameMenu>();
        this.uiMenu = transform.Find("UIMenu").GetComponent<UIMenu>();

        this.uiInGameMenu.Init();
        this.uiMenu.Init();

        this.uiInGameMenu.onClickItem = (id) => 
        {
            this.onClickItem(id);        
        };
        this.uiInGameMenu.onClickUIMenuButton = () => 
        {
            this.uiInGameMenu.gameObject.SetActive(false);
            this.uiMenu.gameObject.SetActive(true);
            this.uiMenu.RePainting(36);
            TimeManager.instance.Pause();
        };
        this.uiMenu.onClickCloseMenuButton = () => 
        {
            this.uiInGameMenu.gameObject.SetActive(true);
            this.uiMenu.gameObject.SetActive(false);
            this.uiInGameMenu.RePainting(12);
            TimeManager.instance.Resume();
        };

        this.uiMenu.onClickGotoTitleButton = () => 
        {
            onClickGotoTitleButton();
        };
    }

    public void UpdateInventory()
    {
        this.uiInGameMenu.RePainting(12);
    }

    public void TimeUpdate(int hour, int minute)
    {
        this.uiInGameMenu.uiDateTime.SetUITimeText(hour, minute);
    }

    public void HideUI(GameObject go)
    {
        go.SetActive(false);
        this.uiInGameMenu.gameObject.SetActive(true);
        this.uiInGameMenu.RePainting(12);
        this.uiMenu.RePainting(36);
    }
}
