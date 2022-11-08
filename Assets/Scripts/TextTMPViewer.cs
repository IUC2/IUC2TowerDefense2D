using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;//Text - TextMeshPro UI[플레이어의 체력]
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;//Text - TextMeshPro UI[플레이어의 골드]
    [SerializeField]
    private TextMeshProUGUI textWave;//Text - TextMeshPro UI[현재 웨이브 / 총 웨이브]
    [SerializeField]
    private WaveSystem waveSystem;//웨이브 정보
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;//Text - TextMeshPro UI[현재 적 숫자 / 총 적 숫자]
    [SerializeField]
    private EnemySpawner enemySpawner;//적 정보

    private void Update()
    {
        textWave.text = "WAVE" + waveSystem.CurrentWave;
        textPlayerGold.text = GameManager.gameManager.PlayerGold.ToString();
        textEnemyCount.text = "Enemy " + enemySpawner.CurrentEnemyCount + "/" + GameManager.gameManager.PlayerMaxHP;
    }
}
