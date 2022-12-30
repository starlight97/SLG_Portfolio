using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    private Coroutine moveRoutine;

    // �÷��̾� �̵� ��ũ��Ʈ
    // �Ű����� pathList�� �Է¹޾� ��� ������ �����Դϴ�.
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
                // dir(����) = Ÿ�ٹ��� - �÷��̾� ���� ��ġ
                var dir = pathList[index] - this.transform.position;
                this.transform.Translate(dir.normalized * this.moveSpeed * Time.deltaTime);

                // Ÿ����ġ�� ������ġ�� �Ÿ����̰� 0.1���ϰ� �ɽ� while�� �������ɴϴ�
                var distance = dir.magnitude;
                if (distance <= 0.1f)
                {
                    this.transform.position = pathList[index];
                    break;
                }
                    
                yield return null;
            }
        }
        // move��ƾ �������Ƿ� null�� �ʱ�ȭ
        this.moveRoutine = null;
    }

}

// �ۼ��� : ������
// ������ ���� : 2022-08-10 
// �÷��̾� �̵� ��ũ��Ʈ �Դϴ�.
