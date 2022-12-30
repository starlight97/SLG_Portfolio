using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIAnimalPurchase : MonoBehaviour
{
    public Button btnOkay;
    public Button btnCancel;
    public InputField inputFieldAnimalName;

    public UnityAction<string> onClickOkay;
    public UnityAction onClickCancel;
    public void Init()
    {
        this.btnOkay.onClick.AddListener(() =>
        {
            this.onClickOkay(inputFieldAnimalName.text);
        });
        this.btnCancel.onClick.AddListener(() =>
        {
            this.onClickCancel();
        });
    }

}
