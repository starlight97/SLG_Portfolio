using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogo : MonoBehaviour
{
    private UILogoImage uILogoImage;

    private void Start()
    {
        uILogoImage = transform.GetChild(1).gameObject.GetComponent<UILogoImage>();
        uILogoImage.onLogoClick = () => {
            Destroy(this.gameObject);
        };

    }


}
