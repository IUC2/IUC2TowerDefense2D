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
    private Transform       hitTransform = null;//���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����

    private void Awake()
    {
        //"Main Camera" �±׸� ���� ������Ʈ�� Ž�� ��, Camera ������Ʈ ���� ����
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();�� ����
        mainCamera = Camera.main;
    }

    //private bool IsPointerOverUIObject()//UI �� ���� ������Ʈ�� Ŭ���Ǵ� ���� ����
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}

    private void Update()
    {
        //���콺�� UI ���� ���� ��� �Ʒ� �ڵ尡 ������� �ʵ��� �� Ÿ�� "Upgrade"�� �� ��, Ÿ�� ������ ��Ȱ��ȭ ���� �ʵ��� ����
        if(EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        //���콺 ���� ��ư�� ������ ��
        //if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶� ��ġ���� ȭ���� ���콺�� ��ġ�� �����ϴ� ���� ����
            //ray.origin: ������ ������ġ(=ī�޶� ��ġ)
            //ray.direction: ������ ���� ����
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //2D ����͸� ���� 3D ������ ������Ʈ�� ���콺�� �����ϴ� ���
            //������ �ε����� ������Ʈ�� ������ hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                //������ �ε��� ������Ʈ�� �±װ� "Tile"�̸�
                if (hit.transform.CompareTag("Tile")) { 
                    //Ÿ���� �����ϴ� SpawnTower() ȣ��
                    towerSpawner.SpawnTower(hit.transform);//������ �ε��� �� �ִ� ������ Collider�� 3D�� �͸� ����
                }//Ÿ�Ͽ� Ÿ���� ���� ������ �ε��� ������Ʈ�� �±װ� "Tower"�̸�
                else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);//Ÿ�� ������ ����ϴ� Ÿ�� ����â ON 
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //���콺�� ������ ��, ������ ������Ʈ�� ���ų� ������ ������Ʈ�� Ÿ���� �ƴϸ�
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                //Ÿ�� ���� �г��� ��Ȱ��ȭ
                towerDataViewer.OffPanel();
            }
            hitTransform = null;
        }
    }

}