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
        //화면의 마우스 좌표를 기준으로 게임 월드 상의 좌표를 구한다.
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        
        //Camera.ScreenToWorldPoint: 화면 상의 좌표를 월드 좌표로 변환하는 함수
        transform.position = mainCamera.ScreenToWorldPoint(position);

        //z의 위치를 0으로 설정한다.
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
