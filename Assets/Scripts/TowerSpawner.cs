using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;    //Ÿ�� ����(���ݷ�, ���� �ӵ� ��)
    //[SerializeField]
    //private GameObject towerPrefab;
    //[SerializeField]
    //private int towerBuildGold = 50;        //Ÿ�� �Ǽ��� ���Ǵ� ���
    [SerializeField]
    private EnemySpawner enemySpawner;      //���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ���� �ӽ� �����ϴ� ����
    [SerializeField]
    private PlayerGold playerGold;          //Ÿ�� �Ǽ� �� ��� ���Ҹ� ����..
    [SerializeField]
    private SystemTextViewer systemTextViewer;//�� ����, �Ǽ� �Ұ��� ���� �ý��� �޽����� ���
    private bool isOnTowerButton = false;//Ÿ�� �Ǽ� ��ư�� �������� üũ
    private GameObject followTowerClone = null;//�ӽ� Ÿ�� ��� �Ϸ� �� ������ ���� �����ϴ� ����
    private int towerType;//Ÿ�� �Ӽ�

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        //��ư�� �ߺ��ؼ� ������ ���� �����ϱ� ���� �ʿ�
        if ( isOnTowerButton == true)
        {
            return;
        }

        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //Ÿ���� �Ǽ��� ��ŭ ���� ������, Ÿ�� �Ǽ� X
        if (towerTemplate[towerType].weapon[0].cost > GameManager.gameManager.CurrentGold)
        {
            //��尡 ������ Ÿ�� �Ǽ��� �Ұ����ϴ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        //Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        isOnTowerButton = true;
        //���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        followTowerClone = Instantiate(towerTemplate[towerType].follorTowerPrefab);
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        if(isOnTowerButton == false)
        {
            return;
        }

        ////Ÿ�� �Ǽ� ���� Ȯ��
        ////1. Ÿ���� �Ǽ��� ���� ���ٸ� Ÿ�� �Ǽ� X
        ////if(towerBuildGold > playerGold.CurrentGold)
        //if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        //{
        //    //��尡 �����ؼ� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
        //    systemTextViewer.PrintText(SystemType.Money);
        //    return;
        //}

        Tile tile = tileTransform.GetComponent<Tile>();
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� �ִٸ� Ÿ�� �Ǽ� X
        if(tile.IsBuildTower == true)
        {
            //���� ��ġ�� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }
        //�ٽ� Ÿ�� �Ǽ� ��ư�� ���� Ÿ���� �Ǽ��ϵ��� ���� ����
        isOnTowerButton = false;
        //Ÿ���� �Ǽ��ϱ� ������ �ش� Ÿ�Ͽ� ǥ��
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        //playerGold.CurrentGold -= towerBuildGold;
        GameManager.gameManager.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ��� z = -1 ��ġ�� ��ġ) => Ÿ���� Ÿ�Ͽ� ��ġ�� ��� Ÿ�Ϻ��� Ÿ���� �켱 ������ �� �ֵ��� ��
        Vector3 position = tileTransform.position + Vector3.back;
        //GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        clone.GetComponent<TowerWeapon>().SetUp(this, enemySpawner, playerGold, tile);

        //���� ��ġ�Ǵ� Ÿ���� ���� Ÿ�� �ֺ��� ��ġ�� ���
        //���� ȿ���� ���� �� �յ��� ��� ���� Ÿ���� ���� ȿ�� ����
        OnBuffAllBuffTowers();

        //Ÿ���� ��ġ�߱� ������ ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        Destroy(followTowerClone);
        //Ÿ�� ���� ��ġ�� ���� Ÿ�� �Ǽ��� ����� �� �ִ� �Լ��� ������ ����
        StopCoroutine("OnTowerCancelSystem");
    }
    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            //ESCŰ �Ǵ� ���콺 ������ ��ư�� ������ �� Ÿ�� �Ǽ� ���
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                //���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if(weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
/*
 * File: TowerSpawner.cs
 * Desc: Ÿ�� ���� ����
 * 
 * Functions
 * SpawnTower(): �Ű����� ��ġ�� Ÿ�� ����
 */
