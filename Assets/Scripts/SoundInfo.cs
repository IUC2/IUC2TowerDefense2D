using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInfo : MonoBehaviour
{
    public static SoundInfo snd = null;
    public float music = 0.5f;
    public float sound = 0.5f;
   
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (snd == null)
        {
            snd = this;
        }
        else if (snd != this)
        {
            Destroy(gameObject);
        }
    }

}
