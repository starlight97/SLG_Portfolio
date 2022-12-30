using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Animal : MonoBehaviour
{
    protected enum eStateType
    {
        None = -1,
        RunBottom,
        RunRight,
        RunTop,
        RunLeft
    }

    public enum eAnimalSound
    {
        Chicken,
        Cow,
        Rabbit,
        Sheep
    }

    public AudioClip[] arrAnimalSound;

    public int id;
    public string animalName;           //이름
    public int friendship;              //우정, 호감도    
    //public int mood;                //기분
    public int age;                     //나이
    public int yummyDay;                 //밥먹은날

    public bool isFull = true;           //배부른가?
    public bool isPatted = false;        //쓰다듬어졌나?
    public bool isAnimalOut = false;

    private GameObject emote;


    private Coroutine roamingRoutine;

    public UnityAction<Vector2Int, Vector2Int, List<Vector3>, Animal> onDecideTargetTile;
    public UnityAction<Vector2Int, List<Vector3>> goHome;
    public UnityAction onAnimalOut;

    private Movement2D movement2D;

    public Vector2Int mapBottomLeft, mapTopRight;

    public Vector2Int RandomMoveRange = new Vector2Int(-3, 3);
    public Animator babyAnim;
    public Animator anim;


    public void Init(AnimalInfo animalInfo, Vector2Int mapTopRight)
    {
        this.mapBottomLeft = new Vector2Int(0,0);
        this.mapTopRight = mapTopRight;
        this.id = animalInfo.animalId;
        this.animalName = animalInfo.animalName;
        this.friendship = animalInfo.friendship;
        this.age = animalInfo.age;
        this.yummyDay = animalInfo.yummyDay;
        this.isFull = animalInfo.isFull;
        this.isPatted = animalInfo.isPatted;
        this.isAnimalOut = animalInfo.isAnimalOut;

        this.movement2D = GetComponent<Movement2D>();
        this.emote = this.transform.Find("emote").gameObject;

        //돌아다니기
        //Roaming();

        this.movement2D.onPlayAnimation = (dir) => {
            this.SetAnimation(dir);
        };
        movement2D.onMoveComplete = (dir) => { };

        AnimalGrow();

        AnimalRunOut();
    }        

    public void AnimalGrow()
    {     
        //6일이상 밥먹었으면 무적권 성장
        if (this.yummyDay >= 6)
        {
            this.GrowUp();
        }
    }

    // 돌아다니는 함수 입니다.
    public void Roaming()
    {
        if (this.roamingRoutine != null)
            this.StopCoroutine(this.roamingRoutine);

        this.roamingRoutine = StartCoroutine(RoamingRoutine());
    }
    #region 무브루틴
    public IEnumerator RoamingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);            
            var targetPos = new Vector2Int((int)this.transform.position.x + Random.Range(RandomMoveRange.x, RandomMoveRange.y+1), 
                (int)this.transform.position.y + Random.Range(RandomMoveRange.x, RandomMoveRange.y+1));


            #region 똥같은 코드            
            if (targetPos.x < mapBottomLeft.x)
                targetPos.x = mapBottomLeft.x;
            if (targetPos.x >= mapTopRight.x)
                targetPos.x = mapTopRight.x;
            if (targetPos.y < mapBottomLeft.y)
                targetPos.y = mapBottomLeft.y;
            if (targetPos.y >= mapTopRight.y)
                targetPos.y = mapTopRight.y;
            #endregion
            

            var curPos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);
            this.movement2D.pathList.Clear();
            onDecideTargetTile(curPos, targetPos, this.movement2D.pathList, this);
        }       
    }
    #endregion

    // 무브먼트 2D에 무브루틴 호출
    public void Move()
    {
        this.movement2D.Move();
    }

    public virtual void GrowUp()    
    {
        Debug.Log("성장함");

        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);

        //Destroy(this.transform.GetChild(0).gameObject);
        //Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/RabbitModel"), this.transform);

        //스프라이트 교체로 할경우 
        //this.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/Animals/Rabbit");
        //this.GetComponentInChildren<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/TestRabbit_0");       

    }
    public void ComeBackHome()
    {
        StopCoroutine(roamingRoutine);        
        var curPos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.y);

        this.movement2D.pathList.Clear();
        goHome(curPos, this.movement2D.pathList);
    }
    

    // 쓰다듬기는 하루에 한번만 가능
    public void Patted()
    {
        this.isPatted = true;
        this.friendship += 15;

        var animalInfo = InfoManager.instance.GetInfo().ranchInfo.GetAnimalInfo(this.animalName);
        animalInfo.isPatted = true;
        animalInfo.friendship = this.friendship;

        StartCoroutine(SetAnimalEmote());
    }   

    protected IEnumerator SetAnimalEmote()
    {
        emote.SetActive(true);
        yield return new WaitForSeconds(1.417f);
        emote.SetActive(false);
    }

    public void AnimalRunOut()
    {
        if (isAnimalOut)
        {
            this.gameObject.SetActive(true);
            this.Roaming();
        }
        else
        {
            this.gameObject.SetActive(false);
        }         
    }

    public virtual bool Produce()
    {
        return false;
    }

    public virtual void SetAnimation(Vector3 dir)
    {
        if(this.yummyDay > 6)
        {
            // 상
            if (dir.x == 0 && dir.y > 0)
            {
                this.anim.SetInteger("State", (int)eStateType.RunTop);
            }

            // 하
            else if (dir.x == 0 && dir.y < 0)
            {
                this.anim.SetInteger("State", (int)eStateType.RunBottom);
            }

            //좌
            else if (dir.x < 0)
            {
                this.anim.SetInteger("State", (int)eStateType.RunLeft);
            }

            // 우
            else if (dir.x > 0)
            {
                this.anim.SetInteger("State", (int)eStateType.RunRight);
            }
        }
        else
        {
            // 상
            if (dir.x == 0 && dir.y > 0)
            {
                this.babyAnim.SetInteger("State", (int)eStateType.RunTop);
            }

            // 하
            else if (dir.x == 0 && dir.y < 0)
            {
                this.babyAnim.SetInteger("State", (int)eStateType.RunBottom);
            }

            //좌
            else if (dir.x < 0)
            {
                this.babyAnim.SetInteger("State", (int)eStateType.RunLeft);
            }

            // 우
            else if (dir.x > 0)
            {
                this.babyAnim.SetInteger("State", (int)eStateType.RunRight);
            }
        }
    }

    public void PlaySound(int id)
    {
        eAnimalSound soundType = (eAnimalSound)(id - 8000);
        SoundManager.instance.PlaySound(arrAnimalSound[(int)soundType]);
    }
}
