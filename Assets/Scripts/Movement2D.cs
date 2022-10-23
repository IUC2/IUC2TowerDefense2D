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

    //public float            MoveSpeed => moveSpeed;//moveSpeed 변수의 Property(Get 기능)
    public float MoveSpeed//Set & Get도 가능한 Property로 변경
    {
        set => moveSpeed = Mathf.Max(0, value);//이동속도가 음수가 되지 않도록 설정
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
//Desc: 이동 가능한 모든 오브젝트에 부착
//Functions: MoveTo(): 외부에서 호출해 이동 방향 설정
