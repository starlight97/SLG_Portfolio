using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UILogoImage : MonoBehaviour
{
    private Button btn;
    private Animator anim;
    public UnityAction onLogoClick;
    public UnityAction onAnimEnd;
    public Image DeveloperImage;
    private void Start()
    {
        btn = GetComponent<Button>();
        anim = GetComponent<Animator>();

        btn.onClick.AddListener(() => {
            anim.SetTrigger("click");
            onLogoClick();
        });

    }
    public void DestoryThisGO()
    {
        this.DeveloperImage.DOFade(0, 1);
        this.gameObject.GetComponent<Image>().DOFade(0, 0.8f).OnComplete(() => {
            onAnimEnd();
        });
    }
}
