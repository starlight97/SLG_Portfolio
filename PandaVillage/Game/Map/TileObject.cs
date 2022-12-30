using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tile Object Data")]
public class TileObject : ScriptableObject
{
    public List<TileBase> tiles;

    public bool isField;
}
