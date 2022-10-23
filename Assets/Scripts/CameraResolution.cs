using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();//카메라 컴포넌트를 가져옴
        Rect rect = camera.rect;//카메라의 rect값을 가져옴
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)//float가 붙은 이유는 정수끼리 연산 시 소수점을 없애기 때문
        //고정하고 싶은 비율(9:16)으로 해상도 설정
        float scaleWidth = 1f / scaleHeight;
        if(scaleHeight < 1)//위, 아래가 남는 경우 y값을 미세 조정
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else//좌, 우, 상, 하가 긴 경우 
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
            camera.rect = rect;
        }

    }
}
