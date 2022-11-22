using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSettingMenu : MonoBehaviour
{
    public GameObject settingPanel;
    bool activeSetting = false;

    //public void Setting_button()
    //{
    //    Time.timeScale = 0;
    //    activeSetting = !activeSetting;
    //    settingPanel.SetActive(activeSetting);
    //}

    //public void Continue()
    //{
    //    Time.timeScale = 1;
    //    activeSetting = !activeSetting;
    //    settingPanel.SetActive(activeSetting);
    //}

    public void OnOff()
    {
        settingPanel.SetActive(activeSetting);

        if (activeSetting == false)
        {
            activeSetting = !activeSetting;
            Time.timeScale = 1;
        }
        else
        {
            activeSetting = !activeSetting;
            Time.timeScale = 0;
        }
    }
}
