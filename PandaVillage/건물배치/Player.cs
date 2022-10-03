using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityAction<GameObject> onSelectedBuilding;
    public bool isBuildingSelected = false;

    private void Update()
    {
        // 마우스 클릭시 좌표를 인게임 좌표로 변환하여 mousePos 변수에 저장
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // z값은 사용을 안하므로 x, y 값만 저장후 Move
        int targetPosX = (int)Math.Truncate(mousePos.x);
        int targetPosY = (int)Math.Truncate(mousePos.y);

        int currentPosX = (int)Math.Truncate(this.transform.position.x);
        int currentPosY = (int)Math.Round(this.transform.position.y);
        Vector2Int curPos = new Vector2Int(currentPosX, currentPosY);
        Vector2Int targetPos = new Vector2Int(targetPosX, targetPosY);

        // 왼쪽 마우스 클릭 시 타일 변경됨
        if (Input.GetMouseButtonDown(0)) 
        {
            this.pos = new Vector3Int(currentPosX, currentPosY, 0);
            Vector3Int tpos = new Vector3Int((int)mousePos.x, (int)mousePos.y, 0);

            GameObject findGo = FindObject(tpos);
            
            if (findGo != null && isBuildingSelected == false && findGo.tag == "Building")
            {
                findGo.transform.position = new Vector3(-10, -10, 0);
                this.onSelectedBuilding(findGo);
                isBuildingSelected = true;
                return;
            }
          
        }
    }

    private GameObject FindObject(Vector3Int tpos)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Object")) + (1 << LayerMask.NameToLayer("WallObject"))
            + (1 << LayerMask.NameToLayer("Crop"));    // Object 와 WallObject 레이어만 충돌체크함
        GameObject findGo = null;

        var col = Physics2D.OverlapBox(new Vector2(tpos.x + 0.5f, tpos.y + 0.5f), new Vector2(0.95f, 0.95f),0, layerMask);

        if (col != null)
        {
            findGo = col.gameObject;
            //OtherObject obj = col.gameObject.GetComponent<OtherObject>();
            //obj.DestroyObject();
        }
        return findGo;
    }
}

