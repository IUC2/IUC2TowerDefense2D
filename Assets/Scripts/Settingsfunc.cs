using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settingsfunc : MonoBehaviour
{
    public void MusicDown()
    {
        if (GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music > 0.01)
        {
            GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music -= 0.1f;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().SetAudioMusicLevel();
        }
    }

    public void MusicUp()
    {
        if (GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music < 0.99)
        {
            GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music += 0.1f;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().SetAudioMusicLevel();
        }
    }

    public void SoundDown()
    {
        if (GameObject.Find("SoundInfo").GetComponent<SoundInfo>().sound > 0.01)
        {
            GameObject.Find("SoundInfo").GetComponent<SoundInfo>().sound -= 0.1f;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().SetAudioSoundLevel();
        }
    }

    public void SoundUp()
    {
        if (GameObject.Find("SoundInfo").GetComponent<SoundInfo>().sound < 0.99)
        {
            GameObject.Find("SoundInfo").GetComponent<SoundInfo>().sound += 0.1f;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().SetAudioSoundLevel();
        }
    }
}
