using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player player;
    GameObject playerObject;
    [SerializeField] Image hpBar;//체력바 이미지
    [SerializeField] Image expBar;//경험치바 이미지
    [SerializeField] TextMeshProUGUI levelText;//레벨 텍스트

    [SerializeField] GameObject gameOverUI;//게임오버 UI
    [SerializeField] GameObject gameClearUI;//게임클리어 UI
    [SerializeField] GameObject AugmentUI;//증강 UI
    [SerializeField] TextMeshProUGUI timeText;//생존시간
    [SerializeField] TextMeshProUGUI finalLevel;//달성 레벨
    [SerializeField] TextMeshProUGUI enemyCount;//처치한 적 수

    [SerializeField] GameObject Timer;//시계

    void Start()
    {
        //씬에 player오브젝트가 생성되었는지 확인
        if (GameManager.instance.player != null)
        {
            player = GameManager.instance.player.GetComponent<Player>();
        }

    }
    void Update()
    {
        if (player != null)
        {
            //체력바 업데이트
            hpBar.fillAmount = (float)player.Hp / (float)player.MaxHp;
            //경험치바 업데이트
            expBar.fillAmount = (float)player.Exp / (float)player.MaxExp;
            //레벨 업데이트
            levelText.text = "Lv." + player.Level.ToString();
            //플레이어 사망
            if (player.Hp <= 0)
            {
                Time.timeScale = 0f;//시간정지
                gameOverUI.SetActive(true);//게임 오버 UI 활성화
                SetUIText();
            }
            //보스 클리어
            if (GameManager.instance.isClear == true)
            {
                Time.timeScale = 0f;//시간정지
                gameClearUI.SetActive(true);//게임 오버 UI 활성화
                SetUIText();
            }
            //레벨업 증강
            if(player.Exp == player.MaxExp)
            {
                Time.timeScale = 0f;//시간정지
                AugmentUI.SetActive(true);
            }
        }
    }

    void SetUIText()
    {
        //생존 시간
        float ms = Timer.GetComponent<TimeDisplay>().GetTime();
        int min = Mathf.FloorToInt(ms / 60);
        int sec = Mathf.FloorToInt(ms % 60);
        timeText.text = "생존 시간: " + string.Format("{0:D2}분 {1:D2}초", min, sec);

        //달성 레벨
        finalLevel.text = "달성 레벨: " + player.Level.ToString();

        //처치한 적
        enemyCount.text = "처치한 적: " + player.EnemyCount.ToString();
    }
}