using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class Node
    {
        public Node(bool _isWall, int _y, int _x) 
        { 
            isWall = _isWall; 
            y = _y; 
            x = _x; 
        }

        public bool isWall;
        public Node ParentNode;

        // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
        public int y, x, G, H;
        public int F 
        { 
            get { return G + H; } 
        }
    }

    int sizeX, sizeY;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;
    // 맵전체 크기 설정 변수
    public Vector2Int mapBottomLeft, mapTopRight;
    // 맵탐색 범위 설정 변수
    private Vector2Int bottomLeft, topRight;
    public int searchRange;
    public List<Node> FinalNodeList;

    public bool allowDiagonal, dontCrossCorner;

    // 장애물 감지후 경로에 반영 로직
    private void WallSetting(int sizeY, int sizeX)
    {
        // 장애물 감지
        // 맵에 Layer가 Wall인 태그를 가진 오브젝트가 있는지 검사하고 있을시 지나갈수없는 통로로 설정한다.
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapBoxAll(new Vector2(x + bottomLeft.x + 0.5f, y + bottomLeft.y + 0.5f), new Vector2(0.95f, 0.95f), 0))
                {
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall") || col.gameObject.layer == LayerMask.NameToLayer("WallObject")) 
                        isWall = true;

                }

                NodeArray[y, x] = new Node(isWall, y + bottomLeft.y, x + bottomLeft.x);
            }
        }
    }


    public void PathFinding(Vector2Int startPos, Vector2Int targetPos, List<Vector3> pathList)
    {
        // x , y
        if (startPos.x < targetPos.x)
        {
            bottomLeft.x = startPos.x - searchRange;
            topRight.x = targetPos.x + searchRange;
        }
        else
        {
            bottomLeft.x = targetPos.x - searchRange;
            topRight.x = startPos.x + searchRange;
        }

        if (startPos.y < targetPos.y)
        {
            bottomLeft.y = startPos.y - searchRange;
            topRight.y = targetPos.y + searchRange;
        }
        else
        {
            bottomLeft.y = targetPos.y - searchRange;
            topRight.y = startPos.y + searchRange;
        }

        if (bottomLeft.x < mapBottomLeft.x)
            bottomLeft.x = mapBottomLeft.x;
        if (bottomLeft.y < mapBottomLeft.y)
            bottomLeft.y = mapBottomLeft.y;
        if (topRight.x >= mapTopRight.x)
            topRight.x = mapTopRight.x;
        if (topRight.y >= mapTopRight.y)
            topRight.y = mapTopRight.y;

        // 맵크기 설정
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;

        NodeArray = new Node[sizeY, sizeX];        

        WallSetting(sizeY, sizeX);
        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.y - bottomLeft.y, startPos.x - bottomLeft.x];
        TargetNode = NodeArray[targetPos.y - bottomLeft.y, targetPos.x - bottomLeft.x];
        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count; i++)
                {
                    Vector3 path = new Vector3(FinalNodeList[i].x, FinalNodeList[i].y, 0);
                    pathList.Add(path);
                }
                return;
            }

            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.y + 1, CurNode.x + 1);
                OpenListAdd(CurNode.y - 1, CurNode.x + 1);
                OpenListAdd(CurNode.y - 1, CurNode.x - 1);
                OpenListAdd(CurNode.y + 1, CurNode.x - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.y, CurNode.x + 1);
            OpenListAdd(CurNode.y + 1, CurNode.x);
            OpenListAdd(CurNode.y, CurNode.x - 1);
            OpenListAdd(CurNode.y - 1, CurNode.x);
        }
    }


    // 벽오브젝트로 가는길 탐색
    public void WallPathFinding(Vector2Int startPos, Vector2Int targetPos, List<Vector3> pathList)
    {

        // x , y
        if (startPos.x < targetPos.x)
        {
            bottomLeft.x = startPos.x - searchRange;
            topRight.x = targetPos.x + searchRange;
        }
        else
        {
            bottomLeft.x = targetPos.x - searchRange;
            topRight.x = startPos.x + searchRange;
        }

        if (startPos.y < targetPos.y)
        {
            bottomLeft.y = startPos.y - searchRange;
            topRight.y = targetPos.y + searchRange;
        }
        else
        {
            bottomLeft.y = targetPos.y - searchRange;
            topRight.y = startPos.y + searchRange;
        }

        if (bottomLeft.x < mapBottomLeft.x)
            bottomLeft.x = mapBottomLeft.x;
        if (bottomLeft.y < mapBottomLeft.y)
            bottomLeft.y = mapBottomLeft.y;
        if (topRight.x >= mapTopRight.x)
            topRight.x = mapTopRight.x;
        if (topRight.y >= mapTopRight.y)
            topRight.y = mapTopRight.y;

        // 맵크기 설정
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;

        NodeArray = new Node[sizeY, sizeX];

        WallSetting(sizeY, sizeX);
        //NodeArray[targetPos.y, targetPos.x].isWall = false;


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.y - bottomLeft.y, startPos.x - bottomLeft.x];
        TargetNode = NodeArray[targetPos.y - bottomLeft.y, targetPos.x - bottomLeft.x];
        TargetNode.isWall = false;
        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count-1; i++)
                {
                    Vector3 path = new Vector3(FinalNodeList[i].x, FinalNodeList[i].y, 0);
                    pathList.Add(path);
                }
                return;
            }

            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.y + 1, CurNode.x + 1);
                OpenListAdd(CurNode.y - 1, CurNode.x + 1);
                OpenListAdd(CurNode.y - 1, CurNode.x - 1);
                OpenListAdd(CurNode.y + 1, CurNode.x - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.y, CurNode.x + 1);
            OpenListAdd(CurNode.y + 1, CurNode.x);
            OpenListAdd(CurNode.y, CurNode.x - 1);
            OpenListAdd(CurNode.y - 1, CurNode.x);
        }
    }

    void OpenListAdd(int checkY, int checkX)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkY >= bottomLeft.y && checkY < topRight.y + 1 && checkX >= bottomLeft.x && checkX < topRight.x + 1 
            && !NodeArray[checkY - bottomLeft.y, checkX - bottomLeft.x].isWall && !ClosedList.Contains(NodeArray[checkY - bottomLeft.y, checkX - bottomLeft.x]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.y - bottomLeft.y, checkX - bottomLeft.x].isWall && NodeArray[checkY - bottomLeft.y, CurNode.x - bottomLeft.x].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.y - bottomLeft.y, checkX - bottomLeft.x].isWall || NodeArray[checkY - bottomLeft.y, CurNode.x - bottomLeft.x].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkY - bottomLeft.y, checkX - bottomLeft.x];
            int MoveCost = CurNode.G + (CurNode.y - checkY == 0 || CurNode.x - checkX == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.y - TargetNode.y) + Mathf.Abs(NeighborNode.x - TargetNode.x)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0)
        {
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x + 0.5f, FinalNodeList[i].y + 0.5f), new Vector2(FinalNodeList[i + 1].x + 0.5f, FinalNodeList[i + 1].y + 0.5f));
            }
        }
    }

    public bool[,] GetWallPosArr()
    {
        bool[,] wallPosArr = new bool[mapTopRight.x, mapTopRight.y];

        //int layerMask = (1 << LayerMask.NameToLayer("Object")) + (1 << LayerMask.NameToLayer("WallObject"))
        //    + (1 << LayerMask.NameToLayer("Wall"));    // Object 와 WallObject 레이어만 충돌체크함


        int layerMask =(1 << LayerMask.NameToLayer("WallObject"))
            + (1 << LayerMask.NameToLayer("Wall"));    // Object 와 WallObject 레이어만 충돌체크함


        // 장애물 감지
        // 맵에 Layer가 Wall인 태그를 가진 오브젝트가 있는지 검사하고 있을시 지나갈수없는 통로로 설정한다.
        for (int x = mapBottomLeft.x; x < mapTopRight.x; x++)
        {
            for (int y = mapBottomLeft.y; y < mapTopRight.y; y++)
            {
                var col = Physics2D.OverlapBox(new Vector2(x + 0.5f, y + 0.5f), new Vector2(0.95f, 0.95f), 0, layerMask);

                if (col != null)
                {
                    wallPosArr[x, y] = true;
                }
            }
        }
        return wallPosArr;
    }

}

// 작성자 : 박정식
// 마지막 수정 : 2022-09-06 
// 맵을 관리하는 스크립트 입니다.
// 경로탐색 알고리즘으로 에이스타 알고리즘을 사용하고 있습니다.
