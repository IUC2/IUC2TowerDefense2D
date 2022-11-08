using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textTime;

    public float spawntime;
    public int sec;
    public double min;

    [SerializeField]
    private int playerGold = 10000;
    public int PlayerGold//set & get�� ������ Property ����
    {
        set => playerGold = Mathf.Max(0, value);
        get => playerGold;
    }
    [SerializeField]
    private Image imageScreen;  //��üȭ���� ���� ������ �̹���
    [SerializeField]
    private float playerMaxHP = 50;   //�ִ�ü��
    public float PlayerMaxHP => playerMaxHP;


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
        //if(MaxHP > 10)
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
        if (PlayerMaxHP > 0)
        {
            GameOver();
        }

        if (spawntime > 0)
        {
            spawntime -= Time.deltaTime;
            textTime.text = string.Format("{0:F2}", spawntime);
        }
    }
}
