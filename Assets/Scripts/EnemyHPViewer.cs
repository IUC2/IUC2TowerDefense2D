using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyHP enemyHP;
    private Slider hpSlider;

    public void Setup(EnemyHP enemyHP)
    {
        this.enemyHP = enemyHP;
        hpSlider = GetComponent<Slider>();
    }

    void Update()
    {
        hpSlider.value = enemyHP.CurrentHP / enemyHP.MaxHP;//������Ƽ�� ������ Get & Set�Լ��� ���� ����
    }
}
