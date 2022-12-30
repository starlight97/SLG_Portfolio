using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVillage : MonoBehaviour
{
    private GameObject animalUI;

    public void Init()
    {
        this.animalUI = transform.Find("animalUI").gameObject;
    }

    public void ShowAnimalUI(string name, int friendship, int age)
    {
        animalUI.SetActive(true);
        var text = animalUI.transform.GetChild(1).GetComponentInChildren<Text>();
        text.text = "이름 : " + name + "\n" +
                    "우정도 : " + friendship + "\n" +
                    "함께한날 : " + age;
    }
    public void HideAnimalUI()
    {
        animalUI.SetActive(false);
    }
}
