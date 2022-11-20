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
    private Transform mouseBtnUpHitTransform = null;//���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����
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
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));  // Everything���� Player ���̾ �����ϰ� �浹 üũ��
            mouseBtnUpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseBtnUpRay, out mouseBtnUpHit, Mathf.Infinity, layerMask))
            {
                mouseBtnUpHitTransform = mouseBtnUpHit.transform;
                if (mouseBtnUpHit.transform.CompareTag("PlacedTower"))
                {
                    //��ü
                    Debug.Log("1�ܰ� ��ü ����");
                    GameObject.Find("TowerManager").GetComponent<TowerManager>().TowerUpgrade2(mouseBtnUpHitTransform.gameObject, towerSpawner.towerTemplate[towerindex].weapon[0].name);
                    

                }
                else if (mouseBtnUpHit.transform.CompareTag("Tile"))
                {
                    clickedTower = towerSpawner.SpawnTower(mouseBtnUpHitTransform);
                }
                else
                {
                    //�ʿ� ����
                    Debug.Log("UIClick3");
                }

            }
            else
            {
                //�ʿ� ����
                Debug.Log("UIClick2");
            }
        }
        else
        {
            //click
            Debug.Log("UIClick");
        }
        //���� ���� ó������� �ϴ� ���� UI�� ��ġ�� ��Ȳ or ��� ��ġ�� ��Ȳ
        towerSpawner.DestroyFollowTowerClone();
        mouseBtnUpHitTransform = null;
        isDragTime = 0f;
    }
}
