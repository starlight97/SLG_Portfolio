using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Coop : TransparentObject
{

    private GameObject coopDoor;
    private CoopInfo coopInfo;
    public GameObject coopPortal;

    public List<Animal> animalList= new List<Animal>();
    public UnityAction<Vector2Int, Vector2Int, List<Vector3>, Animal> onDecideTargetTile;
    public UnityAction<GameObject> setAnimalPos;
    public UnityAction onProduceItem;


    public override void Init(Sprite sp)
    {
        base.Init(sp);

    }

    public void Init(CoopInfo coopInfo)
    {
        this.coopDoor = this.transform.GetChild(1).gameObject;
        this.coopInfo = coopInfo;

        var animalinfoList = coopInfo.animalinfoList;
        foreach (var info in animalinfoList)
        {
            CreateAnimal(info);
        }     
        //어제 문을 열어뒀을경우 문이 열려있어야함
        if (coopInfo.isOpen)
        {
            DoorOpen();
        }
        else
        {
            AnimalOut();
        }

        CoopPortalInit();
    }
    public void InitCoopScene(CoopInfo coopInfo)
    {
        this.coopDoor = this.gameObject;
        var animalinfoList = coopInfo.animalinfoList;
        foreach (var info in animalinfoList)
        {
            CreateAnimal(info);
        }      
        
        foreach (var animal in animalList)
        {

            if (!animal.isAnimalOut)
            {
                animal.gameObject.SetActive(true);
                animal.Roaming();
                setAnimalPos(animal.gameObject);
            }
            else
            {
                animal.gameObject.SetActive(false);
            }

            if (animal.GetComponent<BarnAnimal>() != null)
            {
                animal.GetComponent<BarnAnimal>().onProduceItem = () => {
                    onProduceItem();
                };
            }
        }
        SpawnProduct(coopInfo);
    }

    public void SpawnProduct(CoopInfo coopInfo)
    {
        foreach (var product in coopInfo.productInfoList)
        {
            var prefabPath = DataManager.instance.GetData<GatheringData>(product.productId).prefab_name;
            var productGo = Instantiate<GameObject>(Resources.Load<GameObject>(prefabPath));
            productGo.transform.position = new Vector3(product.posX, product.posY, 0);
            productGo.GetComponent<OtherObject>().onDestroy = (otherObj) => {
                var selectProduct= coopInfo.productInfoList.Find(x => x.posX == otherObj.transform.position.x && x.posY == otherObj.transform.position.y);
                coopInfo.productInfoList.Remove(selectProduct);
            };
        }
    }
    public void AnimalOut()
    {       
        foreach (var animal in animalList)
        {
            if (animal.isAnimalOut)
            {
                animal.gameObject.SetActive(true);
                animal.Roaming();
            }
        }

    }
    public void ClickedDoor()
    {       
        if (!coopInfo.isOpen)
        {                
            DoorOpen();

        }
        else if (coopInfo.isOpen)
        {
            DoorClose();
        }        
    }    
    

    public void DecideTargetTile(Vector2Int startPos, Vector2Int targetPos, List<Vector3>pathList, Animal animal)
    {
        this.onDecideTargetTile(startPos, targetPos, pathList, animal);
    }

    public void AnimalsGoHome()
    {
        foreach (var animal in animalList)
        {
            animal.ComeBackHome();
        }
    }

    public void CreateAnimal(AnimalInfo info)
    {
        GameObject animalGo = Instantiate(Resources.Load<GameObject>
       (DataManager.instance.GetData<AnimalData>(info.animalId).prefab_path), this.transform);
        animalGo.transform.position = coopDoor.transform.position + new Vector3(0, -1, 0);
        var animal = animalGo.GetComponent<Animal>();
        var mapTopRight = GameObject.FindObjectOfType<MapManager>().mapTopRight;        
        animal.Init(info, mapTopRight);
        animal.onDecideTargetTile = (startPos, targetPos, pathList, animal) =>
        {
            this.DecideTargetTile(startPos, targetPos, pathList, animal);
        };
        animalList.Add(animal);
    }

    public void DoorOpen()
    {            
        this.coopDoor.transform.DOLocalMoveY(0.8f,1);

        foreach (var animal in animalList)
        {
            animal.isAnimalOut = true;
            animal.AnimalRunOut();
        }
        foreach (var info in coopInfo.animalinfoList)        
            info.isAnimalOut = true;
        
        coopInfo.isOpen = true;
    }

    public void DoorClose()
    {
        this.coopDoor.transform.DOLocalMoveY(0f, 1);

        coopInfo.isOpen = false;
    }

    public void CoopPortalInit()
    {
        var portalManager = GameObject.FindObjectOfType<PortalManager>();
        var portalGo = Instantiate<GameObject>(coopPortal,this.transform.position + coopPortal.transform.position, Quaternion.Euler(0,0,0),portalManager.transform);
    }

}
