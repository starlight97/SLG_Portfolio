public class UIFarm : UIBase
{
    private UIAnimalState uIAnimalState;
    private UIShippingBin uIShippingBin;
    private UISiloState uiSiloState;

    public override void Init()
    {
        base.Init();
        this.uIAnimalState = transform.Find("UIAnimalState").GetComponent<UIAnimalState>();
        this.uIShippingBin = transform.Find("UIShippingBin").GetComponent<UIShippingBin>();
        this.uiSiloState = transform.Find("UISiloState").GetComponent<UISiloState>();
        
        uIShippingBin.onExitClick =() =>
        {
            HideUI(uIShippingBin.gameObject);
        };
        uIAnimalState.onHideState = () => 
        {
            HideAnimalUI();
        };
        uiSiloState.onHideState = () =>
        {
            HideSiloUI();
        };
        uIShippingBin.Init();
        uIAnimalState.Init();
        uiSiloState.Init();
    }

    public void ShowAnimalUI(string name, int friendship, int age)
    {
        uIAnimalState.gameObject.SetActive(true);
        uIAnimalState.ShowAnimalUI(name, friendship, age);
    }

    public void ShowShippingUI()
    {
        this.uIShippingBin.uIInventory.RePainting(36);
        this.uiInGameMenu.gameObject.SetActive(false);
        this.uIShippingBin.gameObject.SetActive(true);
    }
    public void HideAnimalUI()
    {
        uIAnimalState.gameObject.SetActive(false);
    }   

    public void ShowSiloHayAmountUI(int currAmount, int maxAmount)
    {
        this.uiSiloState.gameObject.SetActive(true);
        this.uiSiloState.SetHayText(currAmount, maxAmount);
    }

    public void ShowSiloFillHayUI(int amount)
    {
        this.uiSiloState.gameObject.SetActive(true);
        this.uiSiloState.SetFillHayText(amount);
    }

    public void HideSiloUI()
    {
        this.uiSiloState.gameObject.SetActive(false);
    }
}
