using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player player;
    GameObject playerObject;
    [SerializeField] Image hpBar;//ü�¹� �̹���
    [SerializeField] Image expBar;//����ġ�� �̹���
    [SerializeField] TextMeshProUGUI levelText;//���� �ؽ�Ʈ

    void Start()
    {
        //���� player������Ʈ�� �����Ǿ����� Ȯ��
        if (GameManager.instance.player != null)
        {
            player = GameManager.instance.player.GetComponent<Player>();
        }

    }
    void Update()
    {
        if (player != null)
        {
            //ü�¹� ������Ʈ
            hpBar.fillAmount = (float)player.Hp / (float)player.MaxHp;
            //����ġ�� ������Ʈ
            expBar.fillAmount = (float)player.Exp / (float)player.MaxExp;
            //���� ������Ʈ
            levelText.text = "Lv." + player.Level.ToString();
        }
    }
}