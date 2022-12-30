using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIInventoryUpgrade : MonoBehaviour
{
    public Text descriptionText;
    public Text purchaseButtonText;
    public Button PurchaseButton;
    public Button ExitButton;
    public UnityAction onSuccessExpansionInventory;
    public UnityAction onFailedExpansionInventory;

    private int size;
    public void Init()
    {
        PurchaseButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            BuyInventory();
            SetText();
        });

        ExitButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.gameObject.SetActive(false);
        });

        SetText();

        SoundManager.instance.Init();
    }
    private void SetText()
    {
        size = InfoManager.instance.GetInfo().playerInfo.inventory.size;

        if (size == 12)
        {
            descriptionText.text = "인벤토리를 24칸으로 확장하시겠습니까?";
            purchaseButtonText.text = "구매하기 (2,000골드)";
        }
        else if (size == 24)
        {
            descriptionText.text = "인벤토리를 36칸으로 확장하시겠습니까?";
            purchaseButtonText.text = "구매하기 (10,000골드)";
        }
    }

    private void BuyInventory()
    {
        this.gameObject.SetActive(false);

        int purchasePrice =0;
        if (size == 12)
            purchasePrice = 2000;
        else if (size == 24)
            purchasePrice = 10000;

        if (InfoManager.instance.GetInfo().playerInfo.gold >= purchasePrice)
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
            InfoManager.instance.GetInfo().playerInfo.gold -= purchasePrice;
            InfoManager.instance.GetInfo().playerInfo.inventory.size = size + 12;
            onSuccessExpansionInventory();
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
            onFailedExpansionInventory();
        }
    }
}
