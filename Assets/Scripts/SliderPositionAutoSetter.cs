using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down;
    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        //Slider UI�� �i�ƴٴ� target ����
        targetTransform = target;
        
        //RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
    }
    private void LateUpdate()
    {
        //���� �ı��Ǿ� �i�ƴٴ� ����� �������, Slider UI�� ����
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        //������Ʈ�� ��ġ�� ���ŵ� ���� Slider UI�� �԰� ��ġ�� �����ϵ��� �ϱ� ���� LateUpdate()���� ȣ��

        //������Ʈ�� ���� ��ǥ�� �������� ��ũ�������� ��ǥ�� ����
        Vector3 screenPosition = targetTransform.position;
        //ȭ�� ������ ��ǥ + distance��ŭ ������ ��ġ�� slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }
}
