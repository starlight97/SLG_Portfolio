using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropInfo
{
    // 작물 아이디
    public int id;
    // 작물 sprite 상태 변경 변수
    public int state;

    // 물 준 횟수
    public int wateringCount;
    //포지션
    public int posX;
    public int posY;

    public bool isWatering;

}
