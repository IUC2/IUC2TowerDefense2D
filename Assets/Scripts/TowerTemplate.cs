using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    //������ ������� �ϳ��� Ÿ���� �ϳ��� �ʿ��� ������ Ÿ�� �����հ� ���� ����
    public GameObject towerPrefab;//Ÿ�� ������ ���� ������
    public GameObject follorTowerPrefab;//�ӽ� Ÿ�� ������
    public Weapon[] weapon;//������ Ÿ��(����) ����
    [System.Serializable]
    public struct Weapon//�������� �ٸ��� �����Ǵ� ���� ����
    {
        //Tip. Ŭ���� ���ο� ����ü�� ����� Ŭ���� �ܺο��� ����ü ������ ������ �� ����.
        //Tip. �����ϴ� ����� ������ �ڵ忡�� �������� ���ϵ��� ��� private�� �����ϰ�, ��� ������ ������ �� �ִ� ������Ƽ�� ����
        public Sprite sprite;   //�������� Ÿ�� �̹���(UI)
        public string name;
        public string type;
        public float damage;    //���ݷ�
        public float slow;      //���� �ۼ�Ʈ(0.2 = 20%)
        public float buff;      //���ݷ� ������(0.2 = +20%)
        public float rate;      //���ݼӵ�
        public float range;     //���� ����
        public int cost;        //�ʿ���(0����: �Ǽ�, 1~����: ���׷��̵�)
        public int sell;        //Ÿ�� �Ǹ� �� ȹ�� ���
    }
}