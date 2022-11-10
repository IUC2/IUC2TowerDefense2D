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

    private Camera          mainCamera;
    private Ray             mouseBtnDownRay;
    private Ray             mouseBtnUpRay;
    private RaycastHit      mouseBtnDownHit;
    private RaycastHit      mouseBtnUpHit;
    private RaycastHit      UIHit;
    private Transform       mouseBtnDownHitTransform = null;//���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����
    private Transform       mouseBtnUpHitTransform = null;//���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����
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

            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));  // Everything���� Player ���̾ �����ϰ� �浹 üũ��
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
                //Ÿ�� �̹��� �������
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
                if (towerClicked)//Tower Ŭ�� -->
                {

                    //Ư�� layer�� raycast�����ϱ� (1)
                    int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));
                    mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity, layerMask))
                    {
                        mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                        //���� tile ���� �� tower ���� ����
                        if (mouseBtnUpHitTransform.CompareTag("PlacedTower"))
                        {
                            if (!towerManager.GetComponent<TowerManager>().TowerUpgrade(mouseBtnUpHitTransform.gameObject, clickedTower)){
                                Debug.Log("SpawnTower2");
                                clickedTower.transform.position = saveVector + Vector3.back;
                                clickedTower.layer = LayerMask.NameToLayer("PlacedTower");
                                clickedTower.tag = "PlacedTower";
                                SystemTextViewer.systemTextViewer.PrintText(SystemType.Build);//���� ��ġ�� Ÿ�� �Ǽ��� �Ұ��� ���
                            }
                            Debug.Log("2�ܰ� ��ü");
                        }
                        else if (mouseBtnUpHitTransform.CompareTag("Tile"))
                        {
                            if (mouseBtnUpHitTransform.gameObject.GetComponent<Tile>().IsBuildTower == true)
                            {
                                //���� �ڸ� ��ȯ
                                Debug.Log("SpawnTower2");
                                clickedTower.transform.position = saveVector + Vector3.back;
                                clickedTower.layer = LayerMask.NameToLayer("PlacedTower");
                                clickedTower.tag = "PlacedTower";
                                SystemTextViewer.systemTextViewer.PrintText(SystemType.Build);//���� ��ġ�� Ÿ�� �Ǽ��� �Ұ��� ���

                            }
                            else
                            {
                                //�ڸ� �̵�
                                Debug.Log("SpawnTower2");
                                towerSpawner.SpawnTower2(mouseBtnUpHitTransform, clickedTower);
                            }
                            
                        }
                    }
                    else
                    {
                        Debug.Log("elsePointerUp");
                        //���⿡�� ���� �ڸ��� �����ϵ��� ����
                        if (EventSystem.current.IsPointerOverGameObject())
                        {
                            //�Ǹ�
                            clickedTower.GetComponent<TowerWeapon>().Sell();
                        }
                        else
                        {
                            //���� �ڸ� ��ȯ
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
                    //�ʿ� ����
                    Debug.Log("Ư������2");
                }
            }
            else
            {
                //click
                Debug.Log("ick");
                towerSpawner.DestroyFollowTowerClone();
                isDragTime = 0;
            }
            sellPanel.SetActive(false);
        }
    }
}