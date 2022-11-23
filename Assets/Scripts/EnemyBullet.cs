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
        this.target = target;//Ÿ���� �������� Ÿ��
        this.damage = damage;//Ÿ���� �������� ���ݷ�
    }
    void Update()
    {
        if (target != null)//Ÿ���� �����ϸ�
        {
            //�߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else//Ÿ���� �������� ������
        {
            //�߻�ü ������Ʈ ����
            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //���� �ƴ� ���� �ε����� => �׳� ����
        if (collision.transform != target) return;  //���� target�� ���� �ƴϸ� => �׳� ����(���ָ� �Ѿ��� �ٸ� ���� ���� �� ����)

        //collision.GetComponent<Enemy>().OnDie();    //�� ��� �Լ� ȣ��
        collision.GetComponent<EnemyHP>().TakeDamage(damage);
        if (collision.GetComponentInChildren<ParticleSystem>() != null)
        {
            collision.GetComponentInChildren<ParticleSystem>().Play();
        }
        Destroy(gameObject);                        //bullet ����
    }
}

/*
 * File: EnemyBullet.cs
 * Desc: Ƽ���� �߻��ϴ� �⺻ �߻�ü�� ����
 * Functions: Update() - Ÿ���� �����ϸ�, Ÿ�� �������� �̵��ϰ�, Ÿ���� �������� ������, �߻�ü ����
 * OnTriggerEnter2D() - Ÿ������ ������ ���� �ε����� ��, �� �� ����
 */