using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;//�ִ� ü��
    private float currentHP;//���� ü��
    private bool isDie = false;//���� ��� �����̸�, isDie�� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP ;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        maxHP = maxHP * Mathf.Sqrt(WaveSystem.waveSystem.CurrentWave + WaveSystem.waveSystem.offset);
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        //TIP. ���� ü���� damage��ŭ ������ ���� ��Ȳ�� ��, ���� Ÿ���� ������ ���ÿ� ������ OnDie()�� ������ �߻� ����

        if (isDie == true) return;//�� ������� ��, �ڵ� ���� X

        //���� ���� ü���� damage��ŭ ����
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //ü���� 0 ���� = �� ĳ���� ���
        if ( currentHP <= 0)
        {
            isDie = true;
            //�� ĳ���� ���
            enemy.OnDie(EnemyDestroyType.kill);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("die");
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("eat");
        //���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;

        //Hit �߻� ��, ���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;

        //0.1�� wait
        yield return new WaitForSeconds(0.1f);

        //���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }



}
