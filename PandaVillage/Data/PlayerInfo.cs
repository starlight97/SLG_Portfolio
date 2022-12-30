using System.Collections;
using System.Collections.Generic;

public class PlayerInfo
{
    // 인벤토리 정보
    public Inventory inventory;
    // 유저 이름
    public string playerName;
    // 농장 이름
    public string farmName;
    // 좋아 하는 것
    public string favoritething;
    // 선호하는 펫
    public string pet;
    // 유저 보유 골드
    public int gold;
    // 게임 진행 시간 
    public int playYear;
    public int playDay;
    // 플레이어 스태미너
    public int currentEnegy;
    // 광산에서 사용할 체력
    public int currentHp;

    //배송상자를 이용해서 얻은 수익
    public int dailySaleGold;
    //마지막으로 배송시킨 아이템
    public InventoryData lastShippedItem;

    public PlayerInfo(string playerName,string farmName, string favoritething, string pet, int gold = 500,
        int playYear = 1, int playDay = 1, int currentEnegy = 100, int currentHp = 100)
    {
        inventory = new Inventory(12);
        this.playerName = playerName;
        this.farmName = farmName;
        this.favoritething = favoritething;
        this.pet = pet;
        this.gold = gold;
        this.playYear = playYear;
        this.playDay = playDay;
        this.currentEnegy = currentEnegy;
        this.currentHp = currentHp;
    }
}
