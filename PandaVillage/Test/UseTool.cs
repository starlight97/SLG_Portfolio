using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UseTool : MonoBehaviour
{
    public Button btn;
    public UnityAction onEquipTool;

    public void Init()
    {
        this.Equipment();
    }

    public void Equipment()
    {
        this.btn.onClick.AddListener(() => {
            this.onEquipTool();
        });
    }
}
