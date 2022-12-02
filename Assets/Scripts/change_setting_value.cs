using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change_setting_value : MonoBehaviour
{
    [SerializeField]
    private Slider music;
    [SerializeField]
    private Slider sound;
   

    // Start is called before the first frame update
    void Start()
    {
        music.value = SoundInfo.snd.music;
        sound.value = SoundInfo.snd.sound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void change_value()
    {
        SoundInfo.snd.music = music.value;
        SoundInfo.snd.sound = sound.value;
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
