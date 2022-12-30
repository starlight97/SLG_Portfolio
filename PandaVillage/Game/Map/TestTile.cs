using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;     // 타일맵 사용
using System;

public class TestTile : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField]
    private TileBase tileBases;

    public void CreateTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.tilemap = GetComponent<Tilemap>();
        // 월드 포지션을 셀 포지션으로 변경
        Vector3Int location = this.tilemap.WorldToCell(mousePos);
        // 해당 셀 포지션에 타일 그리기
        this.tilemap.SetTile(location, this.tileBases);
    }

    // 타일에 커서 올리면 색 변경되는 메서드(테스트용)
    // 사용 X
    private void onMouseOver()
    {
        try
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);

            if (this.tilemap = hit.transform.GetComponent<Tilemap>())
            {
                // 타일맵의 모든 타일 새로 고침: 모든 타일에 대한 데이터 검색 및 요소 업데이트
                this.tilemap.RefreshAllTiles();

                int x, y;
                x = this.tilemap.WorldToCell(ray.origin).x;
                y = this.tilemap.WorldToCell(ray.origin).y;

                Vector3Int v3Int = new Vector3Int(x, y, 0);

                // 타일 좌표 받아와서 색 바꾸기
                this.tilemap.SetTileFlags(v3Int, TileFlags.None);
                this.tilemap.SetColor(v3Int, (Color.red));
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.LogFormat("click point :{0}", hit.point);
            }
        }
        catch (NullReferenceException)
        {

        } 
    }
}

// 작성자: 김현정
// 수정 일자: 2022-08-14
// 호미를 사용했을 때 생기는 타일 그리기 스크립트