using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player player;
    public BossAI boss;

    int curLevel;//������ Ȯ�ο�
    [Header("�÷��̾�")]
    [SerializeField] Image hpBar;//ü�¹� �̹���
    [SerializeField] Image expBar;//����ġ�� �̹���
    [SerializeField] TextMeshProUGUI levelText;//���� �ؽ�Ʈ

    [Header("����")]
    [SerializeField] GameObject bossUI;//���� ü�� UI
    [SerializeField] Image bossHpBar;//ü�¹� �̹���

    [Header("GameOver UI")]
    [SerializeField] GameObject gameOverUI;//���ӿ��� UI
    [SerializeField] GameObject gameClearUI;//����Ŭ���� UI
    [SerializeField] GameObject AugmentUI;//���� UI
    [SerializeField] TextMeshProUGUI[] timeText;//�����ð�
    [SerializeField] TextMeshProUGUI[] finalLevel;//�޼� ����
    [SerializeField] TextMeshProUGUI[] enemyCount;//óġ�� �� ��

    [Header("��Ÿ")]
    [SerializeField] GameObject Timer;//�ð�
    [SerializeField] GameObject spawner;//������
    [SerializeField] GameObject PauseUI;//�Ͻ�����UI

    void Start()
    {
        //���� player������Ʈ�� �����Ǿ����� Ȯ��
        if (GameManager.instance.player != null)
        {
            player = GameManager.instance.player.GetComponent<Player>();
            curLevel = player.Level;
            spawner = GameObject.Find("Spawner(Clone)");
        }
    }
    void Update()
    {
        //���� boss������Ʈ�� �����Ǿ����� Ȯ��
        if (GameManager.instance.isBoss == true)
        {
            boss = GameManager.instance.boss.GetComponent<BossAI>();
            bossUI.SetActive(true);
            //���� ü�¹� ������Ʈ
            bossHpBar.fillAmount = (float)boss.currentHP / (float)boss.maxHP;
            //���� ü�� 30% ����
            if (boss.currentHP <= boss.maxHP * 0.3f)
            {
                spawner.GetComponent<Spawner>().DestroyWall();
            }
            //���� Ŭ����
            if (boss.currentHP <= 0)
            {
                GameManager.instance.isClear = true;
                GameManager.instance.isBoss = false;
                Invoke("SetClear", 1f);//1�� ����
            }
        }
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
                GameManager.instance.isTimeStop = true;//�ð� ����
                gameOverUI.SetActive(true);//���� ���� UI Ȱ��ȭ
                SetUIText(0);
            }
            //������ ����
            if (player.Level > curLevel)
            {
                GameManager.instance.isTimeStop = true;//�ð� ����
                curLevel = player.Level;
                AugmentUI.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.isTimeStop = true;//�ð� ����
            PauseUI.SetActive(true);
        }
        if(PauseUI.activeSelf == true && GameManager.instance.isTimeStop == false)
        {
            PauseUI.SetActive(false);
        }
    }

    void SetUIText(int n)
    {
        //���� �ð�
        float ms = Timer.GetComponent<TimeDisplay>().GetTime();
        int min = Mathf.FloorToInt(ms / 60);
        int sec = Mathf.FloorToInt(ms % 60);
        timeText[n].text = "���� �ð�: " + string.Format("{0:D2}�� {1:D2}��", min, sec);

        //�޼� ����
        finalLevel[n].text = "�޼� ����: " + player.Level.ToString();

        //óġ�� ��
        enemyCount[n].text = "óġ�� ��: " + player.EnemyCount.ToString();
    }

    public void SetClear()
    {
        bossUI.SetActive(false);
        GameManager.instance.isTimeStop = true;//�ð� ����
        gameClearUI.SetActive(true);//���� ���� UI Ȱ��ȭ
        SetUIText(1);
    }
}