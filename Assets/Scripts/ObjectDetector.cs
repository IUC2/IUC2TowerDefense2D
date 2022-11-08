using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private RaycastHit      UIHit;
    private Transform       mouseBtnDownHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장
    private Transform       mouseBtnUpHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장
    private Vector3         saveVector;

    private GameObject clickedTower = null;

    public GameObject sellPanel = null;

    private float isDragTime;
    bool towerClicked = false;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
            mouseBtnDownRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseBtnDownRay, out mouseBtnDownHit, Mathf.Infinity, layerMask))
            {
                mouseBtnDownHitTransform = mouseBtnDownHit.transform;
                if (mouseBtnDownHitTransform.CompareTag("PlacedTower"))
                {
                    towerClicked = true;
                    sellPanel.SetActive(true);
                    clickedTower = mouseBtnDownHitTransform.gameObject;
                    saveVector = clickedTower.transform.position;
                    clickedTower.layer = LayerMask.NameToLayer("Tower");
                    clickedTower.tag = "Tower";
                }
                else if (mouseBtnDownHitTransform.CompareTag("Tile"))
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
                isDragTime += Time.deltaTime;
                towerSpawner.SetDragPosition(mainCamera, clickedTower);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragTime >= 0.01f)
            {
                //Drag
                isDragTime = 0;
                if (towerClicked)//Tower 클릭 -->
                {
                    //특정 layer만 raycast제외하기 (1)
                    int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));
                    mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity, layerMask))
                    {
                        mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                        if (mouseBtnUpHitTransform.CompareTag("PlacedTower"))
                        {
                            //2단계 합체
                        }
                        else if (mouseBtnUpHitTransform.CompareTag("Tile"))
                        {
                            //기존 tile 정보 및 tower 정보 갱신
                            towerSpawner.SpawnTower2(mouseBtnUpHitTransform, clickedTower);
                        }
                        else
                        {
                            //필요 구간
                        }
                    }
                    else
                    {
                        Debug.Log("elsePointerUp");
                        //여기에서 원래 자리로 복귀하도록 설정
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            clickedTower.GetComponent<TowerWeapon>().Sell();
                        }
                        else
                        {
                            clickedTower.transform.position = saveVector + Vector3.back;
                            clickedTower.layer = LayerMask.NameToLayer("PlacedTower");
                            clickedTower.tag = "PlacedTower";
                        }
                    }
                    mouseBtnDownHitTransform = null;
                    mouseBtnUpHitTransform = null;
                    clickedTower = null;
                    towerClicked = false;
                }
            }
            else
            {
                //click
                towerSpawner.DestroyFollowTowerClone();
                isDragTime = 0;
            }
            sellPanel.SetActive(false);
        }
    }
}