using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIMarniesRanchSelect : MonoBehaviour
{
    private GameObject content;
    private Button marniesRanchShopButton;
    private Button animalShopButton;
    private Button exitButton;

    public UnityAction onUIShopClick;
    public UnityAction onUIAnimalShopClick;

    public void Init()
    {
        content = this.transform.Find("content").gameObject;
        marniesRanchShopButton = content.transform.Find("MarniesRanchShopButton").GetComponent<Button>();
        animalShopButton = content.transform.Find("AnimalShopButton").GetComponent<Button>();
        exitButton = content.transform.Find("ExitButton").GetComponent<Button>();

        marniesRanchShopButton.onClick.AddListener(() => {
            Debug.Log("marniesRanchShopButton");
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            this.gameObject.SetActive(false);
            onUIShopClick();
        });
        animalShopButton.onClick.AddListener(() => {
            Debug.Log("animalShopButton");
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            this.gameObject.SetActive(false);
            onUIAnimalShopClick();
        });
        exitButton.onClick.AddListener(() => {
            Debug.Log("exitButton");
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            this.gameObject.SetActive(false);
        });
    }
}
