using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] enemyAPoints;
    public UnityAction<Vector3, int> onEnemyDie;


    public void CreateEnemies()
    {
        this.StartCoroutine(this.CreateEnemiesRoutine());
    }

    private IEnumerator CreateEnemiesRoutine()
    {
        while (true)
        {
            this.CreateEnemy();
            yield return new WaitForSeconds(1f);
        }
    }
    public void CreateEnemy()
    {
        var randEnemyIndex = Random.Range(0, 2);
        var go = Instantiate<GameObject>(this.enemyPrefabs[randEnemyIndex]);

        var randX = Random.Range(this.enemyAPoints[0].position.x, this.enemyAPoints[1].position.x);
        var y = this.enemyAPoints[0].position.y;
        var initPos = new Vector3(randX, y, 0);
        go.transform.position = initPos;
        var enemy = go.GetComponent<Enemy>();
        enemy.Init();
        enemy.onDie = (dieStatus) =>
        {
            if(Enemy.eDieStatus.Kill == dieStatus)
            {
                this.onEnemyDie(enemy.transform.position, enemy.score);
                Destroy(go.gameObject);
            }
        };
    }
}
