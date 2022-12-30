using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
public class UIInventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
{
    public UnityAction onBeginDrag;
    public UnityAction<UIInventoryItem> onEndDrag;
    public UnityAction<int> onClickItem;
    public UnityAction<int, Vector2> onPointerPressHold;
    public UnityAction onPointerPressHoldEnd;

    private Image itemSprite;
    private Text itemAmount;
    private Button itemButton;
    public GameObject dim;

    private Coroutine onPointerPressHoldRoutine;
    private PointerEventData pointerEventData;

    private Vector3 itemSpritePos;
    private Vector3 itemAmountPos;


    public int itemID = -1;
    public int index;

    public void Init(int index)
    {
        this.itemSprite = this.transform.Find("itemSprite").GetComponent<Image>();
        this.itemAmount = this.transform.Find("itemAmount").GetComponent<Text>();
        this.dim = this.transform.Find("dim").gameObject;
        this.itemButton = this.GetComponent<Button>();        
        this.index = index;
        itemButton.onClick.AddListener(() => {
            Debug.Log(itemID);
            onClickItem(itemID);
        });
        SoundManager.instance.Init();
    }
    
    
    public void SetUIInventoryItem(Sprite sp, InventoryData data)
    {
        itemSprite.gameObject.SetActive(true);
        this.itemSprite.sprite = sp;                
        this.itemAmount.text = data.amount.ToString();
        this.itemID = data.itemId;
    }
    public void SetUIInventoryItem()
    {
        this.itemSprite.sprite = null;
        this.itemAmount.text = "";
        this.itemID = -1;
        itemSprite.gameObject.SetActive(false);
    }


    public void UIInventoryItemSetActive(bool isActive)
    {
        if (isActive)
        {
            itemSprite.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(true);
            dim.SetActive(false);           
        }
        else
        {
            itemSprite.gameObject.SetActive(false);
            itemAmount.gameObject.SetActive(false);
            dim.SetActive(true);            
        }        
    }


    private void OnPointerPressHold()
    {
        Debug.Log("<color=cyan>UI보여주기</color>");
        //Debug.Log(pointerEventData.position);
        onPointerPressHold(this.index, pointerEventData.position);
    }

    private IEnumerator OnPointerPressHoldRoutine()
    {
        float delta = 0;
        float holdTime = 1f;
        while (true)
        {
            if (pointerEventData.dragging)
                break;

            delta += Time.unscaledDeltaTime;

            //Debug.Log("<color=red>코루틴도는중</color>");            

            if (delta > holdTime)
            {
                OnPointerPressHold();
                break;
            }

            yield return null;
        }

        while (true)
        {
            //Debug.Log("<color=blue>코루틴도는중</color>");
            if (pointerEventData.dragging)
            {
                onPointerPressHoldEnd();
                break;
            }
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemID == -1 || dim.activeSelf)
            return;

        this.pointerEventData = eventData;
        onPointerPressHoldRoutine = StartCoroutine(OnPointerPressHoldRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onPointerPressHoldRoutine != null)        
            StopCoroutine(onPointerPressHoldRoutine);

        onPointerPressHoldEnd();
    }   

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemID == -1 || dim.activeSelf)
            return;

        onBeginDrag();       

        this.itemSpritePos = this.itemSprite.transform.position;
        this.itemAmountPos = this.itemAmount.transform.position;
        itemSprite.transform.DOScale(1.5f, 0.3f).SetUpdate(true);
        itemAmount.transform.DOScale(1.5f, 0.3f).SetUpdate(true);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemID == -1 || dim.activeSelf)
            return;

        onEndDrag(this);

        DOTween.Kill(itemSprite.transform);
        DOTween.Kill(itemAmount.transform);
        
        this.itemSprite.transform.position = this.itemSpritePos;
        this.itemAmount.transform.position = this.itemAmountPos;        

        itemSprite.transform.localScale = new Vector3(1, 1, 1);
        itemAmount.transform.localScale = new Vector3(1, 1, 1);

        itemSprite.color = Color.white;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemID == -1 || dim.activeSelf)
            return;

        this.pointerEventData = eventData;       

        itemSprite.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        itemAmount.GetComponent<RectTransform>().anchoredPosition += eventData.delta;        
        Color alpha = new Color(1,1,1,0.7f);
        itemSprite.color = alpha;        
    }

   
}
