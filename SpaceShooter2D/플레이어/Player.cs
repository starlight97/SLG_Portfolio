using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private enum eSoundState
    {
        EatItem, Attack, Bomb
    }

    public float moveSpeed = 2f;
    public Transform[] firePivots;
    private Animator anim;
    public GameObject playerBulletPrefab;
    public GameObject playerBullet2Prefab;
    public GameObject bombPrefab;
    public UnityAction<int> onEatItem;
    public UnityAction<int> onHit;
    public UnityAction onDie;
    public UnityAction<int> onUseBomb;
    public int maxHp;
    public int currentHp;
    public float invincibilityTime;
    private int power = 0;
    public int bombCount = 2;
    private SpriteRenderer spriteRenderer;
    private bool isInvincibility = false;

    public AudioClip audioEatItem;
    public AudioClip audioAttack;
    public AudioClip audioBomb;
    private AudioSource audioSource;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.audioSource = GetComponent<AudioSource>();
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
        this.Attack();
    }

    private void Hit(int damage)
    {
        if(!isInvincibility)
        {            
            currentHp -= damage;
            if (currentHp <= 0)
            {
                this.onDie();
            }
            this.onHit(currentHp);
            StartCoroutine(InvincibilityRoutine());
            StartCoroutine(HitRoutine());
        }
    }
    private IEnumerator HitRoutine()
    {
        this.anim.SetTrigger("isHit");
        yield return new WaitForSeconds(0.3f);
        this.transform.position = new Vector3(0, -7, 0);
        int posY = -7;        
        Color color = spriteRenderer.color;          

        while (isInvincibility)
        {
            if(posY < -3)
            {
                posY++;
                this.transform.position = new Vector3(0, posY, 0);
            }

            color.a = 0;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.1f);
            color.a = 255;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 무적시간 코루틴 
    // 무적시간 동안 이동제어
    private IEnumerator InvincibilityRoutine()
    {
        float moveSpeedtemp = moveSpeed;
        moveSpeed = 0;
        isInvincibility = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincibility = false;
        moveSpeed = moveSpeedtemp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Hit(100);
            Hit(enemy.damage);
            this.onHit(currentHp);
        }
        else if (collision.tag == "EnemyBullet")
        {
            var enemyBullet = collision.gameObject.GetComponent<EnemyBullet>();
            Hit(enemyBullet.damage);
            this.onHit(currentHp);
        }
        else if(collision.tag == "Item")
        {
            var item = collision.gameObject.GetComponent<Item>();

            EatItem(item.score, item.itemName);
            Destroy(item.gameObject);
        }
    }

    private void EatItem(int score, string itemName)
    {
        PlaySound(eSoundState.EatItem);
        if (itemName == "Bomb" && bombCount < 2)
        {
            bombCount++;
        }

        if (itemName == "Power" && power < 3)
        {
            power++;
        }
        else
        {
            this.onEatItem(score);
        }

    }
    private void Move()
    {
        int dirX = (int)Input.GetAxisRaw("Horizontal");
        int dirY = (int)Input.GetAxisRaw("Vertical");

        if (this.transform.position.x < -2.5 && dirX == -1)
        {
            dirX = 0;
        }
        else if (this.transform.position.x > 2.5 && dirX == 1)
        {
            dirX = 0;
        }
        if (this.transform.position.y < -4.5 && dirY == -1)
        {
            dirY = 0;
        }
        else if (this.transform.position.y > 4.5 && dirY == 1)
        {
            dirY = 0;
        }
        // h : -1, 0, 1
        this.anim.SetInteger("dirX", dirX);

        // 방향 * 속도 * 시간
        Vector3 dir = new Vector3(dirX, dirY, 0);
        // 길이가1인 단위벡터
        this.transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
    }
    private void Attack()
    {
        // 총알 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound(eSoundState.Attack);
            GameObject bulletGo = null;
            if (power == 3)
            {
                bulletGo = Instantiate<GameObject>(this.playerBullet2Prefab, this.firePivots[0].position, Quaternion.identity);
            }
            else
            {
                bulletGo = Instantiate<GameObject>(this.playerBulletPrefab, this.firePivots[0].position, Quaternion.identity);

            }
            if (power >= 1)
            {
                bulletGo = Instantiate<GameObject>(this.playerBulletPrefab, this.firePivots[1].position, Quaternion.identity);
            }
            if (power >= 2)
            {
                bulletGo = Instantiate<GameObject>(this.playerBulletPrefab, this.firePivots[2].position, Quaternion.identity);
            }
            //bulletGo.transform.position = this.firePivot.position;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (bombCount > 0)
            {
                PlaySound(eSoundState.Bomb);
                bombCount--;
                var bombGo = Instantiate<GameObject>(this.bombPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                this.onUseBomb(bombCount);
            }
        }
    }

    private void PlaySound(eSoundState state)
    {
        if (state == eSoundState.EatItem)
        {
            audioSource.clip = audioEatItem;
        }
        else if (state == eSoundState.Attack)
        {
            audioSource.clip = audioAttack;
        }
        else if (state == eSoundState.Bomb)
        {
            audioSource.clip = audioBomb;
        }
        audioSource.Play();
    }

}
