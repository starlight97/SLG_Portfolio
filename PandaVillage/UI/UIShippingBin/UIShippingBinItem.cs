using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.Events;
using System.Linq;

public class UIShippingBinItem : MonoBehaviour
{
    private Button lastShippingItemButton;
    private Text ShippingBinText;
    private Text lastShippingItemText;

    public SpriteAtlas springObjectAtlas;
    public UnityAction onLastShippingItemButton;

    public AudioClip[] dropItemClips;

    public void Init()
    {
        this.lastShippingItemButton = transform.Find("LastShippingItemButton").GetComponent<Button>();
        this.ShippingBinText = transform.Find("ShippingBinText").GetComponent<Text>();
        this.lastShippingItemText = transform.Find("LastShippingItemText").GetComponent<Text>();

        if(InfoManager.instance.GetInfo().playerInfo.lastShippedItem == null)
            EmptyShippingItem();
        else        
            ShippedItem();     

        lastShippingItemButton.onClick.AddListener(() => {
            Debug.Log("버튼누름");
            onLastShippingItemButton();
            EmptyShippingItem();            
            InfoManager.instance.GetInfo().playerInfo.lastShippedItem = null;
            SoundManager.instance.PlaySound(dropItemClips[Random.Range(0, 2)]);
        });
        SoundManager.instance.Init();
    }

    public void EmptyShippingItem()
    {
        lastShippingItemButton.gameObject.SetActive(false);
        ShippingBinText.gameObject.SetActive(true);
        lastShippingItemText.gameObject.SetActive(false);
    }

    public void ShippedItem()
    {
        lastShippingItemButton.gameObject.SetActive(true);
        ShippingBinText.gameObject.SetActive(false);
        lastShippingItemText.gameObject.SetActive(true);
        SoundManager.instance.PlaySound(dropItemClips[Random.Range(0, 2)]);

        var itemId =InfoManager.instance.GetInfo().playerInfo.lastShippedItem.itemId;
        var itemAmount = InfoManager.instance.GetInfo().playerInfo.lastShippedItem.amount;
        lastShippingItemButton.transform.Find("itemAmount").GetComponent<Text>().text = itemAmount.ToString();
        lastShippingItemButton.transform.Find("itemSprite").GetComponent<Image>().sprite = GetItemSprite(itemId);
    }


    public Sprite GetItemSprite(int itemID)
    {

        int i = itemID / 1000;
        switch (i)
        {
            case 1:
                var seedData = DataManager.instance.GetData<SeedData>(itemID);
                return springObjectAtlas.GetSprite(seedData.sprite_name);
            case 3:
                var gatheringData = DataManager.instance.GetData<GatheringData>(itemID);
                return springObjectAtlas.GetSprite(gatheringData.sprite_name);

            case 4:
                var materialData = DataManager.instance.GetData<MaterialData>(itemID);
                return springObjectAtlas.GetSprite(materialData.sprite_name);

            case 6:
                var toolData = DataManager.instance.GetData<ToolData>(itemID);
                return springObjectAtlas.GetSprite(toolData.sprite_name);

            default: Debug.Log("이상해요"); return null;
        }
    }
}
