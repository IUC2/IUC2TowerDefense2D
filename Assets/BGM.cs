using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public static BGM bgm = null;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (bgm == null)
        {
            bgm = this;
        }
        else if (bgm != this)
        {
            Destroy(gameObject);
        }
    }
    public void DestroyBGM()
    {
        Destroy(gameObject);
    }
}
