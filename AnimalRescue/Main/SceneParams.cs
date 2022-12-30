using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneParams
{

}

public class LobbyMainParam : SceneParams
{
    public int heroId;
}

public class GameMainParam : SceneParams
{
    public int heroId;
}


public class RepairShopParam : SceneParams
{
    public int heroId;
}

public class GameOverMainParam : SceneParams
{
    public int heroId;
}
