using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{ 
    //[SerializeField]
    //private GameObject      enemyPrefab;            //적 프리팹

    [SerializeField]
    private GameObject      enemyHPSliderPrefab;    //적 체력을 나타내는 SliderUI 프리팹
    [SerializeField]
    private Transform       canvasTransform;        //UI를 표현하는 Canvas 오브젝트의 Transform
    //[SerializeField]
    //private float           spawnTime;              //적 생성 주기
    [SerializeField]
    private Transform[]     wayPoints;              //현재 스테이지의 이동 경로
    private Wave currentWave;                       //현재 웨이브 정보. 웨이브 구조체에 적 프리팹과 생성주기에 대한 정보가 존재해 본 클래스의 프리팹 및 생성주기 변수 삭제 가능

    //enemySpawner 스크립트에서 타워에게 맵에 있는 적 정보를 제공하기 위해 적이 생성될 때, 적 정보를 list에 저장
    //적 삭제시, 적 정보를 리스트에서 삭제
    //using System.Collections.Generic; 코드를 통해 import
    //Awake 함수에서 new를 통해 메모리 할당

    private List<Enemy> enemyList;//현재 맵에 존재하는 모든 적의 정보

    //적의 생성 및 삭제는 Enemy에서 수행하기 때문에 Set 프로퍼티는 필요X
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        //적 리스트 메모리 할당
        enemyList = new List<Enemy>();
        ////적 생성 코루틴 함수 호출
        //StartCoroutine("SpawnEnemy");
    }

    public void StartWave(Wave wave)
    {
        //매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        //현재 웨이브 시작
        StartCoroutine("SpawnEnemy");
    }

    //#Wave Ver SpawnEnemy()
    private IEnumerator SpawnEnemy()
    {
        //현재 웨이브에서 생성한 적 숫자
        int spawnEnemyCount = 0;

        //현재 웨이브에서 생성되어야 하는 적의 숫자만큼 적을 생성하고, 코루틴 종료
        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            //웨이브에 등장하는 적의 종류가 여러 종류일 때, 임의의 적이 등장하도록 설정하고, 적 오브젝트 설정
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();//방금 생성된 적의 Enemy 컴포넌트

            //this는 나 자신(자신의 EnemySpawner 정보)
            enemy.SetUp(this, wayPoints);       //wayPoint 정보를 매개변수로 Setup() 호출
            EnemyList.Add(enemy);               //리스트에 방금 생성된 적 정보 저장
            SpawnEnemyHPSlider(clone);          //적 체력을 나타내는 Slider UI 생성 및 설정

            //현재 웨이브에서 생성된 적의 숫자 + 1
            spawnEnemyCount++;
            GameManager.gameManager.CurEnemyCount += 1;

            //각 웨이브마다 SpawnTime이 다를 수 있기 때문에 현재 웨이브(currentWave)의 spawnTime 사용
            yield return new WaitForSeconds(currentWave.spawnTime);//spawnTime 시간 동안 대기
            if (enemyList.Count >= 40)
            {
                GameObject.Find("SoundManager").GetComponent<SoundManager>().OnAudio("people");
            }
        }

    }
    
    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold)
    {
        if(type == EnemyDestroyType.kill)//적이 플레이어의 발사체에 사망했을 때
        {
            GameManager.gameManager.PlayerGold += gold;
        }
        GameManager.gameManager.CurEnemyCount -= 1;
        //리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        //적 오브젝트 삭제
        Destroy(enemy.gameObject);
        
        if (enemyList.Count < 40)
        {
            GameObject.Find("SoundManager").GetComponent<SoundManager>().OffAudio("people");
        }
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        //적 체력을 나타내는 Slider UI 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        //Slider UI 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        //TIP. UI는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다.
        sliderClone.transform.SetParent(canvasTransform);
        //계층 설정으로 바뀐 크기를 다시 (1,1,1)으로 설정
        sliderClone.transform.localScale = Vector3.one;

        //Slider UI가 쫒아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        //Slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
