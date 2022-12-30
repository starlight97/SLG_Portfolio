using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionSetter : MonoBehaviour
{
    private GameObject playerGo;
    public float offsetX;
    public float offsetY;
    public Vector2 bottomLeft;
    public Vector2 topRight;

    private void Start()
    {
        this.playerGo = GameObject.FindObjectOfType<Player>().gameObject;
    }

    private void LateUpdate()
    {
        Vector3 newPos = playerGo.transform.position;
        
        newPos.y += 1;
        newPos.z = -5;

        if (newPos.x <= bottomLeft.x)
            newPos.x = bottomLeft.x;
        if(newPos.y <= bottomLeft.y)
            newPos.y = bottomLeft.y;

        if (newPos.x >= topRight.x)
            newPos.x = topRight.x;
        if (newPos.y >= topRight.y)
            newPos.y = topRight.y;

        newPos.x += offsetX;
        newPos.y += offsetY;
        this.transform.position = newPos;
    }


}
