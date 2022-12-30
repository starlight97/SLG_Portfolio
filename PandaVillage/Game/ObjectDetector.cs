using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ObjectDetector : MonoBehaviour
{
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit2D hit;
    private Transform hitTransform = null; // 마우스 픽킹으로 선택한 오브젝트 임시 저장
    public UnityAction<GameObject> onClickBuilding;

    private Coroutine detectingRoutine;

    public void Init()
    {
        // "MainCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 와 동일
        mainCamera = Camera.main;

        Detecting();
    }

    public void Detecting()
    {
        if (detectingRoutine != null)
            StopCoroutine(detectingRoutine);
        detectingRoutine = StartCoroutine(this.DetectingRoutine());
    }
    public void StopDetecting()
    {
        StopCoroutine(detectingRoutine);
        detectingRoutine = null;
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private IEnumerator DetectingRoutine()
    {
        while(true)
        {
            yield return null;
            // 마우스가 UI에 머물러 있을 때는 아래 코드가 실행되지 않도록 함
            if (EventSystem.current.IsPointerOverGameObject() == true || IsPointerOverUIObject() == true)
            {
                continue;
            }
            // 마우스 왼쪽 버튼을 눌렀을 때
            if (Input.GetMouseButtonDown(0))
            {
                // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
                // ray.origin : 광선의 시작위치(=카메라 위치)
                // ray.direction : 광선의 진행방향
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                // 2D 모니터를 통해 2D 월드의 오브젝트를 마우스로 선택하는 방법
                // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
                hit = Physics2D.Raycast(ray.origin, mainCamera.transform.forward, Mathf.Infinity);
                if(hit.collider != null)
                {
                    hitTransform = hit.transform;
                    // 광선에 부딪힌 오브젝트의 태그가 "Tower"이면
                    if (hit.transform.CompareTag("Building"))
                    {
                        this.onClickBuilding(hit.transform.gameObject);
                    }
                }
                
            }
            else if(Input.GetMouseButtonUp(0))
            {
                // 마우스를 눌렀을 때 선택한 오브젝트가  없거나 선택한 오브젝트가 타워가 아니면
                if (hitTransform == null || hitTransform.CompareTag("Building") == false)
                {
                    hitTransform = null;

                }
            }
        }
        detectingRoutine = null;
        yield return null;
    }

}
