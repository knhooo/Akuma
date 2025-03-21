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

    [SerializeField] GameObject gameOverUI;//���ӿ��� UI
    [SerializeField] TextMeshProUGUI timeText;//�����ð�
    [SerializeField] TextMeshProUGUI finalLevel;//�޼� ����
    [SerializeField] TextMeshProUGUI enemyCount;//óġ�� �� ��

    [SerializeField] GameObject Timer;//�ð�



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
            //�÷��̾� ���
            if (player.Hp <= 0)
            {
                Time.timeScale = 0f;//�ð�����
                gameOverUI.SetActive(true);//���� ���� UI Ȱ��ȭ

                //���� �ð�
                float ms = Timer.GetComponent<TimeDisplay>().GetTime();
                int min = Mathf.FloorToInt(ms / 60);
                int sec = Mathf.FloorToInt(ms % 60);
                timeText.text = "���� �ð�: " + string.Format("{0:D2}�� {1:D2}��", min, sec);

                //�޼� ����
                finalLevel.text = "�޼� ����: " + player.Level.ToString();

                //óġ�� ��

            }
        }
    }
}