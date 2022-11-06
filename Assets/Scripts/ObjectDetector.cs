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
    private Ray             mouseBtnDownRay;
    private Ray             mouseBtnUpRay;
    private RaycastHit      mouseBtnDownHit;
    private RaycastHit      mouseBtnUpHit;
    private Transform       mouseBtnDownHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장
    private Transform       mouseBtnUpHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장

    bool towerClicked = false;
    bool uiBtnClicked = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseBtnDownRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            //마우스가 UI 위에 있을 경우 아래 코드가 실행되지 않도록 하는 것
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                return;
            }

            if (Physics.Raycast(mouseBtnDownRay, out mouseBtnDownHit, Mathf.Infinity))
            {
                mouseBtnDownHitTransform = mouseBtnDownHit.transform;

                if (mouseBtnDownHitTransform.CompareTag("Tower"))
                {
                    towerClicked = true;
                    Debug.Log("TowerDown");
                }
                else if (mouseBtnDownHitTransform.CompareTag("Tile"))
                {
                    Debug.Log("TileDown");
                    return;
                }
                else
                {
                    return;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (towerClicked)
            {
                //타워 이미지 따라오기
                Debug.Log("TileClicking");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (towerClicked)//Tower 클릭 -->
            {
                mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity))
                {
                    mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                    if (mouseBtnUpHitTransform.CompareTag("Tower"))//Tower에 배치
                    {
                        Debug.Log("TowerUp");
                    }
                    else if (mouseBtnUpHitTransform.CompareTag("Tile"))//Tile에 배치
                    {
                        Debug.Log("TileUp");
                    }
                }
            }
            mouseBtnDownHitTransform = null;
            mouseBtnUpHitTransform = null;
            towerClicked = false;
            uiBtnClicked = false;
        }
    }
}