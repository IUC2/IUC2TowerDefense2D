using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Movement2D  movement2D;
    private Transform   target;
    private float       damage;

    public void Setup(Transform target, float damage)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;//타워가 설정해준 타겟
        this.damage = damage;//타워가 설정해준 공격력
    }
    void Update()
    {
        if (target != null)//타겟이 존재하면
        {
            //발사체를 target의 위치로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else//타겟이 존재하지 않으면
        {
            //발사체 오브젝트 삭제
            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //적이 아닌 대상과 부딪히면 => 그냥 종료
        if (collision.transform != target) return;  //현재 target인 적이 아니면 => 그냥 종료(없애면 총알이 다른 적도 맞을 수 있음)

        //collision.GetComponent<Enemy>().OnDie();    //적 사망 함수 호출
        collision.GetComponent<EnemyHP>().TakeDamage(damage);
        if (collision.GetComponentInChildren<ParticleSystem>() != null)
        {
            collision.GetComponentInChildren<ParticleSystem>().Play();
        }
        Destroy(gameObject);                        //bullet 삭제
    }
}

/*
 * File: EnemyBullet.cs
 * Desc: 티워가 발사하는 기본 발사체에 부착
 * Functions: Update() - 타겟이 존재하면, 타겟 방향으로 이동하고, 타겟이 존재하지 않으면, 발사체 삭제
 * OnTriggerEnter2D() - 타겟으로 설정된 적과 부딪혔을 때, 둘 다 삭제
 */