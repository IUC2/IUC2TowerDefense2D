using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private int towerindex;
    private Camera mainCamera;
    private Ray mouseBtnUpRay;
    private RaycastHit mouseBtnUpHit;
    private Transform mouseBtnUpHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장

    bool isDrag = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        towerSpawner.ReadyToSpawnTower(towerindex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        //이미지 따라다니기 Effect
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDrag)
        {
            Debug.Log("ClickUp");
            mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity))
            {
                mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                if (mouseBtnUpHit.transform.CompareTag("Tile"))
                {
                    Debug.Log("Tile");
                    towerSpawner.SpawnTower(mouseBtnUpHitTransform);
                }
                else
                {
                    Debug.Log("Else");
                    return;
                }
            }
            towerSpawner.DestroyFollowTowerClone();
            mouseBtnUpHitTransform = null;
            isDrag = false;
        }
        else
        {
            Debug.Log("Click");
        }
    }
}
