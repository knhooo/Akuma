using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Player player;
    public BossAI boss;

    int curLevel;//레벨업 확인용
    [Header("플레이어")]
    [SerializeField] Image hpBar;//체력바 이미지
    [SerializeField] Image expBar;//경험치바 이미지
    [SerializeField] TextMeshProUGUI levelText;//레벨 텍스트

    [Header("보스")]
    [SerializeField] GameObject bossUI;//보스 체력 UI
    [SerializeField] Image bossHpBar;//체력바 이미지

    [Header("GameOver UI")]
    [SerializeField] GameObject gameOverUI;//게임오버 UI
    [SerializeField] GameObject gameClearUI;//게임클리어 UI
    [SerializeField] GameObject AugmentUI;//증강 UI
    [SerializeField] TextMeshProUGUI[] timeText;//생존시간
    [SerializeField] TextMeshProUGUI[] finalLevel;//달성 레벨
    [SerializeField] TextMeshProUGUI[] enemyCount;//처치한 적 수

    [Header("기타")]
    [SerializeField] GameObject Timer;//시계
    [SerializeField] GameObject spawner;//스포너
    [SerializeField] GameObject PauseUI;//일시정지UI
    [SerializeField] GameObject chest;//증강 상자
    [SerializeField] GameObject effect;//레벨업 이펙트

    void Start()
    {
        //씬에 player오브젝트가 생성되었는지 확인
        if (GameManager.instance.player != null)
        {
            player = GameManager.instance.player.GetComponent<Player>();
            curLevel = player.Level;
            spawner = GameObject.Find("Spawner(Clone)");
        }
    }
    void Update()
    {
        //씬에 boss오브젝트가 생성되었는지 확인
        if (GameManager.instance.isBoss == true)
        {
            boss = GameManager.instance.boss.GetComponent<BossAI>();
            bossUI.SetActive(true);
            //보스 체력바 업데이트
            bossHpBar.fillAmount = (float)boss.currentHP / (float)boss.maxHP;
            //보스 체력 30% 이하
            if (boss.currentHP <= boss.maxHP * 0.3f)
            {
                spawner.GetComponent<Spawner>().DestroyWall();
            }
            //보스 클리어
            if (boss.currentHP <= 0)
            {
                GameManager.instance.isBoss = false;
            }
        }
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
                GameManager.instance.pool.ClearAll();
            }
            //레벨업
            if (player.Level > curLevel)
            { 
                GameObject ob = Instantiate(effect, player.transform.position, Quaternion.identity);//이펙트 생성
                Destroy(ob, 1f);
                Instantiate(chest, player.transform.position, Quaternion.identity);//상자 생성
                curLevel = player.Level;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.isTimeStop = true;//시간 정지
            PauseUI.SetActive(true);
        }
        if(PauseUI.activeSelf == true && GameManager.instance.isTimeStop == false)
        {
            PauseUI.SetActive(false);
        }
    }

    //게임 오버
    public void GameOver()
    {
        GameManager.instance.isTimeStop = true;//시간 정지
        gameOverUI.SetActive(true);//게임 오버 UI 활성화
        SetUIText(0);
    }

    //게임 클리어
    public void GameClear()
    {
        GameManager.instance.isClear = true;
        GameManager.instance.isBoss = false;
        Invoke("SetClear", 1f);//1초 지연
    }

    public void SetClear()
    {
        bossUI.SetActive(false);
        GameManager.instance.isTimeStop = true;//시간 정지
        gameClearUI.SetActive(true);//게임 오버 UI 활성화
        SetUIText(1);
    }
    void SetUIText(int n)
    {
        //생존 시간
        float ms = Timer.GetComponent<TimeDisplay>().GetTime();
        int min = Mathf.FloorToInt(ms / 60);
        int sec = Mathf.FloorToInt(ms % 60);
        timeText[n].text = "생존 시간: " + string.Format("{0:D2}분 {1:D2}초", min, sec);

        //달성 레벨
        finalLevel[n].text = "달성 레벨: " + player.Level.ToString();

        //처치한 적
        enemyCount[n].text = "처치한 적: " + player.EnemyCount.ToString();
    }

    public void AugmentChest()
    {
        GameManager.instance.isTimeStop = true;//시간 정지
        AugmentUI.SetActive(true);
        GameManager.instance.isAugementActive = true;
    }
}