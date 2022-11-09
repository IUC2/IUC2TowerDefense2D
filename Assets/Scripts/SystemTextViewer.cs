using UnityEngine;
using TMPro;

public enum SystemType { Money, Build }

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;

    public static SystemTextViewer systemTextViewer = null;
    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();

        if (systemTextViewer == null)
        {
            systemTextViewer = this;
        }
        else if (systemTextViewer != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(systemTextViewer);
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System : Not enough money...";
                break;
            case SystemType.Build:
                textSystem.text = "System : There is tower Already build ...";
                break;
        }
        tmpAlpha.FadeOut();
    }
}
