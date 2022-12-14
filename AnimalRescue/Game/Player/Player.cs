using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;  // OnDrawGizmos

public class Player : MonoBehaviour
{
    private enum eStateType
    {
        None = -1,
        Idle, Run, Die, Hit
    }

    public int Id { get; private set; }

    private GameObject heroGo;
    private Transform hpGaugePoint;

    public PlayerLife playerLife = new PlayerLife();
    private PlayerMove playerMove;
    private PlayerStats playerStats;

    private Coroutine hitRoutine;
    public UnityAction onDie;
    private Animator anim;

    public UnityAction<Vector3> onUpdateMove;
    public UnityAction<int> onLevelUp;
    public UnityAction<float, float> onUpdateHp;
    public UnityAction onGiveRiviveChance;
    public UnityAction onGameOver;

    private bool isRivive = false;
    private bool isDie = false;

    public void Init(int heroId)
    {
        this.Id = heroId;

        var heroData = DataManager.instance.GetData<HeroData>(this.Id);
        var info = InfoManager.instance.GetInfo();

        this.heroGo = Instantiate(Resources.Load<GameObject>(heroData.prefab_path));
        this.heroGo.name = "model";
        this.heroGo.transform.parent = this.transform;

        this.hpGaugePoint = this.transform.Find("HpGaugePoint").GetComponent<Transform>();
        this.anim = this.GetComponentInChildren<Animator>();
        anim.keepAnimatorControllerStateOnDisable = true;
        SetState(eStateType.Idle);

        this.playerStats = GetComponent<PlayerStats>();
        this.playerMove = GetComponent<PlayerMove>();

        this.playerMove.onMove = () =>
        {
            if (hitRoutine != null)
            {
                return;
            }
            SetState(eStateType.Run);
        };
        this.playerMove.onMoveComplete = () => 
        {
            if (this.hitRoutine == null)
                SetState(eStateType.Idle);
        };

        this.playerStats.Init(this.Id, 1);

        this.playerMove.moveSpeed = this.playerStats.moveSpeed;

        this.playerMove.Init();
        playerLife.MaxHp = playerStats.maxHp;
        playerLife.Hp = playerLife.MaxHp;

        FindEnemys();

        this.playerStats.onLevelUp = (amount) =>
        {
            this.onLevelUp(amount);
        };

        this.onUpdateMove(this.hpGaugePoint.position);
    }

    public void Recovery(float hp, float maxHp, float per)
    {
        playerLife.Hp *= per;
        if (playerLife.Hp >= playerLife.MaxHp)
            playerLife.Hp = playerLife.MaxHp;
        onUpdateHp(hp, maxHp);
    }

    public void Hit(int damage)
    {
        this.playerLife.Hp -= damage;

        if (this.playerLife.Hp <= 0)
        {
            // ?????? ??? ??? ?????? ????????????
            if (isRivive)
            {
                this.isDie = true;
                this.Die();
                return;
            }
            // ?????? ?????? ?????? ?????? ?????????
            else if (!isRivive && !isDie)
            {
                onGiveRiviveChance();
            }
        }
        else
        {
            if (this.hitRoutine != null)
                StopCoroutine(hitRoutine);
            this.hitRoutine = StartCoroutine(HitRoutine());
            onUpdateHp(this.playerLife.Hp, this.playerLife.MaxHp);
        }
    }

    public void SetRiviveState(bool state)
    {
        isRivive = state;
    }

    public void SetDieState(bool state)
    {
        isDie = state;
        Die();
    }

    private IEnumerator HitRoutine()
    {
        SetState(eStateType.Hit);
        var length = this.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(length);
        this.hitRoutine = null;
    } 

    private void Die()
    {
        if (isDie)
        {
            this.playerMove.StopMove();
            StartCoroutine(DieRoutine());
        }
    }

    private IEnumerator DieRoutine()
    {
        SetState(eStateType.Die);
        var length = this.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        this.onGameOver();
        yield return new WaitForSeconds(length);

        this.onDie();
        yield break;
    }

    private void SetState(eStateType state)
    {
        this.anim.SetInteger("State", (int)state);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDie && collision.collider.tag == "Enemy")
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            this.Hit(enemy.damage);
        }
    }

    #region ?????? ?????? ????????? ??????
    // ?????? ????????? ???????????? ?????? ??????
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask enemyMask;

    // Target mask??? ray hit??? transform??? ???????????? ?????????
    public List<Transform> visibleEnemyList = new List<Transform>();

    public void FindEnemys()
    {
        StartCoroutine(FindEnemysWithDelay(0.1f));
    }

    IEnumerator FindEnemysWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleEnemys();
        }
    }

    void FindVisibleEnemys()
    {
        visibleEnemyList.Clear();
        // viewRadius??? ??????????????? ??? ??? ?????? ??? targetMask ???????????? ??????????????? ?????? ?????????
        Collider[] EnemysInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);

        for (int i = 0; i < EnemysInViewRadius.Length; i++)
        {
            Transform target = EnemysInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // ??????????????? forward??? target??? ????????? ?????? ????????? ?????? ?????????
            if (Vector3.Angle(this.heroGo.transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                visibleEnemyList.Add(target);
            }
        }
    }

    // y??? ????????? ?????? 3?????? ?????? ????????? ????????????.
    // ?????? ?????????: https://nicotina04.tistory.com/197
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }
    #endregion
}