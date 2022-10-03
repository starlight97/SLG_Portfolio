using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Movement2D movement2D;

    public UnityAction<Vector2Int, Vector2Int, List<Vector3>> onDecideTargetTile;


    private Vector3Int dir;
    private Vector3Int pos;


    private void Start()
    {
        this.movement2D = GetComponent<Movement2D>();
    }

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
        
        if (Input.GetMouseButtonDown(1))
        {
            this.pos = new Vector3Int(currentPosX, currentPosY, 0);
            Vector3Int tpos = new Vector3Int((int)mousePos.x, (int)mousePos.y, 0);

            this.movement2D.pathList.Clear();
            this.onDecideTargetTile(curPos, targetPos, this.movement2D.pathList);
        }
    }

    public void Move()
    {
        this.movement2D.Move();
    }

}


