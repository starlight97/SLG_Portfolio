using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public int index;
    public bool isClickPortal;
    public UnityAction<App.eSceneType, int, Vector3> onArrival;
    public App.eSceneType sceneType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            this.onArrival(sceneType, index, this.transform.position);
    }

    public void ClickPotal()
    {
        if(isClickPortal == true)
            this.onArrival(sceneType, index, this.transform.position);
    }

}
