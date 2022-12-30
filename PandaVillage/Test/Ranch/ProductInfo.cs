using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductInfo 
{
    public int productId;
    public float posX;
    public float posY;

    public ProductInfo(int productId, float posX, float posY)
    {
        this.productId = productId;
        this.posX = posX;
        this.posY = posY;
    }
   
}
