using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner    towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera          mainCamera;
    private Ray             ray;
    private RaycastHit      hit;
    private Transform       hitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장

    private void Awake()
    {
        //"Main Camera" 태그를 가진 오브젝트를 탐색 후, Camera 컴포넌트 정보 전달
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();와 동일
        mainCamera = Camera.main;
    }

    //private bool IsPointerOverUIObject()//UI 및 게임 오브젝트가 클릭되는 현상 방지
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}

    private void Update()
    {
        //마우스가 UI 위에 있을 경우 아래 코드가 실행되지 않도록 해 타워 "Upgrade"를 할 때, 타워 정보가 비활성화 되지 않도록 설정
        if(EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        //마우스 왼쪽 버튼을 눌렀을 때
        //if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        if (Input.GetMouseButtonDown(0))
        {
            //카메라 위치에서 화면의 마우스의 위치를 관통하는 광선 생성
            //ray.origin: 광선의 시작위치(=카메라 위치)
            //ray.direction: 광선의 진행 방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
            //광선에 부딪히는 오브젝트를 검출해 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                //광선에 부딪힌 오브젝트의 태그가 "Tile"이면
                if (hit.transform.CompareTag("Tile")) { 
                    //타워를 생성하는 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);//광선과 부딪힐 수 있는 조건은 Collider가 3D인 것만 가능
                }//타일에 타워가 놓여 광선에 부딪힌 오브젝트의 태그가 "Tower"이면
                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);//타워 정보를 출력하는 타워 정보창 ON 
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //마우스를 눌렀을 때, 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                //타워 정보 패널을 비활성화
                towerDataViewer.OffPanel();
            }
            hitTransform = null;
        }
    }

}