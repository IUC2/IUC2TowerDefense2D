using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        //ȭ���� ���콺 ��ǥ�� �������� ���� ���� ���� ��ǥ�� ���Ѵ�.
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        
        //Camera.ScreenToWorldPoint: ȭ�� ���� ��ǥ�� ���� ��ǥ�� ��ȯ�ϴ� �Լ�
        transform.position = mainCamera.ScreenToWorldPoint(position);

        //z�� ��ġ�� 0���� �����Ѵ�.
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
