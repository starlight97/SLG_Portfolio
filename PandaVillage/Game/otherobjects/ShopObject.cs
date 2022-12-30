using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopObject : MonoBehaviour
{
    public UnityAction onShowShopUI;
    public UnityAction onInventoryUpgrade;

    public void ShowShop()
    {
        if (this.GetComponent<InventoryUpgrade>())        
            this.onInventoryUpgrade();        
        else
            this.onShowShopUI();
    }

}
