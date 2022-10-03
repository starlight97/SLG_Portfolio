using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 맵전체 크기 설정 변수
    public Vector2Int mapBottomLeft, mapTopRight;

    public bool[,] GetWallPosArr()
    {
        bool[,] wallPosArr = new bool[mapTopRight.x, mapTopRight.y];

        int layerMask = (1 << LayerMask.NameToLayer("Object")) + (1 << LayerMask.NameToLayer("WallObject"))
            + (1 << LayerMask.NameToLayer("Wall"));    // Object 와 WallObject 레이어만 충돌체크함


        // 장애물 감지
        // 맵에 Layer가 Wall인 태그를 가진 오브젝트가 있는지 검사하고 있을시 지나갈수없는 통로로 설정한다.
        for (int x = mapBottomLeft.x; x < mapTopRight.x; x++)
        {
            for (int y = mapBottomLeft.y; y < mapTopRight.y; y++)
            {   
                var col = Physics2D.OverlapBox(new Vector2(x + 0.5f, y + 0.5f), new Vector2(0.95f, 0.95f), 0, layerMask);

                if(col != null)
                {
                    wallPosArr[x, y] = true;
                }
            }
        }

        return wallPosArr;
    }
}
