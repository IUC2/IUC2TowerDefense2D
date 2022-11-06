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
    private Transform       mouseBtnDownHitTransform = null;//���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����
    private Transform       mouseBtnUpHitTransform = null;//���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����

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
            //���콺�� UI ���� ���� ��� �Ʒ� �ڵ尡 ������� �ʵ��� �ϴ� ��
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
                //Ÿ�� �̹��� �������
                Debug.Log("TileClicking");
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (towerClicked)//Tower Ŭ�� -->
            {
                mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity))
                {
                    mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                    if (mouseBtnUpHitTransform.CompareTag("Tower"))//Tower�� ��ġ
                    {
                        Debug.Log("TowerUp");
                    }
                    else if (mouseBtnUpHitTransform.CompareTag("Tile"))//Tile�� ��ġ
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