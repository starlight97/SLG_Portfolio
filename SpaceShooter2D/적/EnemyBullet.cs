using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 dir;
    public float speed;
    public int damage;

    public void Init(Vector3 dir)
    {
        this.dir = dir;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Delete();
    }

    private void Move()
    {
        this.transform.Translate(dir * this.speed * Time.deltaTime);
    }
    private void Delete()
    {
        if (Vector3.Distance(this.transform.position, Vector3.zero) > 10)
        {
            Destroy(this.gameObject);
        }
    }
}
