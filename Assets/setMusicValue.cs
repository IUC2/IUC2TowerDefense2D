using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class setMusicValue : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Slider>().value = SoundInfo.snd.music;
    }
}
