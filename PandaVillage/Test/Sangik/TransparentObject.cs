using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : OtherObject
{
    public void HideObject()       //go의 SpriteRenderer와 go의 자식들의 SpriteRenderer의 알파값을 변경
    {
        if (this.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            var color = this.gameObject.GetComponent<SpriteRenderer>().color;
            color = new Color(1f, 1f, 1f, .5f);
        }

        foreach (var sprite in this.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(1f, 1f, 1f, .5f);
        }       
    }

    public void ShowObject()
    {
        if (this.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            var color = this.gameObject.GetComponent<SpriteRenderer>().color;
            color = new Color(1f, 1f, 1f, 1f);
        }

        foreach (var sprite in this.gameObject.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
