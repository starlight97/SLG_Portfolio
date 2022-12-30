using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UICarpentersSelect : MonoBehaviour
{
    private GameObject content;
    private Button carpentersShopButton;
    private Button houseUpgradeButton;
    private Button bulidButton;
    private Button exitButton;

    public UnityAction onCarpentersShopClick;
    public UnityAction onHouseUpgradeClick;
    public UnityAction onBuildClick;


    public void Init()
    {
        content = this.transform.Find("content").gameObject;
        carpentersShopButton = content.transform.Find("CarpentersShopButton").GetComponent<Button>();
        houseUpgradeButton = content.transform.Find("HouseUpgradeButton").GetComponent<Button>();
        bulidButton = content.transform.Find("BulidButton").GetComponent<Button>();
        exitButton = content.transform.Find("ExitButton").GetComponent<Button>();

        carpentersShopButton.onClick.AddListener(()=> {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            onCarpentersShopClick();
            this.gameObject.SetActive(false);
        });
        houseUpgradeButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            onHouseUpgradeClick();
            this.gameObject.SetActive(false);
        });
        bulidButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Menu);
            onBuildClick();
            this.gameObject.SetActive(false);
        });
        exitButton.onClick.AddListener(() => {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Exit);
            Debug.Log("exitButton");
            this.gameObject.SetActive(false);
        });
    }
}
