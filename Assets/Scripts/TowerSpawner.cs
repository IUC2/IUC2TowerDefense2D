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
    private int towerType;//Ÿ�� �Ӽ�
    public GameObject followTowerClone = null;//�ӽ� Ÿ�� ��� �Ϸ� �� ������ ���� �����ϴ� ����


    public bool ReadyToSpawnTower(int type)
    {
        towerType = type;
        //��ư�� �ߺ��ؼ� ������ ���� �����ϱ� ���� �ʿ�
        if ( isOnTowerButton == true)
        {
            return false;
        }
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //Ÿ���� �Ǽ��� ��ŭ ���� ������, Ÿ�� �Ǽ� X
        if (towerTemplate[towerType].weapon[0].cost > GameManager.gameManager.CurrentGold)
        {
            //��尡 ������ Ÿ�� �Ǽ��� �Ұ����ϴ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return false;
        }
        //Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        isOnTowerButton = true;
        //���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        followTowerClone = Instantiate(towerTemplate[towerType].follorTowerPrefab);
        followTowerClone.gameObject.SetActive(false);
        //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");

        return true;
    }

    public GameObject SpawnTower(Transform tileTransform)
    {
        if(isOnTowerButton == false)
        {
            return null;
        }

        Tile tile = tileTransform.GetComponent<Tile>();
        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� �ִٸ� Ÿ�� �Ǽ� X
        if(tile.IsBuildTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);//���� ��ġ�� Ÿ�� �Ǽ��� �Ұ��� ���
            //or ���׷��̵� ����
            return null;
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
        clone.tag = "PlacedTower";
        clone.layer = LayerMask.NameToLayer("PlacedTower");
        //���� ��ġ�Ǵ� Ÿ���� ���� Ÿ�� �ֺ��� ��ġ�� ���
        //���� ȿ���� ���� �� �յ��� ��� ���� Ÿ���� ���� ȿ�� ����
        OnBuffAllBuffTowers();

        //Ÿ���� ��ġ�߱� ������ ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        DestroyFollowTowerClone();
        //Ÿ�� ���� ��ġ�� ���� Ÿ�� �Ǽ��� ����� �� �ִ� �Լ��� ������ ����
        StopCoroutine("OnTowerCancelSystem");

        return clone;
    }

    public void SpawnTower2(Transform tileTransform, GameObject clickedTower)
    {
        //�ٽ� Ÿ�� �Ǽ� ��ư�� ���� Ÿ���� �Ǽ��ϵ��� ���� ����
        isOnTowerButton = false;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ��� z = -1 ��ġ�� ��ġ) => Ÿ���� Ÿ�Ͽ� ��ġ�� ��� Ÿ�Ϻ��� Ÿ���� �켱 ������ �� �ֵ��� ��
        Vector3 position = tileTransform.position + Vector3.back;
        clickedTower.GetComponent<TowerWeapon>().ownerTile.IsBuildTower = false;
        clickedTower.transform.position = position;
        clickedTower.tag = "PlacedTower";
        clickedTower.layer = LayerMask.NameToLayer("PlacedTower");
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
                DestroyFollowTowerClone();
                break;
            }
            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("PlacedTower");

        for(int i = 0; i < towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if (weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }

    public void DestroyFollowTowerClone()
    {
        Destroy(followTowerClone);
        isOnTowerButton = false;
    }

    public void SetDragPosition(Camera camera, GameObject gO)
    {
        gO.SetActive(true);
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        gO.transform.position = new Vector3(camera.ScreenToWorldPoint(position).x, camera.ScreenToWorldPoint(position).y, 0);
    }
}
/*
 * File: TowerSpawner.cs
 * Desc: Ÿ�� ���� ����
 * 
 * Functions
 * SpawnTower(): �Ű����� ��ġ�� Ÿ�� ����
 */
