using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    private float baseMoveSpeed;

    //public float            MoveSpeed => moveSpeed;//moveSpeed ������ Property(Get ���)
    public float MoveSpeed//Set & Get�� ������ Property�� ����
    {
        set => moveSpeed = Mathf.Max(0, value);//�̵��ӵ��� ������ ���� �ʵ��� ����
        get => moveSpeed;
    }

    private void Awake()
    {
        baseMoveSpeed = moveSpeed;
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }
}
//File: Movement2D.cs
//Desc: �̵� ������ ��� ������Ʈ�� ����
//Functions: MoveTo(): �ܺο��� ȣ���� �̵� ���� ����
