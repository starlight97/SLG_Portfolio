using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private Tilemap groundMap;           // 땅
    private Tilemap WallMap;             // 벽 
    private Tilemap hoeDirtMap;          // 호미질 밭
    private Tilemap wateringDirtMap;     // 물뿌린 밭

    public TileBase[] tileBases;        // 0: hoeDirt, 1: wateringDirt

    public List<Vector3Int> hoeDirtPosList;
    public List<Vector3Int> wateringDirtPosList;

    public UnityAction<Vector3Int> onFinishedSetTile;

    public enum eTileType
    {
        None = -1,
        Dirt, HoeDirt, WateringDirt, Grass
    }

    public void Init()
    {
        var gridMap = GameObject.Find("GridMap");
        this.groundMap = gridMap.transform.Find("TilemapGround").GetComponent<Tilemap>();
        this.WallMap = gridMap.transform.Find("TilemapWall").GetComponent<Tilemap>();
        this.hoeDirtMap = gridMap.transform.Find("TilemapHoeDirt").GetComponent<Tilemap>();
        this.wateringDirtMap = gridMap.transform.Find("TilemapWateringDirt").GetComponent<Tilemap>();
        LoadTiles();
    }

    // 플레이어가 터치한 위치에 타일 베이스가 존재하면 True 반환
    public bool CheckTile(Vector3Int pos, eTileType state)
    {
        bool check = false;
        TileBase tilebase = groundMap.GetTile(pos);

        switch (state)
        {
            case eTileType.None:
                break;
            case eTileType.Dirt:
                if (tilebase != null && tilebase.name == "Dirt")
                    check = true;
                break;
            case eTileType.Grass:
                if (tilebase != null && tilebase.name == "Grass")
                    check = true;
                break;
            case eTileType.HoeDirt:
                if (hoeDirtMap.GetTile(pos) != null)
                    check = true;
                break;
            case eTileType.WateringDirt:
                if (wateringDirtMap.GetTile(pos) != null)
                    check = true;
                break;
            default:
                break;
        }
        return check;
    }

    // 플레이어가 들고 있는 도구에 따른 타일 변경
    public void SetTile(Vector3Int pos, Player.eItemType state)
    {
        switch (state)
        {
            case Player.eItemType.None:
                break;
            case Player.eItemType.Hoe:
                bool checkHoeDirt = CheckTile(pos, eTileType.HoeDirt);
                if (checkHoeDirt == false)
                {
                    hoeDirtMap.SetTile(pos, tileBases[0]);
                    hoeDirtPosList.Add(pos);
                }
                break;
            case Player.eItemType.WateringCan:
                bool checkWateringDirt = CheckTile(pos, eTileType.WateringDirt);
                if (checkWateringDirt == false)
                {
                    wateringDirtMap.SetTile(pos, tileBases[1]);
                    wateringDirtPosList.Add(pos);
                }
                break;
            default:
                break;
        }

        onFinishedSetTile(pos);
    }

    // 일정 시간(하루)이 지나면 물 타일을 지워준다: 플레이어가 매일 물을 줘야하기 때문이다.
    public void ClearWateringTiles()
    {
        wateringDirtMap.ClearAllTiles();
    }

    // groundMap에 존재하는 모든 타일 가져오기
    public List<Vector3> GetTilesPosList(eTileType state)
    {
        // BoundsInt
        // 타일맵의 경계를 셀 크기로 반환
        BoundsInt bounds = this.groundMap.cellBounds;
        List<Vector3> tilePosList = new List<Vector3>();

        for (int y = 0; y < bounds.size.y; y++)
        {
            for (int x = 0; x < bounds.size.x; x++)
            {
                var tile = groundMap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null && tile.name == state.ToString())
                {
                    tilePosList.Add(new Vector3(x, y, 0));
                }
            }
        }

        return tilePosList;
    }

    private void LoadTiles()
    {
        LoadHoeDirtTile();
        LoadWateringDirtTile();
    }

    private void LoadHoeDirtTile()
    {
        var gameInfo = InfoManager.instance.GetInfo();
        if (gameInfo.hoeDirtTileList.Count != 0)
        {
            foreach(var info in gameInfo.hoeDirtTileList)
            {
                var pos = new Vector3Int(info.posX, info.posY, 0);
                hoeDirtMap.SetTile(pos, tileBases[0]);
                this.hoeDirtPosList.Add(pos);
            }
        }
    }

    private void LoadWateringDirtTile()
    {
        var gameInfo = InfoManager.instance.GetInfo();
        if (gameInfo.wateringDirtTileList.Count != 0)
        {
            foreach (var info in gameInfo.wateringDirtTileList)
            {
                var pos = new Vector3Int(info.posX, info.posY, 0);
                wateringDirtMap.SetTile(pos, tileBases[1]);
                this.wateringDirtPosList.Add(pos);
            }
        }
    }
}
