using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIMenuButton : MonoBehaviour
{
    private Button[] buttons;

    public UnityAction<string> onMenuButtonClicked;

    public void Init()
    {
        this.buttons = this.GetComponentsInChildren<Button>();

        foreach (var btn in buttons)
        {
            btn.onClick.AddListener(() => {                
                onMenuButtonClicked(btn.name);
                SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
                foreach(var b in buttons)
                    b.GetComponent<Image>().color = Color.gray;

                btn.GetComponent<Image>().color = Color.white;
            });
        }

        SoundManager.instance.Init();

        
    }
    public void FirstItemSelect()
    {
        foreach (var b in buttons)
            b.GetComponent<Image>().color = Color.gray;

        var firstItemBtn = this.GetComponentInChildren<Button>();
        firstItemBtn.Select();        
        onMenuButtonClicked(firstItemBtn.name);
        firstItemBtn.GetComponent<Image>().color = Color.white;
    }

}
