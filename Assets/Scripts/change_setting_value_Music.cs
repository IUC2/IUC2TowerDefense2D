using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change_setting_value_Music : MonoBehaviour
{
    [SerializeField]
    private Slider music;
   

    // Start is called before the first frame update
    void Start()
    {
        music.value = SoundInfo.snd.music;
    }

    public void change_value()
    {
        SoundInfo.snd.music = music.value;
    }
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
}
