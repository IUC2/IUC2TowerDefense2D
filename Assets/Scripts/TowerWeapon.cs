using System.Collections;
using UnityEngine;

public enum WeaponType { Cannon, Laser, Slow, Buff}
public enum WeaponState { SearchTarget, TryAttackCannon, TryAttackLaser}
//public enum WeaponState { SearchTarget, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    //[Header(string)]: Inspector View�� ǥ�õǴ� �������� �뵵���� �����ϱ� ���� ����ϴ� ��Ʈ����Ʈ string�� �ۼ��� ������ ���� �۾��� ǥ��
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate   towerTemplate;//Ÿ�� ����(���ݷ�, ���ݼӵ� ��)
    //[SerializeField]
    //private GameObject      enemyBulletPrefab01;//�߻�ü ������
    [SerializeField]
    private Transform       spawnPoint;//�߻�ü ���� ��ġ
    [SerializeField]
    private WeaponType      weaponType;//���� �Ӽ� ����

    //[SerializeField]
    //private float           attackRate = 0.5f;//���� �ӵ�
    //[SerializeField]
    //private float           attackRange = 2.0f;//���� ����
    //[SerializeField]
    //private int             attackDamage = 1;//���ݷ�

    [Header("Cannon")]
    [SerializeField]
    private GameObject bulletPrefab;//�߻�ü ������

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;//�������� ���Ǵ� ��(LineRenderer)
    [SerializeField]
    private Transform hitEffect;//Ÿ�� ȿ��
    [SerializeField]
    private LayerMask targetLayer;//������ �ε����� ���̾� ����

    private int             level = 0;//Ÿ�� ����
    private WeaponState     weaponState = WeaponState.SearchTarget;//Ÿ�� ������ ����
    private Transform       attackTarget = null;//���� ���
    private TowerSpawner    towerSpawner;
    private EnemySpawner    enemySpawner;//���ӿ� �����ϴ� �� ���� ȹ���
    private SpriteRenderer  spriteRenderer;//Ÿ�� ������Ʈ �̹��� �����
    
    private float           addedDamage;//������ ���� �߰��� ������
    private int             buffLevel;//������ �޴��� ���� ����(0: ���� X, 1 ~ 3: �޴� ���� ����)

    public Tile ownerTile;//���� Ÿ���� ��ġ�Ǿ� �ִ� Ÿ��

    //Property
    //public float AttackDamage => attackDamage;
    //public float AttackRate => attackRate;
    //public float AttackRange => attackRange;

    //TowerTemplate Property ����
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

        ////���� ���¸� WeaponState.SearchTarget���� ����
        //ChangeState(WeaponState.SearchTarget);

        //���� �Ӽ��� ĳ��, �������� ���� ���� ���¸� Search�� �����ϵ��� ��
        if(weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            //���� ���¸� WeaponState.SearchTarget���� ����
            ChangeState(WeaponState.SearchTarget);
        }
    }

    public void ChangeState(WeaponState newState)
    {
        //������ �缺���̴� ���� ����
        StopCoroutine(weaponState.ToString());
        //���� ����
        weaponState = newState;
        //���ο� ���� ���
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
        //�������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        //���� = arctan(y/x)
        //x, y ���� �� ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        //x, y ���� ���� �������� ���� ���ϱ�
        //������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    //private IEnumerator SearchTarget()
    //{
    //    while(true)
    //    {
    //        //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ��(���Ѵ�) ����
    //        float closestDistSqr = Mathf.Infinity;
    //        //EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
    //        for(int i = 0; i < enemySpawner.EnemyList.Count; ++i)
    //        {
    //            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
    //            //���� �˻����� ������ �Ÿ��� ���� ���� ���� �����ϰ�, ������� �˻��� ������ �Ÿ��� ������
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
            //���� Ÿ���� ���� �������ִ� ���� ���(��) Ž��
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
        //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;
        //EnemySpawnerr�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
        for(int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, �����ڤ� �˘��� ������ �Ÿ��� ������
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
        //target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
        if(attackTarget == null)
        {
            return false;
        }
        //target�� ���� ���� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
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
    //        //1. target�� �ִ��� �˻�(�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
    //        if(attackTarget == null)
    //        {
    //            ChangeState(WeaponState.SearchTarget);
    //            break;
    //        }
    //        //2. target�� ���� ���� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
    //        float distance = Vector3.Distance(attackTarget.position, transform.position);
    //        //if (distance > attackRange)
    //        if(distance > towerTemplate.weapon[level].range)
    //        {
    //            attackTarget = null;
    //            ChangeState(WeaponState.SearchTarget);
    //            break;
    //        }

    //        //3. attackRate �ð���ŭ ���
    //        //yield return new WaitForSeconds(attackRate);
    //        yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            
    //        //4. ����(�߻�ü ����)
    //        SpawnEnemyBullet();
    //    }
    //}

    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            //1. target�� �����ϴ°� �������� �˻�
            if(IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRate �ð���ŭ ���
            //yield return new WaitForSeconds(attackRate);
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. ����(�߻�ü ����)
            SpawnEnemyBullet();
        }
    }

    private IEnumerator TryAttackLaser()
    {
        //������, ������ Ÿ��ȿ�� Ȱ��ȭ
        EnableLaser();

        while (true)
        {
            //1. target�� �����ϴ°� �������� �˻�
            if (IsPossibleToAttackTarget() == false)
            {
                //������, ������ Ÿ��ȿ�� ��Ȱ��ȭ
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            //������ ����
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
        //���� �ʿ� ��ġ�� "Tower" �±׸� ���� ��� ������Ʈ Ž��
        GameObject[] towers = GameObject.FindGameObjectsWithTag("PlacedTower");

        for(int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            //�̹� ������ �ް� �ְ�, ���� ���� Ÿ���� �������� ���� �����̸� �н�
            if(weapon.BuffLevel > level)
            {
                continue;
            }
            //���� ���� Ÿ���� �ٸ� Ÿ���� �Ÿ��� �˻��� ���� �ȿ� Ÿ���� ������
            if(Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                //������ ������ ĳ��, ������ Ÿ���̸�
                if(weapon.WeaponType == WeaponType.Cannon || weapon.WeaponType == WeaponType.Laser)
                {
                    //������ ���� ���ݷ� ����
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    //Ÿ���� �ް� �ִ� ���� ���� ����
                    weapon.BuffLevel = Level;
                }
            }
        }
    }

    private void SpawnEnemyBullet()
    {
        GameObject clone = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        //������ �߻�ü���� ���� ���(attackTarget)�� ��ġ, ������ ���� ����
        //clone.GetComponent<EnemyBullet>().Setup(attackTarget, attackDamage);

        //���ݷ� = Ÿ�� �⺻ ���ݷ� + ������ ���� �߰��� ���ݷ�
        float damage = towerTemplate.weapon[level].damage + AddedDamage;

        clone.GetComponent<EnemyBullet>().Setup(attackTarget, damage);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        //���� �������� ���� ���� ������ ���� �� �� ���� attackTarget�� ������ ������Ʈ ����
        for (int i = 0; i < hit.Length; ++i)
        {
            if (hit[i].transform == attackTarget)
            {
                //���� ��������
                lineRenderer.SetPosition(0, spawnPoint.position);
                //���� ��ǥ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //Ÿ�� ȿ�� ��ġ ����(���� ���� �߾ӿ� Ÿ���� �ְ� ���� ��� attactTarget.position���� ����)
                hitEffect.position = hit[i].point;
                ////�� ü�� ����(Coroutine�Լ����� �� �� ����ǹǷ�, 1�ʿ� damage��ŭ ����)
                //attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
                //���ݷ� = Ÿ�� �⺻ ���ݷ� + ������ ���� �߰��� ���ݷ�
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHP>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }

    public bool Upgrade()
    {
        //Ÿ�� ���׷��̵忡 �ʿ��� ��尡 ������� �˻�
        if(GameManager.gameManager.PlayerGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }

        //Ÿ�� ���� ����
        level++;
        //Ÿ�� ���� ����
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //��� ����
        GameManager.gameManager.PlayerGold -= towerTemplate.weapon[level].cost;

        //���� �Ӽ��� ���������
        if(weaponType == WeaponType.Laser)
        {
            //������ ���� �������� ���� ����
            lineRenderer.startWidth = 0.02f + level * 0.05f;
            lineRenderer.endWidth = 0.02f;
        }

        //����Ÿ�� or ����Ÿ���� ���׷��̵�� ��� ��� Ÿ���� ���� Ÿ�� ȿ�� ����
        //���� Ÿ���� ���� Ÿ���� ���, ���� Ÿ���� ���� Ÿ���� ��� ��� ����
        towerSpawner.OnBuffAllBuffTowers();

        return true;
    }

    public void Sell()
    {
        //��� ����
        GameManager.gameManager.PlayerGold += towerTemplate.weapon[level].sell;
        //���� Ÿ�Ͽ� �ٽ� Ÿ�� �Ǽ��� ������������ ����
        ownerTile.IsBuildTower = false;
        //Ÿ�� �ı�
        Destroy(gameObject);
    }
}

/*
 * File: TowerWeapon.cs
 * Desc: ���� �����ϴ� Ÿ�� ����
 * Functions
 * : ChangeState() - �ڷ�ƾ�� �̿��� FSM���� ���� ���� �Լ�
 * ...
 * ...
 * ...
 */