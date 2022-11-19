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
    private float isDragTime;
    private Camera mainCamera;
    private Ray mouseBtnUpRay;
    private RaycastHit mouseBtnUpHit;
    private Transform mouseBtnUpHitTransform = null;//마우스 픽킹으로 선택한 오브젝트 임시 저장
    private GameObject clickedTower = null;

    [SerializeField]
    private Button button;
    private RectTransform buttonClickedTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.gameManager.ispaused == true)
        {
            return;
        }
        towerSpawner.ReadyToSpawnTower(towerindex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.gameManager.ispaused == true)
        {
            return;
        }
        int i = 0 ;
        if (i == 1)
        {
            towerSpawner.followTowerClone.SetActive(true);
            i++;
        }
        isDragTime += Time.deltaTime;
        towerSpawner.SetDragPosition(mainCamera, towerSpawner.followTowerClone);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.gameManager.ispaused == true)
        {
            return;
        }
        if (isDragTime >= 0.001f)
        {
            //Drag
            isDragTime = 0f;
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
            mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity, layerMask))
            {
                mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                if (mouseBtnUpHit.transform.CompareTag("PlacedTower"))
                {
                    //합체
                    Debug.Log("1단계 합체 구간");
                    GameObject.Find("TowerManager").GetComponent<TowerManager>().TowerUpgrade2(mouseBtnUpHitTransform.gameObject, towerSpawner.towerTemplate[towerindex].weapon[0].name);
                    

                }
                else if (mouseBtnUpHit.transform.CompareTag("Tile"))
                {
                    clickedTower = towerSpawner.SpawnTower(mouseBtnUpHitTransform);
                }
                else
                {
                    //필요 구간
                    Debug.Log("UIClick3");
                }

            }
            else
            {
                //필요 구간
                Debug.Log("UIClick2");
            }
        }
        else
        {
            //click
            Debug.Log("UIClick");
        }
        //내가 지금 처리해줘야 하는 것이 UI에 배치된 상황 or 길목에 배치된 상황
        towerSpawner.DestroyFollowTowerClone();
        mouseBtnUpHitTransform = null;
        isDragTime = 0f;
    }
}
