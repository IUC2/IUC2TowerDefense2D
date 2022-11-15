using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { kill, Arrive }

public class Enemy : MonoBehaviour
{
    private int             wayPointCount;      //이동 경로 개수
    private Transform[]     wayPoints;          //이동 경로 정보
    private int             currentIndex = 0;   //현재 목표지점 인덱스
    private Movement2D      movement2D;         //오브젝트 이동 제어
    private EnemySpawner    enemySpawner;       //적의 삭제를 본인이 하지 않고, EnemySpawner에 알려 삭제 수행
    GameObject centerPoint;
    [SerializeField]
    private int             gold = 10;          //적 사망시 획득 가능한 골드

    private void Awake()
    {
        centerPoint = GameObject.Find("CenterPoint");
    }

    public void SetUp(EnemySpawner enemySpawner, Transform[] wayPoints)  
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //적 이동경로 WayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //적의 위치를 첫번재 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        //적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()//적 이동 목표지점 도착시 다음 목표지점을 설정하는 함수역할을 수행한다.
    {
        //다음 이동 방향 설정 함수
        NextMoveTo();

        while (true)
        {
            //적 오브젝트 회전
            Vector2 direction = new Vector2(transform.position.x - centerPoint.transform.position.x, transform.position.y - centerPoint.transform.position.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angelAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angelAxis, 10f * Time.deltaTime);
            transform.rotation = rotation;
            //적의 현재위치와 목표위치의 거리가 0.02 * movement2D.MoveSpeed보다 작을 때 if 조건문 실행
            //TIP. movement2D.MoveSpeed를 곱해주는 이유는 속도가 빠르면 한 프레임에 0.02보다 크게 움직이기 때문에
            //if 조건무네 걸리지 않고 경로를 탈주하는 오브젝트가 발생 가능하다.
            if(Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.04f * movement2D.MoveSpeed){
                NextMoveTo();
            }
            yield return null;
        }
    }
    private void NextMoveTo()
    {
        //적의 위치를 정확하게 목표위치로 설정
        transform.position = wayPoints[currentIndex].position;
        //이동 방향 설정 => 다음 목표지점(wayPoints)
        if (currentIndex == wayPointCount - 1)
        {
            currentIndex = 0;
            Vector3 direction = (wayPoints[0].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
            


        ////아직 이동할 wayPoints가 남아있다면
        //if (currentIndex < wayPointCount - 1)
        //{

        //}
        //else//현재 위치가 마지막 wayPoint라면
        //{
        //    currentIndex = 0;
        //    transform.position = wayPoints[0].position;
        //    Vector3 direction = (wayPoints[0].position - transform.position).normalized;
        //    movement2D.MoveTo(direction);
        //    //원래 이것만 있었음
        //    ////목표지점에 도달해서 사망할 때는 돈을 주지 않도록 gold = 0으로 설정
        //    //gold = 0;

        //    ////Destroy(gameObject);//적 오브젝트 삭제
        //    //OnDie(EnemyDestroyType.Arrive);
        //}
    }

    public void OnDie(EnemyDestroyType type)
    {
        //EnemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy()를 직접 하지 않고,
        //EnemySpawner에게 본인이 삭제될 때 필요한 처리를 하도록 DestroyEnemy() 함수 호출
        enemySpawner.DestroyEnemy(type, this, gold);//this는 나 자신(Enmey Component)를 의미
    }
}
