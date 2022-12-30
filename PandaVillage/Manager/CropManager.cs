using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class CropManager : MonoBehaviour
{
    public GameObject cropObject;

    public List<Crop> cropList;
    public List<Vector3Int> cropPosList;
    public UnityAction<int, Vector3Int, Crop> onGetFarmTile;
    public UnityAction onUseSeed;

    public AudioClip[] plantClips;

    public void Init()
    {
        this.cropList = new List<Crop>();
        this.cropPosList = new List<Vector3Int>();
        SpawnCrop();
        SoundManager.instance.Init();
    }

    // 해당 좌표에 레이를 쏴서 감지되는 오브젝트가 있으면 true 반환
    // 없으면 false 반환
    private bool FindCrop(Vector3Int pos)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Object")) + (1 << LayerMask.NameToLayer("WallObject"))
             + (1 << LayerMask.NameToLayer("Crop"));    // Object 와 WallObject, Crop 레이어만 충돌체크함
        var col = Physics2D.OverlapCircle(new Vector2(pos.x + 0.5f, pos.y + 0.5f), 0.4f, layerMask);
        
        if (col != null)
        {
            return true;
        }
        return false;
    }

    // 물뿌린 밭에 작물을 심고 리스트에 저장
    // 해당 타일에 작물 오브젝트가 존재하면 심지 않음
    public void CreateCrop(int seedId, Vector3Int pos)
    {
        var info = InfoManager.instance.GetInfo();

        var seedData = DataManager.instance.GetData<SeedData>(seedId);

        bool check = FindCrop(pos);

        if (check == false)
        {
            var cropData = DataManager.instance.GetData<CropData>(seedData.plant_item_id);
            GameObject cropGo = Instantiate(Resources.Load<GameObject>(cropData.prefab_path));
            cropGo.transform.position = new Vector3Int(pos.x, pos.y, 0);
            cropGo.transform.parent = this.cropObject.transform;

            Crop crop = cropGo.GetComponent<Crop>();
            crop.Init(seedData.plant_item_id);
            SoundManager.instance.PlaySound(plantClips[Random.Range(0, 2)]);

            int cropX = (int)crop.transform.position.x;
            int cropY = (int)crop.transform.position.y;

            var cropPos = new Vector3Int(cropX, cropY, 0);

            info.playerInfo.inventory.RemoveItem(seedData.id, 1);
            onUseSeed();

            crop.onDestroy = (cropGo) =>
            {
                this.cropList.Remove(cropGo.GetComponent<Crop>());
                Destroy(cropGo);
            };

            this.cropList.Add(crop);
            this.cropPosList.Add(cropPos);
        }
    }

    private void LoadCrop(int cropId, Vector3Int pos, int state, int wateringCount, bool isWatering)
    {
        var data = DataManager.instance.GetData<CropData>(cropId);
        GameObject cropGo = Instantiate(Resources.Load<GameObject>(data.prefab_path));
        cropGo.transform.position = pos;
        cropGo.transform.parent = this.cropObject.transform;

        Crop crop = cropGo.GetComponent<Crop>();
        crop.state = state;

        if (isWatering)
        {
            crop.Load(data.id, state-1, wateringCount, isWatering);
        }
        else
        {
            crop.Load(data.id, state, wateringCount, isWatering);
        }

        int cropX = (int)crop.transform.position.x;
        int cropY = (int)crop.transform.position.y;

        var cropPos = new Vector3Int(cropX, cropY, 0);
        //this.onGetFarmTile(crop.id, cropPos, crop);


        crop.onDestroy = (cropGo) =>
        {
            this.cropList.Remove(cropGo.GetComponent<Crop>());
            Destroy(cropGo);
        };
        this.cropList.Add(crop);
        this.cropPosList.Add(cropPos);
    }

    public void SpawnCrop()
    {
        var gameInfo = InfoManager.instance.GetInfo();
        if (gameInfo.cropInfoList.Count != 0)
        {
            foreach (var cropInfo in gameInfo.cropInfoList)
            {
                Vector3Int pos = new Vector3Int(cropInfo.posX, cropInfo.posY, 0);
                LoadCrop(cropInfo.id, pos, cropInfo.state, cropInfo.wateringCount, cropInfo.isWatering);
            }
        }
    }

    // 작물이 자라남
    public void GrowUpCrop()
    {

        foreach (var crop in cropList)
        {
            var x = (int)crop.transform.position.x;
            var y = (int)crop.transform.position.y;

            var pos = new Vector3Int(x, y, 0);
            if(crop.isWatering == false)
                this.onGetFarmTile(crop.id, pos, crop);
        }
    }
}
