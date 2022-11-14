using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMainMenu : MonoBehaviour
{
    public void GotoMainMenuBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
