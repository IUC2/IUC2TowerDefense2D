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
    [SerializeField]
    private TowerManager towerManager;

    public bool ispaused;

    private Camera          mainCamera;
    private Ray             mouseBtnDownRay;
    private Ray             mouseBtnUpRay;
    private RaycastHit      mouseBtnDownHit;
    private RaycastHit      mouseBtnUpHit;
    private Transform       mouseBtnDownHitTransform = null;
    private Transform       mouseBtnUpHitTransform = null;
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
        if (GameManager.gameManager.ispaused == true)
        {
            return;
        }
            
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
            if (isDragTime >= 0.001f)
            {
                //Drag
                Debug.Log("Drag");
                isDragTime = 0;
                if (towerClicked)//Tower 클릭 -->
                {

                    //특정 layer만 raycast제외하기 (1)
                    int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));
                    mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity, layerMask))
                    {
                        mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                        //기존 tile 정보 및 tower 정보 갱신
                        if (mouseBtnUpHitTransform.CompareTag("PlacedTower"))
                        {
                            if (!towerManager.GetComponent<TowerManager>().TowerUpgrade(mouseBtnUpHitTransform.gameObject, clickedTower)){
                                Debug.Log("SpawnTower2");
                                clickedTower.transform.position = saveVector + Vector3.back;
                                clickedTower.layer = LayerMask.NameToLayer("PlacedTower");
                                clickedTower.tag = "PlacedTower";
                                SystemTextViewer.systemTextViewer.PrintText(SystemType.Build);//현재 위치에 타워 건설이 불가능 출력
                            }
                            Debug.Log("2단계 합체");
                        }
                        else if (mouseBtnUpHitTransform.CompareTag("Tile"))
                        {
                            if (mouseBtnUpHitTransform.gameObject.GetComponent<Tile>().IsBuildTower == true)
                            {
                                //이전 자리 귀환
                                Debug.Log("SpawnTower2");
                                clickedTower.transform.position = saveVector + Vector3.back;
                                clickedTower.layer = LayerMask.NameToLayer("PlacedTower");
                                clickedTower.tag = "PlacedTower";
                                SystemTextViewer.systemTextViewer.PrintText(SystemType.Build);//현재 위치에 타워 건설이 불가능 출력

                            }
                            else
                            {
                                //자리 이동
                                Debug.Log("SpawnTower2");
                                towerSpawner.SpawnTower2(mouseBtnUpHitTransform, clickedTower);
                            }
                            
                        }
                    }
                    else
                    {
                        Debug.Log("elsePointerUp");
                        //여기에서 원래 자리로 복귀하도록 설정
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            //판매
                            clickedTower.GetComponent<TowerWeapon>().Sell();
                        }
                        else
                        {
                            //이전 자리 귀환
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
                else
                {
                    //필요 구간
                    Debug.Log("특이지점2");
                }
            }
            else
            {
                //click
                Debug.Log("ick");
                towerSpawner.DestroyFollowTowerClone();
                isDragTime = 0;
                GameObject target = null;
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray2D ray = new Ray2D(pos, Vector2.zero);
                RaycastHit2D hit;
                hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                if (hit)
                {
                    target = hit.collider.gameObject;
                    if(target.tag == "Enemy")//Enemy Touch
                    {
                        Debug.Log("Enemy Clicked");
                        target.GetComponent<EnemyHP>().TakeDamage(0.5f);
                    }
                }
            }
            sellPanel.SetActive(false);
        }
    }
}