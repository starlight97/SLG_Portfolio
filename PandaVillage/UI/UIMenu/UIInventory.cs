using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInventory : MonoBehaviour
{
    public UnityAction<int> onClickItem;
    public UnityAction<InventoryData> onShippingItem;
 
    public AudioClip[] arrInventorySound;
    public enum eInventorySound
    {
        Swap,
        TrashCan,
        ThrowTrash
        // 클릭할 때 soundmanager tab
    }
    public enum eInventoryType
    {
        Ui,
        InGame
    }
    private GameObject[] uIInventoryItems;
    private GameObject uIInventoryItemArea;
    private UIItemDescription uIItemDescription;
    private UIPlayerEquipmentInformation uIPlayerEquipmentInformation;

    //eInventoryType.Ui에만 있음
    private UITrashCan uITrashCan;
    private UISortButton uISortButton;

    private GameInfo info;

    public SpriteAtlas springObjectAtlas;

    private void ItemSwap(int index1, int index2)
    {
        var info = InfoManager.instance.GetInfo();
        info.playerInfo.inventory.SwapItem(index1, index2);
        //InfoManager.instance.SaveInfo();
        this.info = InfoManager.instance.GetInfo();
        var dic = info.playerInfo.inventory.dicItem;
        SetUIInventoryItem(index1, dic[index1]);
        SetUIInventoryItem(index2, dic[index2]);

        PlaySound(eInventorySound.Swap);
    }   

    public Canvas m_canvas;
    private GraphicRaycaster m_gr;
    PointerEventData m_ped;
    private Coroutine swapRoutine;
    private GameObject dragItemGo;
    private int resultsCount;
    private IEnumerator SwapRoutine()
    {
        while (true)
        {
            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (results.Count > 0)
            {
                foreach (var re in results)
                {
                    if (re.gameObject.transform.GetComponent<UIInventoryItem>() || re.gameObject.transform.GetComponent<UITrashCan>() || 
                        re.gameObject.transform.GetComponent<UIShippingBinItem>())
                    {
                        this.dragItemGo = re.gameObject;
                    }                    
                }
            }
            this.resultsCount = results.Count;
            yield return null;
        }
    }
    public void Init(eInventoryType type, int size)
    {
        m_gr = this.transform.root.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        this.uIInventoryItemArea = this.transform.Find("UIInventoryItemArea").gameObject;
        this.uIItemDescription = this.transform.Find("UIItemDescription").GetComponent<UIItemDescription>();

        uIItemDescription.Init();

        this.info = InfoManager.instance.GetInfo();
        var dicInventoryItem = info.playerInfo.inventory.dicItem;

        if (type == eInventoryType.Ui)
        {
            this.uITrashCan = this.transform.Find("UITrashCan").GetComponent<UITrashCan>();
            this.uISortButton = this.transform.Find("UISortButton").GetComponent<UISortButton>();
            this.uIPlayerEquipmentInformation = this.transform.Find("UIPlayerEquipmentInformation").GetComponent<UIPlayerEquipmentInformation>();

            uISortButton.Init();
            uIPlayerEquipmentInformation.Init();

            uISortButton.onSortInventory = () => {

                RePainting(36);
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
            };           
        }


        this.uIInventoryItems = new GameObject[size];
        for (int index = 0; index < size; index++)
        {  
            this.uIInventoryItems[index] = this.uIInventoryItemArea.transform.GetChild(index).gameObject;
            uIInventoryItems[index].GetComponent<UIInventoryItem>().Init(index);

           
            //-------------------------

            if (type == eInventoryType.InGame)
            {
                
                var uIInventoryItem = uIInventoryItems[index].GetComponent<UIInventoryItem>();
                uIInventoryItem.UIInventoryItemSetActive(true);
                
            }

            if (type == eInventoryType.Ui)
            {                         
                var uIInventoryItem = uIInventoryItems[index].GetComponent<UIInventoryItem>();
                uIInventoryItem.UIInventoryItemSetActive(false);
                

                //플레이어의 가방크기만큼 setactive해줌
               if (info.playerInfo.inventory.size > index)
               {
                    //Debug.Log(info.playerInfo.inventory.size);
                    uIInventoryItems[index].SetActive(true);
                    uIInventoryItems[index].GetComponent<UIInventoryItem>().UIInventoryItemSetActive(true);
               }

            }

            //-------------------------

            //if (dicInventoryItem[index] != null)
                SetUIInventoryItem(index, dicInventoryItem[index]);
        }
        // InventorySize(type, info.playerInfo.inventory.size);

        foreach (var uiInventoryItem in uIInventoryItems)
        {
            var item = uiInventoryItem.GetComponent<UIInventoryItem>();

            item.onBeginDrag = () => {
                swapRoutine =StartCoroutine(SwapRoutine());
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
            };

            item.onEndDrag = (inventoryItem) => {
                StopCoroutine(swapRoutine);

                if (this.dragItemGo.GetComponent<UIInventoryItem>())
                {
                    var dragItem = dragItemGo.GetComponent<UIInventoryItem>();

                    if (resultsCount > 1 && !dragItem.dim.activeSelf)
                        ItemSwap(inventoryItem.index, dragItem.index);
                }
                
                if (this.dragItemGo.GetComponent<UITrashCan>() && inventoryItem.itemID/ 1000 != 6)
                {
                    this.info = InfoManager.instance.GetInfo();
                    this.info.playerInfo.inventory.dicItem[inventoryItem.index] = null;
                    //InfoManager.instance.SaveInfo();

                    SetUIInventoryItem(inventoryItem.index);
                    this.PlaySound(eInventorySound.ThrowTrash);
                }
                if (this.dragItemGo.GetComponent<UIShippingBinItem>() && resultsCount > 1 && inventoryItem.itemID / 1000 != 6)
                {
                    var gameInfo = InfoManager.instance.GetInfo();
                    var itemAmount = gameInfo.playerInfo.inventory.GetItemCount(inventoryItem.itemID);
                    gameInfo.playerInfo.lastShippedItem = new InventoryData(inventoryItem.itemID, itemAmount);
                    this.info.playerInfo.inventory.dicItem[inventoryItem.index] = null;
                    SetUIInventoryItem(inventoryItem.index);
                    onShippingItem(gameInfo.playerInfo.lastShippedItem);
                }
            };

            //UI Inventory InGame에서 사용
            if(type == eInventoryType.InGame)
            {
                item.onClickItem = (id) => {
                    onClickItem(id);                
                };
            }
            else
            {
                item.onClickItem = (id) => {

                    Debug.Log("imUI"); 
                };
            }



            //꾹누르면 아이템 설명해주는 UI가 활성화됨
            item.onPointerPressHold = (index , pos) => {
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
                var dic = info.playerInfo.inventory.dicItem;
                var itemID = dic[index].itemId;

                string itemName = GetItemName(itemID);
                string itemDesc = GetItemDescription(itemID);
                uIItemDescription.SetDesc(itemName, itemDesc);

                Debug.Log(uIItemDescription.transform.position.y);
                uIItemDescription.transform.position = pos + new Vector2(100,100);

                uIItemDescription.gameObject.SetActive(true);
            };
            //마우스를 떼면 아이템 설명해주는 UI가 비활성화됨
            item.onPointerPressHoldEnd = () => {
                uIItemDescription.gameObject.SetActive(false);
            };

        }

        SoundManager.instance.Init();

    }   
    
    
    /*
    public void InventorySize(eInventoryType type, int size)
    {
        if (type == eInventoryType.InGame)
        {
            for (int i = 0; i < 12; i++)
            {
                var uIInventoryItem = uIInventoryItems[i].GetComponent<UIInventoryItem>();
                uIInventoryItem.UIInventoryItemSetActive(true);
            }
        }
    
        if (type == eInventoryType.Ui)
        {
            for (int i = 0; i < 36; i++)
            {
                var uIInventoryItem = uIInventoryItems[i].GetComponent<UIInventoryItem>();
                uIInventoryItem.UIInventoryItemSetActive(false);
            }

            //플레이어의 가방크기만큼 setactive해줌
            for (int i = 0; i < size; i++)
            {
                uIInventoryItems[i].SetActive(true);
                uIInventoryItems[i].GetComponent<UIInventoryItem>().UIInventoryItemSetActive(true);
            }
        }
    }
    */

    public void RePainting(int index)
    {
        if (index == 36)
        {
            for (int i = 0; i < info.playerInfo.inventory.size; i++)
            {
                uIInventoryItems[i].SetActive(true);
                uIInventoryItems[i].GetComponent<UIInventoryItem>().UIInventoryItemSetActive(true);
            }
        }

        foreach (var dic in info.playerInfo.inventory.dicItem)
        {
            if(dic.Key < index)
            SetUIInventoryItem(dic.Key, dic.Value);
        }
    }


    public void SetUIInventoryItem(int key, InventoryData val = null)
    {
        var uIInventoryItem = uIInventoryItemArea.transform.GetChild(key).GetComponent<UIInventoryItem>();

        if (val != null)
        {
            Sprite sp = GetItemSprite(val.itemId);
            uIInventoryItem.SetUIInventoryItem(sp, val);   
        }
        else
        {
            uIInventoryItem.SetUIInventoryItem();
        }

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


    public string GetItemDescription(int itemID)
    {

        int i = itemID / 1000;
        string description;
        switch (i)
        {
            case 1:
                description = DataManager.instance.GetData<SeedData>(itemID).description;
                break; 
            case 3:
                description = DataManager.instance.GetData<GatheringData>(itemID).description;
                break;

            case 4:
                 description = DataManager.instance.GetData<MaterialData>(itemID).description;
                break;

            case 6:
                description = DataManager.instance.GetData<ToolData>(itemID).description;
                break;

            default: Debug.Log("이상해요"); return null;
        }
        return description;
    }


    public string GetItemName(int itemID)
    {

        int i = itemID / 1000;
        string itemName;
        switch (i)
        {
            case 1:
                itemName = DataManager.instance.GetData<SeedData>(itemID).item_name;
                break;
            case 3:
                itemName = DataManager.instance.GetData<GatheringData>(itemID).gathering_name;
                break;

            case 4:
                itemName = DataManager.instance.GetData<MaterialData>(itemID).material_name;
                break;

            case 6:
                itemName = DataManager.instance.GetData<ToolData>(itemID).tool_name;
                break;

            default: Debug.Log("이상해요"); return null;
        }
        return itemName;
    }

    public void PlaySound(eInventorySound soundType)
    {
        SoundManager.instance.PlaySound(arrInventorySound[(int)soundType]);
    }
}
