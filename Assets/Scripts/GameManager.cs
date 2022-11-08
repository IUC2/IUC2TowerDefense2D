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
    public int PlayerGold//set & get이 가능한 Property 생성
    {
        set => playerGold = Mathf.Max(0, value);
        get => playerGold;
    }
    [SerializeField]
    private Image imageScreen;  //전체화면을 덮는 빨간색 이미지
    [SerializeField]
    private float playerMaxHP = 50;   //최대체력
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
    //    //전체화면 크기로 배치된 imageScreen의 색상을 color 변수에 저장
    //    //imageScreen의 투명도를 40%로 설정
    //    Color color = imageScreen.color;
    //    color.a = 0.4f;
    //    imageScreen.color = color;

    //    //투명도가 0%가 될 때까지 감소
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
