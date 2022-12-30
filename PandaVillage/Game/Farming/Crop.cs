using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Events;

public class Crop : OtherObject
{
    public int state;
    public int maxState;
    public int wateringCount;
    public int growthTime;
    public bool isHarvest = false;
    public bool isWatering = false;

    private SpriteRenderer spRenderer;
    [SerializeField]
    private SpriteAtlas atlas;
    private Sprite sprite;
    public UnityAction<Vector3Int, Crop> onGetWateringDirtTile;
    public UnityAction<GameObject> onHarvest;

    public void Init(int id)
    {            
        this.spRenderer = this.GetComponent<SpriteRenderer>();
        var data = DataManager.instance.GetData<CropData>(id);
        this.id = id;
        this.wateringCount = 0;
        this.state = 1;
        this.maxState = data.max_state;
        this.growthTime = data.growth_time;
        int rand = Random.Range(1, 3);
        var sprite = this.atlas.GetSprite(string.Format(data.sprite_name, rand));
        this.spRenderer.sprite = sprite;
    }

    public void Load(int id, int state, int wateringCount, bool isWatering)
    {
        this.spRenderer = this.GetComponent<SpriteRenderer>();
        var data = DataManager.instance.GetData<CropData>(id);
        this.id = id;
        this.wateringCount = wateringCount;
        this.maxState = data.max_state;
        this.growthTime = data.growth_time;
        var sprite = this.atlas.GetSprite(string.Format(data.sprite_name, state));
        this.spRenderer.sprite = sprite;
        this.isWatering = isWatering;
        this.CheckHarvest(state);
    }

    // 물타일 있냐?
    public void CheckWateringDirt(Vector3Int pos)
    {
        this.onGetWateringDirtTile(pos, this);
    }

    // 작물이 자람에 따라 스프라이트 변경됨
    public void GrowUp()
    {
        if (this.state == 1)
            this.state += 2;
        else
            this.state++;

        if (isWatering == false)
        {
            isWatering = true;
            this.wateringCount++;
        }
    }

    public bool CheckHarvest(int state)
    {
        if (state == maxState)
        {
            isHarvest = true;
        }
        else
        {
            isHarvest = false;
        }

        return isHarvest;
    }
}
