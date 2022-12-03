using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change_setting_value_Sound : MonoBehaviour
{
    [SerializeField]
    private Slider sound;
   

    // Start is called before the first frame update
    void Start()
    {
        sound.value = SoundInfo.snd.sound;
    }

    public void change_value()
    {
        SoundInfo.snd.sound = sound.value;
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
