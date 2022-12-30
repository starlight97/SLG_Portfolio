using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInfo 
{
    public string animalName;   //동물마다 서로다른 이름을 가지고 있어야함
    public int animalId;    //animalData id : 동물의 종류를 알아냄
    public int friendship;
    public int age;
    public int yummyDay;

    public bool isFull;
    public bool isPatted;
    public bool isAnimalOut;
    public AnimalInfo(string animalName, int animalId) 
    {
        this.animalName = animalName;
        this.animalId = animalId;
        this.friendship = 0;
        this.age = 1;
        this.yummyDay = 0;
        this.isFull = true;
        this.isPatted = false;
        this.isAnimalOut = false;
    }   
 
    
}
