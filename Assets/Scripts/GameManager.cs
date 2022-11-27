using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private int currentScore = 0;
    [SerializeField]
    private int maxScore = 0;
    [SerializeField]
    private TextMeshProUGUI textCurrentScore;

    public bool ispaused = false;
    public int sec;
    public float spawntime;
    public double min;

    public int PlayerGold//set & get�� ������ Property ����
    {
        set => playerGold = Mathf.Max(0, value);
        get => playerGold;
    }

    public int CurrentScore//set & get�� ������ Property ����
    {
        set => currentScore = Mathf.Max(0, value);
        get => currentScore;
    }

    public int MaxScore//set & get�� ������ Property ����
    {
        set => maxScore = Mathf.Max(0, value);
        get => maxScore;
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
        Time.timeScale = 1f;
        ispaused = false;
    }

    public void GameOver()
    {
        if (curEnemyCount > PlayerMaxHP)
        {
            GameSave();

            SceneManager.LoadScene("GameOver");
        }
    }

    void Update()
    {
        GameOver();

        if (spawntime > 0)
        {
            spawntime -= Time.deltaTime;
            textTime.text = string.Format("{0:D}",  int.Parse(Mathf.Floor(spawntime).ToString()));
        }

        textWave.text = waveSystem.CurrentWave.ToString();
        textPlayerGold.text = playerGold.ToString();
        textEnemyCount.text = curEnemyCount + "/" + playerMaxHP;
        textCurrentScore.text = currentScore.ToString();
    }
    public void PuaseBtnOnClick()
    {
        Time.timeScale = 0f;

        ispaused = true;
    }
    public void X1BtnOnClick()
    {
        Time.timeScale = 1f;
        ispaused = false;
    }
    public void X2BtnOnClick()
    {
        Time.timeScale = 2f;
        ispaused = false;
    }
    public void NextWave()
    {
        if(!ispaused)
            spawntime = 0;
    }

    public void GameSave()
    {
        Debug.Log("����");
        maxScore = currentScore;
        PlayerPrefs.SetInt("MaxScore", maxScore);
        PlayerPrefs.Save();
        currentScore = 0;
    }
    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("MaxScore"))
        {
            maxScore = 0;
            return;
        }
        maxScore = PlayerPrefs.GetInt("MaxScore");
    }
}
