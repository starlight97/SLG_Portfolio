using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeState : MonoBehaviour
{
    public Text text;
    public Button bg;
    public void Init()
    {
        bg.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
    }
    public void SetText(bool isSuccess)
    {
        if (isSuccess)
        {
            int size = InfoManager.instance.GetInfo().playerInfo.inventory.size;

            if(size ==24)
            text.text = "큰 가방을 입수했다! 인벤토리의 공간이 24로 증가했다.";
            else
            text.text = "디럭스 가방을 입수했다! 인벤토리의 공간이 36으로 증가했다.";
        }
        else
        {
            text.text = "돈이 부족합니다...";
        }
    }
}
