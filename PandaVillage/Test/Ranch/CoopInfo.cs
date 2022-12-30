using System.Collections;
using System.Collections.Generic;

public class CoopInfo 
{  
    public int coopId;
    public List<AnimalInfo> animalinfoList;
    public List<ProductInfo> productInfoList;
    public int posX;
    public int posY;

    //닭장이 열린채로 저장되어있는가?
    public bool isOpen;
        
    public CoopInfo(int coopId, int posX, int posY) 
    {
        this.coopId = coopId;
        this.animalinfoList = new List<AnimalInfo>();
        this.productInfoList = new List<ProductInfo>();
        this.posX = posX;
        this.posY = posY;
        this.isOpen = false;
    }

    public AnimalInfo GetAnimalInfo(string animalName)
    {
        var info = animalinfoList.Find(x => x.animalName == animalName);
        return info;
    }
    
}


