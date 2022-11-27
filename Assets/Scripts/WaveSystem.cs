using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[]          waves;              //���� �������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner    enemySpawner;
    private int     currentWaveIndex = -1;      //���� ���̺� �ε���
    public GameObject bell;

    //���̺� ���� ����� ���� Get ������Ƽ(���� ���̺�, �� ���̺�)
    public int CurrentWave => currentWaveIndex + 1;//������ 0�̱� ������ 1
    public int MaxWave => waves.Length;

    public void StartWave()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("WaveStart");
        enemySpawner.StartWave(waves[Random.Range(0, MaxWave)]);
        bell.GetComponent<Animator>().SetTrigger("Bell");
    }

    private void Update()
    {
        if (GameManager.gameManager.spawntime < 0.1f)
        {
            currentWaveIndex++;
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
