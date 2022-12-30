using System.Collections;
using System.Collections.Generic;

public class GameInfo 
{
    public string gpgsid;
    public PlayerInfo playerInfo;
    public Dictionary<int, HeroInfo> dicHeroInfo;
    public Dictionary<int, int> dicKillEnemyInfo;

    public GameInfo(string gpgsid)
    {
        this.gpgsid = gpgsid;
        this.playerInfo = new PlayerInfo(500, 0, 0);
        this.dicHeroInfo = new Dictionary<int, HeroInfo>();
        this.dicKillEnemyInfo = new Dictionary<int, int>();
    }
}
