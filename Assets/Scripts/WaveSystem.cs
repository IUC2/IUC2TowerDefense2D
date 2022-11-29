using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[]          waves;              //���� �������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner    enemySpawner;
    private int     currentWaveIndex = 0;      //���� ���̺� �ε���
    public GameObject bell;
    public int offset = 0;

    [SerializeField]
    public static WaveSystem waveSystem = null;
    private void Awake()
    {
        if (waveSystem == null)
        {
            waveSystem = this;
        }
        else if (waveSystem != this)
        {
            Destroy(gameObject);
        }
    }

    //���̺� ���� ����� ���� Get ������Ƽ(���� ���̺�, �� ���̺�)
    public int CurrentWave {
        set => currentWaveIndex = Mathf.Max(0, value);//������ 0�̱� ������ 1
        get => currentWaveIndex;
    }

    public int MaxWave => waves.Length;

    public void StartWave()
    {
        bell.GetComponent<Animator>().SetTrigger("Bell");
        //enemySpawner.StartWave(waves[Random.Range(0, MaxWave)]);
        enemySpawner.StartWave(waves[currentWaveIndex]);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("WaveStart");
        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
        }
        else
        {
            currentWaveIndex = 0;
            offset += 20;
        }
    }

    private void Update()
    {
        if (GameManager.gameManager.spawntime <= 0f)
        {
            StartWave();
            GameManager.gameManager.spawntime = 30;
        }
    }
}

[System.Serializable]//����ü �����͸� ����ȭ�ϴ� ���: �޸𸮻� �����ϴ� ������Ʈ ������ stirng or byte ������ ���·� �����ϴ� ��(����̺� ����, ��Ʈ��ũ�� ���� ������ ���� ����)
//Tip. ����ȭ�� ����ϸ� Inspector View���� Ŭ���� ������ ���� �������� ������ �� ����
public struct Wave
{
    public float        spawnTime;              //���� ���̺� �� ���� �ֱ�
    public int          maxEnemyCount;          //���� ���̺� �� ���� ����
    public GameObject[] enemyPrefabs;           //���� ���̺� �� ���� ����
}
