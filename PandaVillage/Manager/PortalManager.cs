using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalManager : MonoBehaviour
{
    public UnityAction<App.eSceneType, int, Vector3> onArrival;

    public void Init()
    {
        //int portalCount = this.transform.childCount;
        //for(int index = 0; index < portalCount; index++)
        //{
        //    var portal = this.transform.GetChild(index).GetComponent<Portal>();
        //    portal.onArrival = (sceneType, index) =>
        //    {
        //        this.onArrival(sceneType, index);
        //    };
        //}
        StartCoroutine(WaitInit());
    }

    private IEnumerator WaitInit()
    {
        yield return new WaitForSeconds(0.1f);
        int portalCount = this.transform.childCount;
        for(int index = 0; index < portalCount; index++)
        {
            var portal = this.transform.GetChild(index).GetComponent<Portal>();
            portal.onArrival = (sceneType, index, pos) =>
            {
                this.onArrival(sceneType, index, pos);
            };
        }
    }

}
