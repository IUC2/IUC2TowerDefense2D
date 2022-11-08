using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { kill, Arrive }

public class Enemy : MonoBehaviour
{
    private int             wayPointCount;      //�̵� ��� ����
    private Transform[]     wayPoints;          //�̵� ��� ����
    private int             currentIndex = 0;   //���� ��ǥ���� �ε���
    private Movement2D      movement2D;         //������Ʈ �̵� ����
    private EnemySpawner    enemySpawner;       //���� ������ ������ ���� �ʰ�, EnemySpawner�� �˷� ���� ����

    [SerializeField]
    private int             gold = 10;          //�� ����� ȹ�� ������ ���

    public void SetUp(EnemySpawner enemySpawner, Transform[] wayPoints)  
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //�� �̵���� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //���� ��ġ�� ù���� wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        //�� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()//�� �̵� ��ǥ���� ������ ���� ��ǥ������ �����ϴ� �Լ������� �����Ѵ�.
    {
        //���� �̵� ���� ���� �Լ�
        NextMoveTo();

        while (true)
        {
            //�� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);
            //���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 * movement2D.MoveSpeed���� ���� �� if ���ǹ� ����
            //TIP. movement2D.MoveSpeed�� �����ִ� ������ �ӵ��� ������ �� �����ӿ� 0.02���� ũ�� �����̱� ������
            //if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻� �����ϴ�.
            if(Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed){
                NextMoveTo();
            }
            yield return null;
        }
    }
    private void NextMoveTo()
    {
        //���� ��ġ�� ��Ȯ�ϰ� ��ǥ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;
        //�̵� ���� ���� => ���� ��ǥ����(wayPoints)
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
            


        ////���� �̵��� wayPoints�� �����ִٸ�
        //if (currentIndex < wayPointCount - 1)
        //{

        //}
        //else//���� ��ġ�� ������ wayPoint���
        //{
        //    currentIndex = 0;
        //    transform.position = wayPoints[0].position;
        //    Vector3 direction = (wayPoints[0].position - transform.position).normalized;
        //    movement2D.MoveTo(direction);
        //    //���� �̰͸� �־���
        //    ////��ǥ������ �����ؼ� ����� ���� ���� ���� �ʵ��� gold = 0���� ����
        //    //gold = 0;

        //    ////Destroy(gameObject);//�� ������Ʈ ����
        //    //OnDie(EnemyDestroyType.Arrive);
        //}
    }

    public void OnDie(EnemyDestroyType type)
    {
        //EnemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy()�� ���� ���� �ʰ�,
        //EnemySpawner���� ������ ������ �� �ʿ��� ó���� �ϵ��� DestroyEnemy() �Լ� ȣ��
        enemySpawner.DestroyEnemy(type, this, gold);//this�� �� �ڽ�(Enmey Component)�� �ǹ�
    }
}