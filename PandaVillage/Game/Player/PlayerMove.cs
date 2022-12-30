using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    private Coroutine moveRoutine;

    // 플레이어 이동 스크립트
    // 매개변수 pathList를 입력받아 경로 단위로 움직입니다.
    public void Move(List<Vector3> pathList)
    {
        if (this.moveRoutine != null)
            this.StopCoroutine(moveRoutine);
        moveRoutine = this.StartCoroutine(this.MoveRoutine(pathList));
    }
    
    private IEnumerator MoveRoutine(List<Vector3> pathList)
    {
        int pathCount = pathList.Count;

        for (int index = 1; index < pathList.Count; index++)
        {
            if (pathCount < 1)
                break;

            while (true)
            {
                // dir(방향) = 타겟방향 - 플레이어 현재 위치
                var dir = pathList[index] - this.transform.position;
                this.transform.Translate(dir.normalized * this.moveSpeed * Time.deltaTime);

                // 타겟위치와 현재위치의 거리차이가 0.1이하가 될시 while문 빠져나옵니다
                var distance = dir.magnitude;
                if (distance <= 0.1f)
                {
                    this.transform.position = pathList[index];
                    break;
                }
                    
                yield return null;
            }
        }
        // move루틴 끝났으므로 null로 초기화
        this.moveRoutine = null;
    }

}

// 작성자 : 박정식
// 마지막 수정 : 2022-08-10 
// 플레이어 이동 스크립트 입니다.
