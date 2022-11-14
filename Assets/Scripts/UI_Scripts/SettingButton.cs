using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
{
    public void SettingBtn()
    {
        SceneManager.LoadScene("Setting");
    }
}