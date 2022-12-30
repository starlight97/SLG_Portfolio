using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectPlaceManager : MonoBehaviour
{
    private Coroutine buildingEditRoutine;
    private GameObject gridGo;
    private GameObject buildingGo;
    
    private Vector3 buildingOldPos;
    public GameObject tilePrefab;
    public GameObject[,] tiles;
    public UnityAction<Vector3> onBuildComplete;
    public UnityAction<Vector3, Vector3> onMoveComplete;

    public UnityAction onEditCancel;

    public UnityAction onFindWallPosList;
    public bool[,] wallPosArr;

    private int width = 5;
    private int height = 5;

    private Vector2 doorPos = Vector2.zero;
    private Vector2 sidedoorPos = Vector2.zero;

    private bool possibleBuild = false;
    public AudioClip buildCilp;

    public void Init(GameObject selectedBuildingGo)
    {
        gridGo = transform.Find("Grid").gameObject;
        int id = selectedBuildingGo.GetComponent<TransparentObject>().id;
        var data = DataManager.instance.GetData<BuildingData>(id);
        this.width = data.width;
        this.height = data.height;
        if (data.door_pos != "NULL")
        {
            doorPos = StrToPos(data.door_pos);
        }
        if (data.side_door_pos != "NULL")
        {
            sidedoorPos = StrToPos(data.door_pos);
        }
        buildingGo = selectedBuildingGo;
        buildingOldPos = selectedBuildingGo.transform.position;
        buildingGo.transform.position = new Vector3(-10, -10, 0);
        CreateTile();
        StartCoroutine(InitRoutine());
    }
    public void Init(int id)
    {
        gridGo = transform.Find("Grid").gameObject;
        var data = DataManager.instance.GetData<BuildingData>(id);
        this.width = data.width;
        this.height = data.height;

        if (data.door_pos != "NULL")
        {
            doorPos = StrToPos(data.door_pos);
        }
        if (data.side_door_pos != "NULL")
        {
            sidedoorPos = StrToPos(data.door_pos);
        }

        CreateTile();
        StartCoroutine(InitRoutine());
    }

    private Vector2 StrToPos(string spos)
    {
        string[] split_data = spos.Split(',');
        Vector2 pos = new Vector2();
        pos.y = Convert.ToInt32(split_data[0]);
        pos.x = Convert.ToInt32(split_data[1]);
        return pos;
    }
    private IEnumerator InitRoutine()
    {        
        yield return new WaitForSeconds(0.1f);
        onFindWallPosList();
    }   

    public void BuildingEdit()
    {
        if (buildingEditRoutine != null)
            StopCoroutine(buildingEditRoutine);
            
        buildingEditRoutine = StartCoroutine(BuildingEditRoutine());
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private IEnumerator BuildingEditRoutine()
    {
        this.gridGo.transform.localPosition = new Vector3(12, 46, 0);
        CheckTile();
        yield return null;
        while (true)
        {
            yield return null;
            if (EventSystem.current.IsPointerOverGameObject() == true || IsPointerOverUIObject() == true)
            {
                continue;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                possibleBuild = true;
                this.gridGo.transform.localPosition = new Vector3((int)mousePos.x, (int)mousePos.y, 0);
                CheckTile();
            }
        }
    }


    private void CreateTile()
    {
        tiles = new GameObject[width, height];
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

    private void CheckTile()
    {
        foreach (var tile in tiles)
        {
            tile.transform.GetChild(1).gameObject.SetActive(false);
            tile.transform.GetChild(2).gameObject.SetActive(false);
            tile.transform.GetChild(3).gameObject.SetActive(false);
            tile.transform.GetChild(4).gameObject.SetActive(false);
            tile.transform.GetChild(5).gameObject.SetActive(false);
        }
        if(doorPos != Vector2.zero)
        {
            tiles[(int)doorPos.x, (int)doorPos.y].transform.GetChild(2).gameObject.SetActive(true);
        }
       
        foreach (var tile in tiles)
        {
            int x = (int)tile.transform.position.x;
            int y = (int)tile.transform.position.y;

            if (wallPosArr[x, y] == true)
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
            tiles[1, 0].transform.GetChild(3).gameObject.SetActive(true);
        }
    }
    private void DeleteTile()
    {
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
    }

    // 건물 이동
    public void BuildingMove()
    {
        if (possibleBuild == true)
        {
            //buildingGo.transform.position = new Vector3((int)mousePos.x, (int)mousePos.y, 0);
            buildingGo.transform.position = this.gridGo.transform.localPosition;
            SoundManager.instance.PlaySound(buildCilp);
            StopCoroutine(buildingEditRoutine);
            buildingEditRoutine = null;
            //this.onEditComplete(buildingGo.transform.position);
            this.onMoveComplete(buildingOldPos, buildingGo.transform.position);
            DeleteTile();
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
        }
    }

    // 건물 건설
    public void BuildingBuildExecute()
    {
        if (possibleBuild == true)
        {
            StopCoroutine(buildingEditRoutine);
            buildingEditRoutine = null;
            this.onBuildComplete(gridGo.transform.position);
            DeleteTile();
        }
        else
        {
            SoundManager.instance.PlaySound(SoundManager.eButtonSound.Fail);
        }
    }

    public void BuildingEditCancel()
    {
        if(buildingEditRoutine != null)
        {
            DeleteTile();
            StopCoroutine(buildingEditRoutine);
            buildingEditRoutine = null;
            buildingGo.transform.position = buildingOldPos;
            //this.onEditComplete(Vector3.zero);
        }
        //onEditCancel();
    }



}
