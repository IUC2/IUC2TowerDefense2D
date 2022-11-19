using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{ 
    //[SerializeField]
    //private GameObject      enemyPrefab;            //�� ������

    [SerializeField]
    private GameObject      enemyHPSliderPrefab;    //�� ü���� ��Ÿ���� SliderUI ������
    [SerializeField]
    private Transform       canvasTransform;        //UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]
    //private float           spawnTime;              //�� ���� �ֱ�
    [SerializeField]
    private Transform[]     wayPoints;              //���� ���������� �̵� ���
    private Wave currentWave;                       //���� ���̺� ����. ���̺� ����ü�� �� �����հ� �����ֱ⿡ ���� ������ ������ �� Ŭ������ ������ �� �����ֱ� ���� ���� ����

    //enemySpawner ��ũ��Ʈ���� Ÿ������ �ʿ� �ִ� �� ������ �����ϱ� ���� ���� ������ ��, �� ������ list�� ����
    //�� ������, �� ������ ����Ʈ���� ����
    //using System.Collections.Generic; �ڵ带 ���� import
    //Awake �Լ����� new�� ���� �޸� �Ҵ�

    private List<Enemy> enemyList;//���� �ʿ� �����ϴ� ��� ���� ����

    //���� ���� �� ������ Enemy���� �����ϱ� ������ Set ������Ƽ�� �ʿ�X
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        //�� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
        ////�� ���� �ڷ�ƾ �Լ� ȣ��
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        //�Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        //���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }

    //#Wave Ver SpawnEnemy()
    private IEnumerator SpawnEnemy()
    {
        //���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;

        //���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ�, �ڷ�ƾ ����
        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //���̺꿡 �����ϴ� ���� ������ ���� ������ ��, ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();//��� ������ ���� Enemy ������Ʈ

            //this�� �� �ڽ�(�ڽ��� EnemySpawner ����)
            enemy.SetUp(this, wayPoints);       //wayPoint ������ �Ű������� Setup() ȣ��
            EnemyList.Add(enemy);               //����Ʈ�� ��� ������ �� ���� ����
            SpawnEnemyHPSlider(clone);          //�� ü���� ��Ÿ���� Slider UI ���� �� ����

            //���� ���̺꿡�� ������ ���� ���� + 1
            spawnEnemyCount++;
            GameManager.gameManager.CurEnemyCount += 1;

            //�� ���̺긶�� SpawnTime�� �ٸ� �� �ֱ� ������ ���� ���̺�(currentWave)�� spawnTime ���
            yield return new WaitForSeconds(currentWave.spawnTime);//spawnTime �ð� ���� ���
            if (enemyList.Count >= 40)
            {
                GameObject.Find("SoundManager").GetComponent<SoundManager>().OnAudio("people");
            }
        }

    }
    
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if(type == EnemyDestroyType.kill)//���� �÷��̾��� �߻�ü�� ������� ��
        {
            GameManager.gameManager.PlayerGold += gold;
        }
        GameManager.gameManager.CurEnemyCount -= 1;
        //����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        //�� ������Ʈ ����
        Destroy(enemy.gameObject);
        
        if (enemyList.Count < 40)
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().OffAudio("people");
        }
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        //�� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        //Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        //TIP. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�.
        sliderClone.transform.SetParent(canvasTransform);
        //���� �������� �ٲ� ũ�⸦ �ٽ� (1,1,1)���� ����
        sliderClone.transform.localScale = Vector3.one;

        //Slider UI�� �i�ƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        //Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
