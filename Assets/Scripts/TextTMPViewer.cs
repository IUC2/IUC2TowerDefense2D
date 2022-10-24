using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;//Text - TextMeshPro UI[�÷��̾��� ü��]
    [SerializeField]
    private PlayerHP playerHP;//�÷��̾��� ü�� ����
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;//Text - TextMeshPro UI[�÷��̾��� ���]
    [SerializeField]
    private PlayerGold playerGold;//�÷��̾��� ��� ����
    [SerializeField]
    private TextMeshProUGUI textWave;//Text - TextMeshPro UI[���� ���̺� / �� ���̺�]
    [SerializeField]
    private WaveSystem waveSystem;//���̺� ����
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;//Text - TextMeshPro UI[���� �� ���� / �� �� ����]
    [SerializeField]
    private EnemySpawner enemySpawner;//�� ����

    private void Update()
    {
        textWave.text = "WAVE" + waveSystem.CurrentWave;
        textPlayerGold.text = GameManager.gameManager.CurrentGold.ToString();
        textEnemyCount.text = "Enemy " + enemySpawner.CurrentEnemyCount + "/" + GameManager.gameManager.MaxHP;
    }
}
