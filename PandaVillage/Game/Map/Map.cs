using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private Tilemap tilemap;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Debug.Log(transform.GetChild(i).transform);
        }
    }


}
