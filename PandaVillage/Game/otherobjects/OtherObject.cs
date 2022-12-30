using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OtherObject : MonoBehaviour
{
    public int id;
    public int objectType;
    private Sprite sp;
    private SpriteRenderer spriteRenderer;

    public UnityAction<OtherObject> onDestroy;
    public void DestroyObject()
    {
        //Debug.LogFormat("{0} 아이템 획득", objectName);
        this.onDestroy(this);
        Destroy(this.gameObject);

    }

    public virtual void Init(Sprite sp)
    {
        if(objectType == 0)
        {
            this.spriteRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        else
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }


        this.spriteRenderer.sprite = sp;
    }


}
