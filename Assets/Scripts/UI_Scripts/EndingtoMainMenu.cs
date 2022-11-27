using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingtoMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void EndingtoMainMenuBtn()
    {
        BGM.bgm.DestroyBGM();
        SoundManager.soundManager.OffAudio("BGM");
        SceneManager.LoadScene("MainMenu");
    }
}
