using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIAnimalState : MonoBehaviour
{
    private GameObject AnimalState;
    private Button thisBg;

    public UnityAction onHideState;
    public void Init()
    {
        this.AnimalState = transform.Find("AnimalState").gameObject;
        this.thisBg = transform.Find("bg").GetComponent<Button>();

        thisBg.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Tab);
            onHideState();
        });
    }
   public void ShowAnimalUI(string name, int friendship, int age)
    {

        var text = AnimalState.transform.GetChild(0).GetComponentInChildren<Text>();
        text.text = "이름 : " + name + "\n" +
                    "호감도 : " + friendship + "\n" +
                    "함께한날 : " + age;
    }
}
