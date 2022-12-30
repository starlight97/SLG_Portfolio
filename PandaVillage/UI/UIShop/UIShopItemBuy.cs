using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIShopItemBuy : MonoBehaviour
{
    private Text SelectedItemName;
    private Text SelectedItemInfomationText;
    private Text SelectedItemGold;
    private Button buyButton;

    public Slider slider;

    private int itemPrice;
    public Action<int> buyButtonClicked;

    public void Init()
    {
        this.SelectedItemName = this.transform.Find("SelectedItemName").GetComponent<Text>();
        this.SelectedItemInfomationText = this.transform.Find("SelectedItemInfomationText").GetComponent<Text>();
        this.SelectedItemGold = this.SelectedItemInfomationText.transform.Find("SelectedItemGold").GetComponent<Text>();
        this.buyButton = this.GetComponentInChildren<Button>();

        buyButton.onClick.AddListener(() => {
            buyButtonClicked((int)slider.value);
        });

        //슬라이더의 값이 변경되면
        this.slider.onValueChanged.AddListener(delegate { SliderValueChange(); });
    }

    //슬라이더의 값이 변경될때마다 버튼의 텍스트를 변경해줘야함
    public void SliderValueChange()
    {      
        SetButtonTextChange(itemPrice, (int)slider.value);
    }

    //스크롤뷰의 아이템이 선택됐을경우
    public void SetText(UIShopItem item)
    {
        SelectedItemName.text = item.item_name;
        SelectedItemInfomationText.text = item.item_description;
        SelectedItemGold.text = item.price.ToString();
        SetButtonTextChange(item.price); //버튼의 텍스트도 같이 변경해줌
        slider.value = 1;                // 슬라이더의 값도 1로 바꿔줌
    }


    public void SetSliderMaxValue(int PlayerMoney, int itemPrice)
    {
        this.itemPrice = itemPrice;

        int MaxVal = (PlayerMoney / itemPrice);

        if (MaxVal <= 1)
        {
            this.slider.gameObject.transform.parent.gameObject.SetActive(false);    //살수있는 아이템 갯수가 1개 이하일때는 슬라이더를 끈다.
        }
        else
        {
            this.slider.gameObject.transform.parent.gameObject.SetActive(true);
            this.slider.minValue = 1;
        this.slider.maxValue = MaxVal;
        }
    }  

    public void SetButtonTextChange( int itemPrice, int val = 1)
    {
        var btnText = buyButton.GetComponentInChildren<Text>();

        if (val <= 1)
        {
           btnText.text = itemPrice.ToString();
        }
        else if(val > 1)
        {
            btnText.text = String.Format("x {0} : {1}", val, itemPrice);
        }
    }


}
