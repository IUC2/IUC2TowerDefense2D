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
    [SerializeField]
    private Slider light;

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
}
