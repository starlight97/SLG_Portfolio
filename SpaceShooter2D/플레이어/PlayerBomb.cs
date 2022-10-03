using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    public int damage;
    public float boomLength;

    private void Start()
    {
        StartCoroutine(BoomEffectRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            // 적을 공격 
            collision.GetComponent<Enemy>().Hit(this.damage);
        }        
    }

    private IEnumerator BoomEffectRoutine()
    {
        yield return new WaitForSeconds(boomLength);
        Destroy(this.gameObject);
    }
}
