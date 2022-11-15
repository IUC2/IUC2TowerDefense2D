using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textTime;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;//Text - TextMeshPro UI[�÷��̾��� ���]
    [SerializeField]
    private TextMeshProUGUI textWave;//Text - TextMeshPro UI[���� ���̺� / �� ���̺�]
    [SerializeField]
    private WaveSystem waveSystem;//���̺� ����
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;//Text - TextMeshPro UI[���� �� ���� / �� �� ����]
    [SerializeField]
    private EnemySpawner enemySpawner;//�� ����
    [SerializeField]
    private Image imageScreen;  //��üȭ���� ���� ������ �̹���
    [SerializeField]
    private int playerGold = 10000;
    public float spawntime;
    public int sec;
    public double min;

    public int PlayerGold//set & get�� ������ Property ����
    {
        set => playerGold = Mathf.Max(0, value);
        get => playerGold;
    }

    [SerializeField]
    private float playerMaxHP = 50;   //�ִ�ü��
    public float PlayerMaxHP => playerMaxHP;

    [SerializeField]
    private int curEnemyCount = 0;

    public int CurEnemyCount
    {
        set => curEnemyCount = Mathf.Max(0, value);
        get => curEnemyCount;
    }


    public static GameManager gameManager = null;
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameManager);
    }

    public void GameOver()
    {
        if (curEnemyCount > PlayerMaxHP)
        {

        }
        //StopCoroutine("HitAlphaAnimation");
        //StartCoroutine("HitAlphaAnimation");
    }

    //private IEnumerator HitAlphaAnimation()
    //{
    //    //��üȭ�� ũ��� ��ġ�� imageScreen�� ������ color ������ ����
    //    //imageScreen�� ������ 40%�� ����
    //    Color color = imageScreen.color;
    //    color.a = 0.4f;
    //    imageScreen.color = color;

    //    //������ 0%�� �� ������ ����
    //    while (color.a >= 0.0f)
    //    {
    //        color.a -= Time.deltaTime;
    //        imageScreen.color = color;

    //        yield return null;
    //    }
    //}

    void Update()
    {
        GameOver();

        if (spawntime > 0)
        {
            spawntime -= Time.deltaTime;
            textTime.text = string.Format("{0:F2}", spawntime);
        }

        textWave.text = waveSystem.CurrentWave.ToString();
        textPlayerGold.text = GameManager.gameManager.PlayerGold.ToString();
        textEnemyCount.text = GameManager.gameManager.CurEnemyCount + "/" + GameManager.gameManager.PlayerMaxHP;
    }
    public void PuaseBtnOnClick()
    {
        Time.timeScale = 0f;
    }
    public void X1BtnOnClick()
    {
        Time.timeScale = 1f;
    }
    public void X2BtnOnClick()
    {
        Time.timeScale = 2f;
    }
}
