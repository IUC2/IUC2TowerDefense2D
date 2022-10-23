using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();//ī�޶� ������Ʈ�� ������
        Rect rect = camera.rect;//ī�޶��� rect���� ������
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (���� / ����)//float�� ���� ������ �������� ���� �� �Ҽ����� ���ֱ� ����
        //�����ϰ� ���� ����(9:16)���� �ػ� ����
        float scaleWidth = 1f / scaleHeight;
        if(scaleHeight < 1)//��, �Ʒ��� ���� ��� y���� �̼� ����
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else//��, ��, ��, �ϰ� �� ��� 
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
            camera.rect = rect;
        }

    }
}
