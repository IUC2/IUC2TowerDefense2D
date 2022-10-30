using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    //레벨과 상관없이 하나의 타워에 하나만 필요한 변수는 타워 프리팹과 같이 선언
    public GameObject towerPrefab;//타워 생성을 위한 프리팹
    public GameObject follorTowerPrefab;//임시 타워 프리팹
    public Weapon[] weapon;//레벨별 타워(무기) 정보
    [System.Serializable]
    public struct Weapon//레벨별로 다르게 설정되는 정보 포함
    {
        //Tip. 클래스 내부에 구조체를 만들면 클래스 외부에선 구조체 변수를 선언할 수 없다.
        //Tip. 권장하는 방법은 변수를 코드에서 조작하지 못하도록 모두 private로 선언하고, 모든 변수에 접근할 수 있는 프로퍼티를 제작
        public Sprite sprite;   //보여지는 타워 이미지(UI)
        public string name;
        public string type;
        public float damage;    //공격력
        public float slow;      //감속 퍼센트(0.2 = 20%)
        public float buff;      //공격력 증가율(0.2 = +20%)
        public float rate;      //공격속도
        public float range;     //공격 범위
        public int cost;        //필요골드(0레벨: 건설, 1~레벨: 업그레이드)
        public int sell;        //타워 판매 시 획득 골드
    }
}