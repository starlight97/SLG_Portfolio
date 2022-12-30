using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    int[] dx;
    int[] dy;
    public class Node
    {
        public bool isWall;
        public int y, x;
        public Node(bool isWall, int y, int x)
        {
            this.isWall = isWall;
            this.y = y;
            this.x = x;
        }
    }

    public SpriteAtlas atlas;

    public List<OtherObject> OtherObjectList
    {
        private set;
        get;
    }

    private List<Vector3> spawnTilePosList;
    private Node[,] map;
    private int topY, topX;
    private bool[,] visited;
    public void Init(List<Vector3> spawnTilePosList)
    {
        this.OtherObjectList = new List<OtherObject>();
        this.spawnTilePosList = spawnTilePosList;
    }


    private int grassRange = 15;

    // 잔디만 임시로 일단 만드는 메소드
    public void SpawnGrassInit(int rightTopY, int rightTopX, bool[,] wallPosArr, int amount)
    {
        this.map = new Node[rightTopX, rightTopY];
        this.topY = rightTopY;
        this.topX = rightTopX;
        this.visited = new bool[topX, topY];

        dx = new int[] {-1, 1 ,0 ,0};
        dy = new int[] {0, 0, 1, -1 };


        for (int y = 0; y < rightTopY - 1; y++)
        {
            for (int x = 0; x < rightTopX - 1; x++)
            {
                //bool isWall = wallPosArr[x, y];
                map[x, y] = new Node(true, x, y);
            }
        }

        foreach (var pos in spawnTilePosList)
        {
            map[(int)pos.x, (int)pos.y].isWall = false;
        }

        foreach (var obj in OtherObjectList)
        {
            map[(int)obj.transform.position.x, (int)obj.transform.position.y].isWall = true;
        }

        grassSpawnPosList = new List<Vector3>();

        int grassCount = 0;
        for (int y = 4; y < rightTopY - 1; y++)
        {            
            for (int x = 4; x < rightTopX - 1; x++)
            {
                if (map[x,y].isWall == false && visited[x,y] == false)
                {
                    BFS(x, y);
                    if(grassSpawnPosList.Count > grassRange)
                    {
                        var rand = Random.Range(0, 20);
                        if (rand == 0)
                        {
                            grassCount++;
                            SpawnGrass(grassSpawnPosList);
                            if (grassCount > amount)
                                return;
                        }
                        else
                            continue;
                    }
                    grassSpawnPosList.Clear();
                }
            }
        }
    }

    public void SpawnGrass(List<Vector3> grassSpawnPosList)
    {
        var datas = DataManager.instance.GetDataList<RuckData>().ToList();
        var grassDatas = datas.FindAll(x => x.ruck_name == "잔디");
        int spawnCount = 0;
        foreach (var pos in grassSpawnPosList)
        {
            var randIndex = Random.Range(0, grassDatas.Count);
            Vector3 spawnPos = new Vector3();
            spawnPos.x = pos.x;
            spawnPos.y = pos.y;
            SpawnObject(grassDatas[randIndex].prefab_name, grassDatas[randIndex].sprite_name, spawnPos);
            spawnCount++;
            if (spawnCount >= grassRange)
                return;
        }
    }


    private List<Vector3> grassSpawnPosList;

    private void BFS(int x, int y)
    {
        int deep = 0;
        Queue<Vector3> q = new Queue<Vector3>();
        Vector3 pos = new Vector3(x, y, 0);
        grassSpawnPosList.Add(pos);
        visited[x, y] = true;
        q.Enqueue(pos);
        while (q.Count > 0 && deep < grassRange)
        {
            Vector3 cur = q.Dequeue();

            for (int dir = 0; dir < 4; dir++)
            {
                // nx = nextX, ny = nextY             
                int nx = (int)cur.x + dx[dir];
                int ny = (int)cur.y + dy[dir];
                // 동서남북 탐색중 배추밭을 벗어나거나 
                if (nx < 0 || nx >= topX || ny < 0 || ny >= topY)
                    continue;

                if (visited[nx, ny] == true || map[nx, ny].isWall == true)
                    continue;

                visited[nx, ny] = true;
                Vector3 nextPos = new Vector3(nx, ny);
                q.Enqueue(nextPos);
                grassSpawnPosList.Add(nextPos);
                deep += 1;
            }
        }
    }


    public void SpawnObject(string prefab_name, string sprite_name, Vector3 pos)
    {
        GameObject objGo = Instantiate(Resources.Load<GameObject>(prefab_name),
    pos, Quaternion.identity);
        objGo.transform.parent = this.transform;

        var otherObj = objGo.GetComponent<OtherObject>();
        otherObj.Init(atlas.GetSprite(sprite_name));
        otherObj.onDestroy = (obj) =>
        {
            DestroyObject(obj);
        };
        this.OtherObjectList.Add(otherObj);
    }

    public void SpawnObject(string prefab_name, string sprite_name)
    {
        if (spawnTilePosList.Count == 0)
        {
            Debug.Log("빈공간 없음");
            return;
        }

        var randPosIdx = Random.Range(0, spawnTilePosList.Count);

        Vector3 spawnPos = spawnTilePosList[randPosIdx];
        if (WallCheck(spawnPos) == true)
        {
            spawnTilePosList.RemoveAt(randPosIdx);
            return;
        }
        GameObject objGo = Instantiate(Resources.Load<GameObject>(prefab_name), spawnPos, Quaternion.identity);
        objGo.transform.parent = this.transform;

        var otherObj = objGo.GetComponent<OtherObject>();
        otherObj.Init(atlas.GetSprite(sprite_name));
        otherObj.onDestroy = (obj) =>
        {
            DestroyObject(obj);
        };
        this.OtherObjectList.Add(otherObj);
        spawnTilePosList.RemoveAt(randPosIdx);
    }

    public void DestroyObject(OtherObject obj)
    {
        this.OtherObjectList.Remove(obj);
    }

    // true 장애물 있음
    // false 장애물 없음
    private bool WallCheck(Vector3 pos)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Object")) + (1 << LayerMask.NameToLayer("WallObject"))
                        + (1 << LayerMask.NameToLayer("Wall"));    // Object 와 WallObject 레이어만 충돌체크함
        var col = Physics2D.OverlapBox(new Vector2(pos.x + 0.5f, pos.y + 0.5f), new Vector2(0.95f, 0.95f), 0, layerMask);
        if (col != null)
        {
            return true;
        }
        return false;
    }
}
