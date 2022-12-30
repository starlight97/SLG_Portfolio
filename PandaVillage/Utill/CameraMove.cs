using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector2 clickPoint;
    float dragSpeed = 60.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) clickPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (Input.GetMouseButton(0))
        {

            Vector3 position = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - clickPoint);

            //position.z = position.y;
            //position.y = .0f;

            Vector3 move = position * (Time.deltaTime * dragSpeed);

            float z = transform.position.z;

            transform.Translate(-move);

            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, z);

            if (newPos.x >= 66)
                newPos.x = 66;
            if (newPos.y >= 56)
                newPos.y = 56;

            if (newPos.x <= 14)
                newPos.x = 14;
            if (newPos.y <= 9)
                newPos.y = 9;

            transform.position = newPos;
        }
    }
}
