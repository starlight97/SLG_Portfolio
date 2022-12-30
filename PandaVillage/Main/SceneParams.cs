using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneParams
{
    public Vector3 SpawnPos;
}

public class FarmEditParam : SceneParams
{
    public int objectId;
    public int editType;
}
public class CoopParam : SceneParams
{
    public CoopInfo coopInfo;
}