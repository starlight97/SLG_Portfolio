using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUpgrade : MonoBehaviour
{
    private GameObject largePackSprite;
    private GameObject deluxePackSprite;
    
    public void Init()
    {
        this.largePackSprite = this.transform.Find("largePackSprite").gameObject;
        this.deluxePackSprite = this.transform.Find("deluxePackSprite").gameObject;       
        SetGameObject(InfoManager.instance.GetInfo().playerInfo.inventory.size);

    }

    public void SetGameObject(int size)
    {
        if (size == 12)
        {
            largePackSprite.SetActive(true);
            deluxePackSprite.SetActive(false);
        }
        if (size == 24)
        {
            largePackSprite.SetActive(false);
            deluxePackSprite.SetActive(true);
        }
        if (size == 36)
            this.gameObject.SetActive(false);
    }
}
