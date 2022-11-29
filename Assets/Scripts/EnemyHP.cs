using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;//최대 체력
    private float currentHP;//현재 체력
    private bool isDie = false;//적이 사망 상태이면, isDie를 true로 설정
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
        //TIP. 적의 체력이 damage만큼 감소해 죽을 상황일 때, 여러 타워의 공격을 동시에 받으면 OnDie()가 여러번 발생 가능

        if (isDie == true) return;//적 사망상태 시, 코드 실행 X

        //현재 적의 체력을 damage만큼 감소
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //체력이 0 이하 = 적 캐릭터 사망
        if ( currentHP <= 0)
        {
            isDie = true;
            //적 캐릭터 사망
            enemy.OnDie(EnemyDestroyType.kill);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("die");
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("eat");
        //현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;

        //Hit 발생 시, 적의 투명도를 40%로 조정
        color.a = 0.4f;
        spriteRenderer.color = color;

        //0.1초 wait
        yield return new WaitForSeconds(0.1f);

        //적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }



}
