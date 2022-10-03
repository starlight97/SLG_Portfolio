using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPlaceManager : MonoBehaviour
{
    private Coroutine buildingEditRoutine;
    private GameObject gridGo;
    public GameObject tilePrefab;
    public GameObject[,] tiles;
    public UnityAction onEditComplete;

    public UnityAction onFindWallPosList;
    public bool[,] wallPosArr;

    private int width = 5;
    private int height = 5;

    private bool possibleBuild = false;

    private void Start()
    {
        gridGo = transform.Find("Grid").gameObject;
    }

    public void BuildingEdit(GameObject selectedBuildingGo)
    {
        CreateTile();

        if (buildingEditRoutine == null)
            buildingEditRoutine = StartCoroutine(BuildingEditRoutine(selectedBuildingGo));
    }

    private IEnumerator BuildingEditRoutine(GameObject selectedBuildingGo)
    {

        yield return new WaitForSeconds(0.1f);
        onFindWallPosList();

        while (true)
        {
            possibleBuild = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        
            this.gridGo.transform.localPosition = new Vector3((int)mousePos.x, (int)mousePos.y, 0);

            foreach (var tile in tiles)
            {
                tile.transform.GetChild(1).gameObject.SetActive(false);
                tile.transform.GetChild(2).gameObject.SetActive(false);
                tile.transform.GetChild(3).gameObject.SetActive(false);
                tile.transform.GetChild(4).gameObject.SetActive(false);
                tile.transform.GetChild(5).gameObject.SetActive(false);
            }
            tiles[1, 0].transform.GetChild(2).gameObject.SetActive(true);
            foreach (var tile in tiles)
            {
                int x = (int)tile.transform.position.x;
                int y = (int)tile.transform.position.y;

                if (wallPosArr[x,y] == true)
                {
                    possibleBuild = false;
                    if (x == 1 && y == 0)
                    {
                        tile.transform.GetChild(3).gameObject.SetActive(true);
                    }
                    else
                    {
                        tile.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }

            }

            int doorPosX = (int)tiles[1, 0].transform.position.x;
            int doorPosY = (int)tiles[1, 0].transform.position.y;

            if (doorPosY != 0 && wallPosArr[doorPosX, doorPosY - 1] == true)
            {
                possibleBuild = false;
                tiles[1,0].transform.GetChild(3).gameObject.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0) && possibleBuild == true)
            {
                selectedBuildingGo.transform.position = new Vector3((int)mousePos.x, (int)mousePos.y, 0);
                this.gridGo.transform.position = new Vector3(0, 0, 0);

                var childList = gridGo.GetComponentsInChildren<Transform>();
                if (childList != null)
                {
                    for (int i = 1; i < childList.Length; i++)
                    {
                        Destroy(childList[i].gameObject);
                    }
                }
                tiles = null;
                break;
            }

            yield return null;
        }
        this.onEditComplete();
        buildingEditRoutine = null;
    }

    private void CreateTile()
    {
        tiles = new GameObject[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject tileGo = Instantiate<GameObject>(this.tilePrefab);
                tileGo.transform.position = new Vector3(x, y, 0);

                tileGo.transform.parent = this.gridGo.transform;
                tiles[x, y] = tileGo;
            }
        }

    }
}
