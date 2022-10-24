using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textTime;

    float time;
    int sec;
    double min;
    void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            textTime.text = string.Format("{0:F2}", time);
        }
        else
        {
            time = 60;
            //spawn
        }

    }
}
