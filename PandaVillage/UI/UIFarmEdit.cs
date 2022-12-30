using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFarmEdit : MonoBehaviour
{
    public UnityAction onCLickBtnOkay;
    public UnityAction onCLickBtnCancel;
    public UnityAction<string> onPurchaseAnimal;
    private GameObject buttons;
    private Button btnOk;
    private Button btnCancel;
    public UIAnimalPurchase uiAnimalPurchase;
    public void Init()
    {
        this.buttons = transform.Find("Buttons").gameObject;
        this.btnOk = buttons.transform.Find("BtnOk").GetComponent<Button>();
        this.btnCancel = buttons.transform.Find("BtnCancel").GetComponent<Button>();
        this.btnOk.onClick.AddListener(() =>
        {
            this.onCLickBtnOkay();
        });
        this.btnCancel.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.onCLickBtnCancel();
        });

        this.uiAnimalPurchase.onClickOkay = (animalName) =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            this.onPurchaseAnimal(animalName);
        };
        this.uiAnimalPurchase.onClickCancel = () =>
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            uiAnimalPurchase.gameObject.SetActive(false);
        };


        uiAnimalPurchase.Init();
        this.HideBtnOk();
    }

    public void ShowBtnOk()
    {
        this.btnOk.gameObject.SetActive(true);
    }
    public void HideBtnOk()
    {
        this.btnOk.gameObject.SetActive(false);
    }

    public void ShowUIAnimalPurchase()
    {
        uiAnimalPurchase.gameObject.SetActive(true);
    }
}
