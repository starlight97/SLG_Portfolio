using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : BarnAnimal
{
    public Animator shearedSheepanim;

    private void Start()
    {
        onProduceItem = () => {
            GrowUp();
        };
    }
    public override void GrowUp()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);

        if (isFull)
        {
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(true);
        }

    }

    public override void SetAnimation(Vector3 dir)
    {

         if(this.yummyDay > 6 && isFull)
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
        else if (this.yummyDay > 6 && !isFull)
        {
            // 상
            if (dir.x == 0 && dir.y > 0)
            {
                this.shearedSheepanim.SetInteger("State", (int)eStateType.RunTop);
            }

            // 하
            else if (dir.x == 0 && dir.y < 0)
            {
                this.shearedSheepanim.SetInteger("State", (int)eStateType.RunBottom);
            }

            //좌
            else if (dir.x < 0)
            {
                this.shearedSheepanim.SetInteger("State", (int)eStateType.RunLeft);
            }

            // 우
            else if (dir.x > 0)
            {
                this.shearedSheepanim.SetInteger("State", (int)eStateType.RunRight);
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
}
