using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInfo : MonoBehaviour
{
    public static SoundInfo snd = null;

    public float music;
    public float sound;
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
