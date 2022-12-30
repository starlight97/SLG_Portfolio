using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager
{
    public static readonly AnimalManager instance = new AnimalManager();

    private AnimalManager() { }
    public Dictionary<int, Animal> AnimalDic = new Dictionary<int, Animal>();
    public bool coopOpened =false;
    private int i =0;
    public void AddAnimal()
    {
        AnimalDic.Add(i, Resources.Load<Rabbit>("Prefabs/Rabbit"));
        i++;

        AnimalDic.Add(i, Resources.Load<Cow>("Prefabs/Cow"));
        i++;

        Debug.Log(AnimalDic.Count);
    }

   
}
