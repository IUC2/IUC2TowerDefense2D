using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void StartBtn()
    {
        SoundManager.soundManager.OffAudio("BGM");
        BGM.bgm.DestroyBGM();
        SceneManager.LoadScene("GameScene");
    }
}
