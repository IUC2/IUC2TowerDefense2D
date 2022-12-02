using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using System;


public class SoundManager : MonoBehaviour
{

    private float maxSound = 0.5f;

    [SerializeField]
    public GameObject SoundSources;

    public static SoundManager soundManager = null;
    private void Awake()
    {
        if (soundManager == null)
        {
            soundManager = this;
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
           OnAudio("BGM");
        }
        else if(SceneManager.GetActiveScene().name == "GameScene")
        {
            OnAudio("BGM");
        }
        else if(SceneManager.GetActiveScene().name == "GameOver")
        {
            OnAudio("BGM");
        }
    }

    public void PlayAudio(string audioClipName)
    {
        GameObject gameObject = Instantiate(SoundSources.transform.Find(audioClipName).gameObject);
        gameObject.GetComponent<AudioSource>().volume = maxSound * GameObject.Find("SoundInfo").GetComponent<SoundInfo>().sound;
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject, (float)(gameObject.gameObject.GetComponent<AudioSource>().clip.length + 0.1));
    }

    public void OnAudio(string audioClipName)
    {
        if(GameObject.Find(audioClipName+"(Clone)") == null)
        {
            GameObject gameObject = Instantiate(SoundSources.transform.Find(audioClipName).gameObject);
            gameObject.GetComponent<AudioSource>().volume = maxSound * GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music;
            
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().loop=true;
        }
        else
        {
            if (!GameObject.Find(audioClipName + "(Clone)").GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Find(audioClipName + "(Clone)").GetComponent<AudioSource>().volume = maxSound * GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music;
                 ///
                GameObject.Find(audioClipName + "(Clone)").GetComponent<AudioSource>().Play();
            }
        }
    }
    public void OffAudio(string audioClipName)
    {
        if (GameObject.Find(audioClipName + "(Clone)") != null && GameObject.Find(audioClipName + "(Clone)").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find(audioClipName + "(Clone)").GetComponent<AudioSource>().Stop();
        }
    }
    public void SetAudioMusicLevel()
    {
        //about music
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AudioMusic"))
        {
            obj.GetComponent<AudioSource>().volume = maxSound* GameObject.Find("SoundInfo").GetComponent<SoundInfo>().music;
        }
    }

    public void SetAudioSoundLevel()
    {
        //about sound
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Audio"))
        {
            obj.GetComponent<AudioSource>().volume = maxSound * GameObject.Find("SoundInfo").GetComponent<SoundInfo>().sound;
        }
    }
   
}
