using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIShop : UIBase
{
    private UIHeroShop uiHeroShop;
    private UIAboutPanel uiAboutPanel;
    private UIRotateDots uiRotateDots;
    public UnityAction onClickLobby;
    public UnityAction onClickAdsBtn;
    public Button btnBack;
    public Button btnShowAd;
    public Button btnCloudSave;
    public Button btnCloudLoad;
    public Button btnLoadCheck;
    public Button btnSaveCheck;
    public Button btnReview;
    public Button btnAbout;
    public Button btnGetGoldCheck;
    public GameObject panelCloudGo;
    public UnityAction onDataLoadComplete;

    public GameObject panelSaveCheckGo;
    public GameObject panelLoadCheckGo;
    public GameObject panelGetGoldGo;

    override public void Init()
    {
        base.Init();
        this.UIOptionInit();

        this.uiHeroShop = GameObject.FindObjectOfType<UIHeroShop>();
        this.uiAboutPanel = GameObject.FindObjectOfType<UIAboutPanel>();
        this.uiRotateDots = GameObject.FindObjectOfType<UIRotateDots>();

        this.uiHeroShop.Init(0);

        this.btnBack.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonAudio.Button1);
            this.onClickLobby();
        });
        this.btnShowAd.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonAudio.Button1);
            onClickAdsBtn();
        });
        this.isShowPanelOption = (check) =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonAudio.Button1);
        };
        this.btnCloudSave.onClick.AddListener(() =>
        {
            GPGSManager.instance.SaveToCloud(InfoManager.instance.GetInfo());
            uiRotateDots.Show();
        });
        this.btnCloudLoad.onClick.AddListener(() =>
        {
            GPGSManager.instance.LoadFromCloud();
            uiRotateDots.Show();
        });

        GPGSManager.instance.onSavedCloud = () =>
        {
            uiRotateDots.Hide();
            panelSaveCheckGo.gameObject.SetActive(true);
            //this.textGameInfo.text = status.ToString();
        };
        GPGSManager.instance.onLoadedCloud = (info) =>
        {
            uiRotateDots.Hide();
            panelLoadCheckGo.gameObject.SetActive(true);
            InfoManager.instance.SetInfo(info);
            //var json = JsonConvert.SerializeObject(this.gameInfo);
        };

        this.btnSaveCheck.onClick.AddListener(() =>
        {
            panelSaveCheckGo.gameObject.SetActive(false);
        });
        this.btnLoadCheck.onClick.AddListener(() =>
        {
            onDataLoadComplete();
        });
        this.btnReview.onClick.AddListener(() =>
        {
            Application.OpenURL("market://details?id=com.subingo.animalrescue");
        });
        this.btnAbout.onClick.AddListener(() =>
        {
            this.uiAboutPanel.ShowPanel();
        });
        this.btnGetGoldCheck.onClick.AddListener(() =>
        {
            this.panelGetGoldGo.gameObject.SetActive(false);
        });

        this.uiAboutPanel.Init();
        this.uiRotateDots.Init();
    }

    public Text GetTextGold()
    {
        return uiHeroShop.textGold;
    }

    public void ShowRotateDots()
    {
        this.uiRotateDots.Show();
    }

    public void HideRotateDots()
    {
        this.uiRotateDots.Hide();
    }

    public void ShowGetGoldPanel()
    {
        this.panelGetGoldGo.gameObject.SetActive(true);
    }
}
