using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss01 : Enemy
{
    public float attackDelay;
    public GameObject bulletPrefab;
    public UnityAction onAppearComplete;

    public float radius = 1;
    public float degree = 0;

    public GameObject[] firePivots;

    private Coroutine appearRoutine;
    private Coroutine moveRoutine;
    private Coroutine attackRoutine;


    public override void Move()
    {
        //this.transform.Translate(Vector3.left * this.speed * Time.deltaTime);

    }



    public void Appear()
    {
        appearRoutine = this.StartCoroutine(this.AppearRoutine());
    }

    private IEnumerator AppearRoutine()
    {
        for (int i = 0; i < 100; i++)
        {
            this.transform.Translate(Vector3.down * this.speed * Time.deltaTime);
            yield return null;
        }

        for (int i = 0; i < 30; i++)
        {
            this.transform.Translate(Vector3.up * this.speed * Time.deltaTime);
            yield return null;
        }
        AppearComplete();
    }
    private void AppearComplete()
    {
        this.onAppearComplete();
        moveRoutine = StartCoroutine(MoveRoutine());
        attackRoutine = StartCoroutine(AttackRoutine());
    }
    public void Stop()
    {
        if(appearRoutine != null)
        {
            StopCoroutine(appearRoutine);
        }
        if(moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }

    private IEnumerator MoveRoutine()
    {
        while(true)
        {
            var rand = Random.Range(0, 3);
            Vector3 dir;
            if (rand == 0)
            {
                dir = Vector3.left;
            }
            else if (rand == 1)
            {
                dir = Vector3.right;
            }
            else
            {
                dir = Vector3.zero;
            }

            if (this.transform.position.x < -2.5 && dir == Vector3.left)
            {
                dir = Vector3.right;
            }
            else if (this.transform.position.x > 2.5 && dir == Vector3.right)
            {
                dir = Vector3.left;
            }

            for (int i = 0; i < 20; i++)
            {
                this.transform.Translate(dir * this.speed * Time.deltaTime);
                yield return null;
            }
        }        
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            var randAttackType = Random.Range(0, 2);
            if(randAttackType == 0 )
            {
                for (int i = 0; i < 3; i++)
                {
                    Attack();
                    yield return new WaitForSeconds(attackDelay);
                }
            }
            else if(randAttackType == 1)
            {
                RadiusAttack();
            }            
            
            yield return new WaitForSeconds(attackDelay);
        }

    }

    public override void Init()
    {
        base.Init();
    }

    private void Attack()
    {       

        for (int index = 0; index < firePivots.Length; index++)
        {                
            var bulletGo = Instantiate<GameObject>(this.bulletPrefab, firePivots[index].transform.position, Quaternion.identity);
            var bullet = bulletGo.GetComponent<Boss01Bullet>();
            bullet.Init(Vector3.down);
        }
        

    }
    private void RadiusAttack()
    {
        //var radian = this.degree * Mathf.Deg2Rad;
        for(int i=0; i<10; i++)
        {
            var radian = this.degree * Mathf.PI / 180;
            var x = Mathf.Cos(radian) * radius;
            var y = Mathf.Sin(radian) * radius;
            var pos = new Vector3(x, y, 0);
            //this.endPoint.transform.position = pos;

            var bulletGo = Instantiate<GameObject>(this.bulletPrefab);

            // 타겟위치와 방향을 구한다 
            var dir = pos - bulletGo.transform.position;

            // 각도(0, 0, 0)에서 dir(방향)만큼 회전한 회전값 
            Vector3 rotation = Quaternion.Euler(0, 0, 0) * dir;
            // 회전축 , 회전값 
            var rot = Quaternion.LookRotation(Vector3.forward, rotation);
            bulletGo.transform.rotation = rot;
            bulletGo.transform.position = pos + this.transform.position;

            var bullet = bulletGo.GetComponent<Boss01Bullet>();
            bullet.Init(dir);

            // 각도 10씩 올리기 
            this.degree += 10;
        }

    }

}
