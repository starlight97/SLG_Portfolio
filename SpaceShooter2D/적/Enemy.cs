using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public enum eDieStatus
    {
        Kill, OutOfRange
    }
    public UnityAction<eDieStatus> onDie;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer hitSpriteRenderer;
    public int maxHp;
    public int currentHp;
    public int score;
    public float speed;
    public int damage;

    private Coroutine hitEffectRoutine;


    private void Update()
    {
        Move();
    }
    public virtual void Move()
    {
        this.transform.Translate(Vector3.down * this.speed * Time.deltaTime);
        if (this.transform.position.y <= -5.5f)
        {
            Die(eDieStatus.OutOfRange);
        }
    }

    private IEnumerator HitEffectRoutine()
    {
        spriteRenderer.enabled = false;
        hitSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;
        hitSpriteRenderer.enabled = false;
    }
    public void Hit(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {            
            Die(eDieStatus.Kill);
        }
        if (hitEffectRoutine != null) hitEffectRoutine = null;
        hitEffectRoutine = StartCoroutine(HitEffectRoutine());
    }
    
    private void Die(eDieStatus dieStatus)
    {
        this.tag = "Untagged";
        this.onDie(dieStatus);            
    }


    public virtual void Init()
    {
        currentHp = maxHp;
        spriteRenderer = this.transform.GetComponent<SpriteRenderer>();
        hitSpriteRenderer = this.transform.Find("Hit").GetComponent<SpriteRenderer>();
        hitSpriteRenderer.enabled = false;
    }
}
