using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class Farming : MonoBehaviour
{

    // 플레이어 농사 행위 타입
    // Plant: 씨앗 심기
    // SetTile: 행위에 따른 타일 변경

    public enum eFarmActType
    {
        None = -1,
        Plant, SetTile
    }

    // 플레이어가 들고 있는 아이템이 어떤 타일을 반환해야 하는가?
    public TileManager.eTileType GetFarmTile(Player.eItemType state)
    {
        TileManager.eTileType tileType = TileManager.eTileType.None;
        switch (state)
        {
            case Player.eItemType.Hoe:
                tileType = TileManager.eTileType.Dirt;
                break;
            case Player.eItemType.WateringCan:
            case Player.eItemType.Seed:
                tileType = TileManager.eTileType.HoeDirt;
                break;
            default:
                break;
        }
        return tileType;
    }

    // 플레이어가 들고 있는 도구에 따른 행위 반환
    // ex. 호미, 물뿌리개: 타일을 바꾼다. / 씨앗: 심는다.
    public eFarmActType FarmingToolAct(Player.eItemType itemType)
    {
        eFarmActType actType = eFarmActType.None;
        switch (itemType)
        {
            case Player.eItemType.None:
                break;
            case Player.eItemType.Hoe:
            case Player.eItemType.WateringCan:
                actType = eFarmActType.SetTile;
                break;
            case Player.eItemType.Axe:
                break;
            case Player.eItemType.Seed:
                actType = eFarmActType.Plant;
                break;
            default:
                break;
        }
        return actType;
    }
}
