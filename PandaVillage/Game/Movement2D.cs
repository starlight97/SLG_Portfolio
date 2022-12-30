using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement2D : MonoBehaviour
{
    public float moveSpeed;
    private Coroutine moveRoutine;
    private Coroutine moveActionRoutine;
    public List<Vector3> pathList = new List<Vector3>();
    private Vector3 dir;

    public UnityAction<Vector3> onMoveComplete;
    public UnityAction<Vector3> onMoveActionComplete;
    public UnityAction<Vector3> onPlayAnimation;


    // 플레이어 이동 스크립트
    // 매개변수 pathList를 입력받아 경로 단위로 움직입니다.
    public void Move()
    {
        if (moveActionRoutine != null || moveRoutine != null)
        {
            StopAllCoroutines();
        }
        moveRoutine = this.StartCoroutine(this.MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        int pathCount = this.pathList.Count;
        for (int index = 1; index < this.pathList.Count; index++)
        {
            this.dir = this.pathList[index] - this.transform.position;
            this.onPlayAnimation(this.dir);
            if (pathCount < 1)
                break;


            while (true)
            {
                // dir(방향) = 타겟방향 - 플레이어 현재 위치
                this.dir = this.pathList[index] - this.transform.position;
                this.transform.Translate(this.dir.normalized * this.moveSpeed * Time.deltaTime);

                // 타겟위치와 현재위치의 거리차이가 0.1이하가 될시 while문 빠져나옵니다
                var distance = this.dir.magnitude;
                if (distance <= 0.15f)
                {
                    this.transform.position = this.pathList[index];
                    break;
                }

                yield return null;
            }
        }
        // move루틴 끝났으므로 null로 초기화
        this.moveRoutine = null;
        this.onMoveComplete(this.dir);
    }

    public void MoveAction()
    {
        if (moveActionRoutine != null || moveRoutine != null)
        {
            StopAllCoroutines();
        }
        moveActionRoutine = StartCoroutine(MoveActionRoutine());
    }

    private IEnumerator MoveActionRoutine()
    {
        int pathCount = this.pathList.Count;

        for (int index = 1; index < pathCount; index++)
        {
            this.dir = this.pathList[index] - this.transform.position;

            this.onPlayAnimation(this.dir);
            if (pathCount < 1)
                break;

            while (true)
            {
                // dir(방향) = 타겟방향 - 플레이어 현재 위치
                this.dir = this.pathList[index] - this.transform.position;
                this.transform.Translate(this.dir.normalized * this.moveSpeed * Time.deltaTime);

                // 타겟위치와 현재위치의 거리차이가 0.1이하가 될시 while문 빠져나옵니다
                var distance = this.dir.magnitude;
                if (distance <= 0.15f)
                {
                    this.transform.position = this.pathList[index];
                    break;
                }

                yield return null;
            }
        }
        // move루틴 끝났으므로 null로 초기화
        this.moveActionRoutine = null;
        this.onMoveActionComplete(this.dir);
    }

}

// 작성자 : 박정식
// 마지막 수정 : 2022-08-10 
// 플레이어 이동 스크립트 입니다.


