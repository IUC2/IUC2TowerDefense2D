using System.Collections;
using UnityEngine;

public enum WeaponType { Cannon, Laser, Slow, Buff}
public enum WeaponState { SearchTarget, TryAttackCannon, TryAttackLaser}
//public enum WeaponState { SearchTarget, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    //[Header(string)]: Inspector View에 표시되는 변수들을 용도별로 구분하기 위해 사용하는 어트리뷰트 string에 작성된 내용을 굵은 글씨로 표시
    [Header("Commons")]
    [SerializeField]
    public TowerTemplate   towerTemplate;//타워 정보(공격력, 공격속도 등)
    //[SerializeField]
    //private GameObject      enemyBulletPrefab01;//발사체 프리팹
    [SerializeField]
    private Transform       spawnPoint;//발사체 생성 위치
    [SerializeField]
    private WeaponType      weaponType;//무기 속성 설정

    //[SerializeField]
    //private float           attackRate = 0.5f;//공격 속도
    //[SerializeField]
    //private float           attackRange = 2.0f;//공격 범위
    //[SerializeField]
    //private int             attackDamage = 1;//공격력

    [Header("Cannon")]
    [SerializeField]
    private GameObject bulletPrefab;//발사체 프리팹

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;//레이저로 사용되는 선(LineRenderer)
    [SerializeField]
    private Transform hitEffect;//타격 효과
    [SerializeField]
    private LayerMask targetLayer;//광선에 부딪히는 레이어 설정

    private int             level = 0;//타워 레벨
    private WeaponState     weaponState = WeaponState.SearchTarget;//타워 무기의 상태
    private Transform       attackTarget = null;//공격 대상
    private TowerSpawner    towerSpawner;
    private EnemySpawner    enemySpawner;//게임에 존재하는 적 정보 획득용
    private SpriteRenderer  spriteRenderer;//타워 오브젝트 이미지 변경용
    
    private float           addedDamage;//버프에 의해 추가된 데미지
    private int             buffLevel;//버프를 받는지 여부 설정(0: 버프 X, 1 ~ 3: 받는 버프 레벨)

    public Tile ownerTile;//현재 타워가 배치되어 있는 타일

    //Property
    //public float AttackDamage => attackDamage;
    //public float AttackRate => attackRate;
    //public float AttackRange => attackRange;

    //TowerTemplate Property 생성
    public Sprite       TowerSprite => towerTemplate.weapon[level].sprite;
    public float        Damage      => towerTemplate.weapon[level].damage;
    public float        Rate        => towerTemplate.weapon[level].rate;
    public float        Range       => towerTemplate.weapon[level].range;
    public int          UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int          SellCost => towerTemplate.weapon[level].sell;
    public int          Level       => level + 1;
    public int          MaxLevel    => towerTemplate.weapon.Length;
    public float        Slow        => towerTemplate.weapon[level].slow;
    public float        Buff => towerTemplate.weapon[level].buff;
    public WeaponType   WeaponType  => weaponType;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    public void SetUp(TowerSpawner towerSpawner, EnemySpawner enemySpawner, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;
        this.ownerTile = ownerTile;

        ////최초 상태를 WeaponState.SearchTarget으로 설정
        //ChangeState(WeaponState.SearchTarget);

        //무기 속성이 캐논, 레이저일 때만 최초 상태를 Search로 변경하도록 함
        if(weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            //최초 상태를 WeaponState.SearchTarget으로 설정
            ChangeState(WeaponState.SearchTarget);
        }
    }

    public void ChangeState(WeaponState newState)
    {
        //이전에 재성중이던 상태 종료
        StopCoroutine(weaponState.ToString());
        //상태 변경
        weaponState = newState;
        //새로운 상태 재생
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        //원점으로부터의 거리가 수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용
        //각도 = arctan(y/x)
        //x, y 변위 값 구하기
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        //x, y 변위 값을 바탕으로 각도 구하기
        //각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위로 변경
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    //private IEnumerator SearchTarget()
    //{
    //    while(true)
    //    {
    //        //제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게(무한대) 설정
    //        float closestDistSqr = Mathf.Infinity;
    //        //EnemySpawner의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
    //        for(int i = 0; i < enemySpawner.EnemyList.Count; ++i)
    //        {
    //            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
    //            //현재 검사중인 적과의 거리가 공격 범위 내에 존재하고, 현재까지 검사한 적보다 거리가 가까우면
    //            //if(distance <= attackRange && distance <= closestDistSqr)
    //            if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
    //            {
    //                closestDistSqr = distance;
    //                attackTarget = enemySpawner.EnemyList[i].transform;
    //            }
    //        }
    //        if(attackTarget != null)
    //        {
    //            ChangeState(WeaponState.AttackToTarget);
    //        }
    //        yield return null; 
    //    }
    //}

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            //현재 타워에 가장 가까이있는 공격 대상(적) 탐색
            attackTarget = FindClosestAttackTarget();
            if (attackTarget != null)
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }
            yield return null;
        }
    }

    private Transform FindClosestAttackTarget()
    {
        //제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
        float closestDistSqr = Mathf.Infinity;
        //EnemySpawnerr의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
        for(int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //현재 검사중인 적과의 거리가 공격범위 내에 있고, 현잮자ㅣ 검샇나 적보다 거리가 가까우면
            if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }
        return attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        //target이 있는지 검사 (다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제 등)
        if(attackTarget == null)
        {
            return false;
        }
        //target이 공격 범위 안에 있는지 검사(공격 범위를 벗어나면 새로운 적 탐색)
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if(distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }

    //private IEnumerator AttackToTarget()
    //{
    //    while (true)
    //    {
    //        //1. target이 있는지 검사(다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제 등)
    //        if(attackTarget == null)
    //        {
    //            ChangeState(WeaponState.SearchTarget);
    //            break;
    //        }
    //        //2. target이 공격 범위 안에 있는지 검사(공격 범위를 벗어나면 새로운 적 탐색)
    //        float distance = Vector3.Distance(attackTarget.position, transform.position);
    //        //if (distance > attackRange)
    //        if(distance > towerTemplate.weapon[level].range)
    //        {
    //            attackTarget = null;
    //            ChangeState(WeaponState.SearchTarget);
    //            break;
    //        }

    //        //3. attackRate 시간만큼 대기
    //        //yield return new WaitForSeconds(attackRate);
    //        yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            
    //        //4. 공격(발사체 생성)
    //        SpawnEnemyBullet();
    //    }
    //}

    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            //1. target을 공격하는게 가능한지 검사
            if(IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRate 시간만큼 대기
            //yield return new WaitForSeconds(attackRate);
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. 공격(발사체 생성)
            SpawnEnemyBullet();
        }
    }

    private IEnumerator TryAttackLaser()
    {
        //레이저, 레이저 타격효과 활성화
        EnableLaser();

        while (true)
        {
            //1. target을 공격하는게 가능한지 검사
            if (IsPossibleToAttackTarget() == false)
            {
                //레이저, 레이저 타격효과 비활성화
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            //레이저 공격
            SpawnLaser();

            yield return null;
        }
    }

    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    public void OnBuffAroundTower()
    {
        //현재 맵에 배치된 "Tower" 태그를 가진 모든 오브젝트 탐색
        GameObject[] towers = GameObject.FindGameObjectsWithTag("PlacedTower");

        for(int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            //이미 버프를 받고 있고, 현재 버프 타워의 레벨보다 높은 버프이면 패스
            if(weapon.BuffLevel > level)
            {
                continue;
            }
            //현재 버프 타워와 다른 타워의 거리를 검사해 범위 안에 타워가 있으면
            if(Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                //공격이 가능한 캐논, 레이저 타워이면
                if(weapon.WeaponType == WeaponType.Cannon || weapon.WeaponType == WeaponType.Laser)
                {
                    //버프에 의해 공격력 증가
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    //타워가 받고 있는 버프 레벨 설정
                    weapon.BuffLevel = Level;
                }
            }
        }
    }

    private void SpawnEnemyBullet()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("attack");
        GameObject clone = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        //생성된 발사체에게 공격 대상(attackTarget)의 위치, 움직임 정보 제공
        //clone.GetComponent<EnemyBullet>().Setup(attackTarget, attackDamage);

        //공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
        float damage = towerTemplate.weapon[level].damage + AddedDamage;

        clone.GetComponent<EnemyBullet>().Setup(attackTarget, damage);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        //같은 방향으로 여러 개의 광선을 쏴서 그 중 현재 attackTarget과 동일한 오브젝트 검출
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //선의 시작지점
                lineRenderer.SetPosition(0, spawnPoint.position);
                //선의 목표지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //타격 효과 위치 설정(만약 적의 중앙에 타격을 주고 싶은 경우 attactTarget.position으로 설정)
                hitEffect.position = hit[i].point;
                ////적 체력 감소(Coroutine함수에서 매 초 실행되므로, 1초에 damage만큼 감소)
                //attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
                //공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHP>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }

    public bool Upgrade()
    {
        //타워 업그레이드에 필요한 골드가 충분한지 검사
        if(GameManager.gameManager.PlayerGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }

        //타워 레벨 증가
        level++;
        //타워 외형 변경
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //골드 차감
        GameManager.gameManager.PlayerGold -= towerTemplate.weapon[level].cost;

        //무기 속성이 레이저라면
        if(weaponType == WeaponType.Laser)
        {
            //레벨에 따라 레이저의 굵기 설정
            lineRenderer.startWidth = 0.02f + level * 0.05f;
            lineRenderer.endWidth = 0.02f;
        }

        //공격타워 or 버프타워가 업그레이드될 경우 모든 타워의 버프 타워 효과 갱신
        //현재 타워가 버프 타워인 경우, 현재 타워가 공격 타워인 경우 모두 적용
        towerSpawner.OnBuffAllBuffTowers();

        return true;
    }

    public void Sell()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("sell");
        //골드 증가
        GameManager.gameManager.PlayerGold += towerTemplate.weapon[level].sell;
        //현재 타일에 다시 타워 건설이 가능해지도록 설정
        ownerTile.IsBuildTower = false;
        //타워 파괴
        Destroy(gameObject);
    }
}

/*
 * File: TowerWeapon.cs
 * Desc: 적을 공격하는 타워 무기
 * Functions
 * : ChangeState() - 코루틴을 이용한 FSM에서 상태 변경 함수
 * ...
 * ...
 * ...
 */
