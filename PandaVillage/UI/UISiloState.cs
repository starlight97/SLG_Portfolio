using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISiloState : MonoBehaviour
{
    private Button bgBtn;
    private GameObject siloStateGo;
    private Text hayAmountText;
    private Text fillHayText;

    public UnityAction onHideState;

    public void Init()
    {
        this.bgBtn = this.transform.Find("bg").GetComponent<Button>();
        this.siloStateGo = this.transform.Find("SiloState").gameObject;
        this.hayAmountText = this.siloStateGo.transform.Find("HayAmountText").GetComponent<Text>();
        this.fillHayText = this.siloStateGo.transform.Find("fillHayText").GetComponent<Text>();
        this.gameObject.SetActive(false);

        this.bgBtn.onClick.AddListener(() => {
            onHideState();
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
        });
    }

    public void SetHayText(int currAmount, int maxAmount)
    {
        hayAmountText.text = string.Format("건초의 양: {0} / {1}", currAmount, maxAmount);
        hayAmountText.gameObject.SetActive(true);
        fillHayText.gameObject.SetActive(false);
    }

    public void SetFillHayText(int amount)
    {
        fillHayText.text = string.Format("저장고에 건초 조각 {0}개를 넣었습니다.", amount);
        hayAmountText.gameObject.SetActive(false);
        fillHayText.gameObject.SetActive(true);
    }
}
