using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon currentTower;//���� Ÿ���� ������ ������ ������ ����

    private void Awake()
    {
        OffPanel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }
    public void OnPanel(Transform towerWeapon)
    {
        //����� Ÿ�� ������ �޾ƿ� ����
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        //Ÿ�� ���� Panel On
        gameObject.SetActive(true);
        //Ÿ�� ���� ����
        UpdateTowerData();
        //Ÿ�� ��Ÿ� ǥ��(Ÿ�� ��ġ �� ��Ÿ� ǥ��)
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        //Ÿ�� ���� Panel Off
        gameObject.SetActive(false);
        //Ÿ�� ��Ÿ� ���� ����
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if(currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(90, 60);
            textDamage.text = "Damage : " + currentTower.Damage + "+" + "<color=red>" + currentTower.AddedDamage.ToString("F1") + "</color>";
        }
        else
        {
            imageTower.rectTransform.sizeDelta = new Vector2(60, 60);
            if(currentTower.WeaponType == WeaponType.Slow)
            {
                textDamage.text = "Slow : " + currentTower.Slow * 100 + "%";
            }
            else if(currentTower.WeaponType == WeaponType.Buff)
            {
                textDamage.text = "Buff : " + currentTower.Buff * 100 + "%";
            }
            
        }
        imageTower.sprite   = currentTower.TowerSprite;
        //textDamage.text     = "Damage : " + currentTower.Damage;
        textRate.text       = "Rate : " + currentTower.Rate;
        textRange.text      = "Range : " + currentTower.Range;
        textLevel.text      = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();

        //���׷��̵尡 �Ұ���������, ��ư ��Ȱ��ȭ
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
        //buttonUpgrade.enabled = false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //Ÿ�� ���׷��̵� ��ư Ŭ����
        bool isSuccess = currentTower.Upgrade();
        if(isSuccess == true)
        {
            //Ÿ���� ���׷��̵� �Ǿ��� ������ Ÿ�� ���� ����
            UpdateTowerData();
            //Ÿ�� �ֺ��� ���̴� ���� ������ ����
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            //Ÿ�� ���׷��̵忡 �ʿ��� ����� �����ϴ� ���
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        //Ÿ�� �Ǹ�
        currentTower.Sell();
        //������ Ÿ���� ������� Panel, ���ݹ��� Off
        OffPanel();
    }
}
