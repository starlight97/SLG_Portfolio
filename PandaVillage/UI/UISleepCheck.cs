using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISleepCheck : MonoBehaviour
{
    public UnityAction<bool> onSleepCheck;
    private Button btnYes;
    private Button btnNo;
    public void Init()
    {
        this.btnYes = transform.Find("BtnYes").GetComponent<Button>();
        this.btnNo = transform.Find("BtnNo").GetComponent<Button>();

        this.btnYes.onClick.AddListener(() =>
        {
            this.onSleepCheck(true);
        });
        this.btnNo.onClick.AddListener(() =>
        {
            this.onSleepCheck(false);
        });
    }
}
